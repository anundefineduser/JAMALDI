using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000C0 RID: 192
public class GameControllerScript : MonoBehaviour
{
	public Material blackSkybox;
    public Material spaceSkybox;

    [Space]
	public GameObject trullFight;
	public AudioSource trullFightMusic;
	public AudioClip trullBegin;
	public AudioClip trullHit;
	public AudioClip trullLoop;
	public AudioClip trullEnd;
    public AudioClip trullFinal;

    [Space]
	public List<AudioSource> toPause;
    public List<AudioClip> crowbarNoises;
	public GameObject crowbarText;
	public AudioClip demoSound;
	public Sprite trull;
	public GameObject mangoSound;

    private void Awake()
    {
		//this.mode = "trull";
    }

    // Token: 0x06000964 RID: 2404 RVA: 0x00021AC4 File Offset: 0x0001FEC4
    private void Start()
	{
		this.cullingMask = this.PlayerCamera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.audioQueue = base.GetComponent<AudioQueueScript>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode
		if (this.mode == "endless") //If it is endless mode
		{
			this.baldiScrpt.endless = true; //Set Baldi use his slightly changed endless anger system
			this.AAC.endless = true; //Set Arts && Crafter anger to be infinite
		}
		this.LockMouse(); //Prevent the mouse from moving
		this.UpdateNotebookCount(); //Update the notebook count
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)
		this.gameOverDelay = 0.5f;
		if (this.mode == "trull")
		{
			SwitchBaldi(trull, Color.white, Vector3.one, null);
			baldiTutor.SetActive(false);
			RenderSettings.ambientLight = Color.white * 0.35f;
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
			RenderSettings.fogDensity = 0.1f;
			RenderSettings.skybox = blackSkybox;
			mangoSound.SetActive(true);
        }
        else
            this.schoolMusic.Play(); //Play the school music
    }

	// Token: 0x06000965 RID: 2405 RVA: 0x00021B5C File Offset: 0x0001FF5C
	private void Update()
	{
		if (!this.learningActive)
		{
			if (Singleton<InputManager>.Instance.GetActionKey(InputAction.PauseOrCancel))
			{
				if (!this.gamePaused)
				{
					this.PauseGame();
				}
				else
				{
					this.UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) && this.gamePaused)
			{
				this.ExitGame();
			}
			else if (Input.GetKeyDown(KeyCode.N) && this.gamePaused)
			{
				this.UnpauseGame();
			}
			if (!this.gamePaused && Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			if ((Input.GetMouseButtonDown(1) || Singleton<InputManager>.Instance.GetActionKey(InputAction.UseItem)) && Time.timeScale != 0f)
			{
				this.UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                this.ChangeItemSelection(-1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                this.ChangeItemSelection(1);
            }
			for (int i = 0; i < this.item.Length; i++)
            {
                bool keyCode = Singleton<InputManager>.Instance.GetActionKey(InputAction.Slot0 + 0 + i);
                if ((keyCode))
                {
                    this.itemSelected = i;
                    this.UpdateItemSelection();
                    break;
                }
            }
		}
		else
		{
			if (notebooks != 1)
			{
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = 0f;
                }
            }
		}

		if (charginTimer > 0f)
		{
			player.movementOffset = playerTransform.forward * 125f * Time.deltaTime;
			charginTimer -= Time.deltaTime;
			if (charginTimer <= 0f)
            {
                player.movementOffset = Vector3.zero;
            }
		}

		if (this.player.gameOver)
		{
			if (this.mode == "endless" && this.notebooks > PlayerPrefs.GetInt("HighBooks") && !this.highScoreText.activeSelf)
			{
				this.highScoreText.SetActive(true);
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.PlayerCamera.farClipPlane = this.gameOverDelay * 400f; //Set camera farClip 
			this.audioDevice.PlayOneShot(this.aud_buzz);
			if (PlayerPrefs.GetInt("Rumble") == 1)
			{

			}
			if (this.gameOverDelay <= 0f)
			{
				if (this.mode == "endless")
				{
					if (this.notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", this.notebooks);
					}
					PlayerPrefs.SetInt("CurrentBooks", this.notebooks);
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene("GameOver");
			}
		}
		if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3 && mode != "trull")
		{
			this.audioDevice.clip = this.aud_MachineLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00021F8C File Offset: 0x0002038C
	private void UpdateNotebookCount()
	{
        this.notebookCount.text = this.notebooks.ToString();
        if (this.notebooks == this.maxNotebooks && this.mode != "endless")
		{
			this.ActivateFinaleMode();
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00022024 File Offset: 0x00020424
	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0002203A File Offset: 0x0002043A
	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor(); //Prevent the cursor from moving
			this.mouseLocked = true;
			this.reticle.SetActive(true);
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00022065 File Offset: 0x00020465
	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor(); //Allow the cursor to move
		this.mouseLocked = false;
		this.reticle.SetActive(false);
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00022085 File Offset: 0x00020485
	public void PauseGame()
	{
		if (!this.learningActive)
		{
			{
				this.UnlockMouse();
			}
			Time.timeScale = 0f;
			this.gamePaused = true;
			AudioListener.pause = true;
			this.pauseMenu.SetActive(true);
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000220C5 File Offset: 0x000204C5
	public void ExitGame()
	{
		AudioListener.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x000220D1 File Offset: 0x000204D1
	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		this.gamePaused = false;
		AudioListener.pause = false;
		this.pauseMenu.SetActive(false);
		this.LockMouse();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x000220F8 File Offset: 0x000204F8
	public void ActivateSpoopMode()
	{
		this.spoopMode = true; //Tells the game its time for spooky
		this.entrance_0.Lower(); //Lower all the exits
		this.entrance_1.Lower();
		this.entrance_2.Lower();
		this.entrance_3.Lower();
		this.baldiTutor.SetActive(false); //Turns off Baldi(The one that you see at the start of the game)
        this.baldi.SetActive(true); //Turns on Baldi
        //this.quarter.SetActive(false);
        if (mode != "trull")
		{
            this.principal.SetActive(true); //Turns on Principal
            this.crafters.SetActive(true); //Turns on Crafters
            this.playtime.SetActive(true); //Turns on Playtime
            this.gottaSweep.SetActive(true); //Turns on Gotta Sweep
            this.bully.SetActive(true); //Turns on Bully
            this.firstPrize.SetActive(true); //Turns on First-Prize
            this.TestEnemy.SetActive(true); //Turns on Test-Enemy
            this.audioDevice.PlayOneShot(this.aud_Hang); //Plays the hang sound
        }
		this.learnMusic.Stop(); //Stop all the music
		this.schoolMusic.Stop();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x000221BF File Offset: 0x000205BF
	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.entrance_0.Raise(); //Raise all the enterances(make them appear)
		this.entrance_1.Raise();
		this.entrance_2.Raise();
		this.entrance_3.Raise();
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x000221F4 File Offset: 0x000205F4
	public void GetAngry(float value) //Make Baldi get angry
	{
		if (!this.spoopMode)
		{
			this.ActivateSpoopMode();
		}
		this.baldiScrpt.GetAngry(value);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00022214 File Offset: 0x00020614
	public void ActivateLearningGame()
	{
		//this.camera.cullingMask = 0; //Sets the cullingMask to nothing
		this.learningActive = true;
		this.UnlockMouse(); //Unlock the mouse
		this.tutorBaldi.Stop(); //Make tutor Baldi stop talking
		if (mode != "trull")
		{
            if (!this.spoopMode) //If the player hasn't gotten a question wrong
            {
                base.StartCoroutine(audioQueue.FadeOut(this.schoolMusic, 0.25f)); //Fade out the school music
                if (this.notebooks == 1)
                    this.learnMusic.Play(); //Start playing the learn music
            }
        }
		foreach (AudioSource source in toPause)
			source.Pause();
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00022278 File Offset: 0x00020678
	public void DeactivateLearningGame(GameObject subject)
	{
		this.PlayerCamera.cullingMask = this.cullingMask; //Sets the cullingMask to Everything
		this.learningActive = false;
		UnityEngine.Object.Destroy(subject);
		this.LockMouse(); //Prevent the mouse from moving
		if (this.player.stamina < 100f) //Reset Stamina
		{
			this.player.stamina = 100f;
		}
		if (mode != "trull")
		{
            if (!this.spoopMode) //If it isn't spoop mode, play the school music
            {
                this.schoolMusic.Play();
                base.StartCoroutine(audioQueue.FadeOut(this.learnMusic, 0.25f));
            }
        }
		if (this.notebooks == 1 && !this.spoopMode) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks == this.maxNotebooks && this.mode == "story") // Plays the all 7 notebook sound
		{
			this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
		}

        foreach (AudioSource source in toPause)
            source.UnPause();
    }

	// Token: 0x06000972 RID: 2418 RVA: 0x00022360 File Offset: 0x00020760
	private void ChangeItemSelection(int change)
    {
        this.itemSelected += change;
        if (this.itemSelected < 0)
        {
            this.itemSelected = (this.item.Length - 1);
        }
        if (this.itemSelected > (this.item.Length - 1))
        {
            this.itemSelected = 0;
        }
        this.UpdateItemSelection();
    }

	// Token: 0x06000974 RID: 2420 RVA: 0x00022425 File Offset: 0x00020825
	private void UpdateItemSelection()
    {
        for (int i = 0; i < this.itemSlotBgs.Length; i++)
        {
            if (this.itemSlotBgs[i] != this.itemSlotBgs[this.itemSelected])
            {
                this.itemSlotBgs[i].color = new Color(1f, 1f, 0f, 0.5f);
            }
            else
            {
                this.itemSlotBgs[i].color = new Color(1f,1f, 0f, 1f);
            }
        }
        this.UpdateItemName();
    }

	// Token: 0x06000975 RID: 2421 RVA: 0x0002245C File Offset: 0x0002085C
	public void CollectItem(int item_ID)
    {
        for (int i = 0; i < this.item.Length; i++)
        {
            if (this.item[i] == 0)
            {
                this.item[i] = item_ID;
                this.itemSlot[i].texture = this.itemTextures[item_ID];
                this.UpdateItemName();
                return;
            }
        }
        this.item[itemSelected] = item_ID;
        this.itemSlot[itemSelected].texture = this.itemTextures[item_ID];
        this.UpdateItemName();
    }

	// Token: 0x06000976 RID: 2422 RVA: 0x00022528 File Offset: 0x00020928
	private void UseItem()
	{
		if (this.item[this.itemSelected] != 0)
		{
			if (this.item[this.itemSelected] == 1)
			{
				this.player.stamina = this.player.maxStamina * 2f;
				this.audioDevice.PlayOneShot(this.aud_Crunch);
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 2)
			{
				Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" && Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
				{
					raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 3)
			{
				Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit2;
				if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" && Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
				{
					DoorScript component = raycastHit2.collider.gameObject.GetComponent<DoorScript>();
					if (component.DoorLocked)
					{
						component.UnlockDoor();
						component.OpenDoor();
						this.ResetItem();
					}
				}
			}
			else if (this.item[this.itemSelected] == 4)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
				this.ResetItem();
				this.player.ResetGuilt("drink", 1f);
				this.audioDevice.PlayOneShot(this.aud_Soda);
			}
			else if (this.item[this.itemSelected] == 5)
			{
				Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit3;
				if (Physics.Raycast(ray3, out raycastHit3))
				{
					if (raycastHit3.collider.name == "BSODAMachine" && Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						CollectItem(4);
						if (this.useEmptyMachine)
						{
							raycastHit3.collider.gameObject.name = "EmptyMachine";
							raycastHit3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfBsoda;
						}
					}
					else if (raycastHit3.collider.name == "ZestyMachine" && Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						CollectItem(1);
						if (this.useEmptyMachine)
						{
							raycastHit3.collider.gameObject.name = "EmptyMachine";
							raycastHit3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfZesty;
						}
					}
					else if (raycastHit3.collider.name == "PayPhone" && Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						ResetItem();
					}
				}
			}
			else if (this.item[this.itemSelected] == 6)
			{
				Ray ray4 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit4;
				if (Physics.Raycast(ray4, out raycastHit4) && (raycastHit4.collider.name == "TapePlayer" && Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
				{
					raycastHit4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 7)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
				gameObject.GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 8)
			{
				Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit5;
				bool hit = Physics.Raycast(ray5, out raycastHit5);

                if (hit && (raycastHit5.collider.tag == "Door" && Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
				{
					raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
					this.ResetItem();
					this.audioDevice.PlayOneShot(this.aud_Spray);
				}
				else if (hit && (raycastHit5.collider.gameObject == wall.gameObject && Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
                {
                    this.ResetItem();
					this.CrackWall();
                    this.audioDevice.PlayOneShot(this.aud_Spray);
                }

            }
			else if (this.item[this.itemSelected] == 9)
			{
				Ray ray6 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit6;
				if (this.player.jumpRope)
				{
					this.player.DeactivateJumpRope();
					this.playtimeScript.Disappoint();
					this.audioDevice.PlayOneShot(this.aud_Snip);
					this.ResetItem();
				}
				else if (Physics.Raycast(ray6, out raycastHit6) && raycastHit6.collider.name == "1st Prize")
				{
					this.firstPrizeScript.GoCrazy();
					this.audioDevice.PlayOneShot(this.aud_Snip);
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 10)
			{
				this.player.ActivateBoots();
				//base.StartCoroutine(this.BootAnimation());
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 11)
			{
				//Debug.Log("12");
				Ray ray7 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit7;
				if (Physics.Raycast(ray7, out raycastHit7))
				{
					//Debug.Log("13");
					WindowScript window = raycastHit7.collider.GetComponent<WindowScript>();
					Debug.Log(window != null);
					if (window != null && !window.isBroken && Vector3.Distance(this.playerTransform.position, raycastHit7.transform.position) <= 10f)
					{
						//Debug.Log("14");
						window.Break();
						this.ResetItem();
						audioDevice.PlayOneShot(crowbarNoises[UnityEngine.Random.Range(0, crowbarNoises.Count - 1)]);
						return;
					}

					if (raycastHit7.collider.gameObject == wall.gameObject && Vector3.Distance(this.playerTransform.position, raycastHit7.transform.position) <= 10f && wall.currentCrack == 1)
					{
						CrackWall();
						this.ResetItem();
						audioDevice.PlayOneShot(crowbarNoises[UnityEngine.Random.Range(0, crowbarNoises.Count - 1)]);
					}
				}
			}
			else if (this.item[this.itemSelected] == 12)
			{
				audioDevice.PlayOneShot(demoSound);
				charginTimer = demoSound.length;
				ResetItem();
            }
            else if (this.item[this.itemSelected] == 13 || this.item[this.itemSelected] == 14)
            {
                Ray ray8 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
                RaycastHit raycastHit8;
                if (Physics.Raycast(ray8, out raycastHit8) && (raycastHit8.collider.CompareTag("Wall") || raycastHit8.collider.CompareTag("Window") || raycastHit8.collider.CompareTag("Door") || raycastHit8.collider.CompareTag("SwingingDoor")))
                {
					GameObject portal;

                    if (this.item[this.itemSelected] == 13)
                    {
						if (currentBluePortal != null)
							Destroy(currentBluePortal);
                        portal = Instantiate(bluePortal);
                        portal.name = "BluePortal";
                        currentBluePortal = portal;
                        portal.transform.position = raycastHit8.point;
                        portal.transform.rotation = Quaternion.LookRotation(-raycastHit8.normal);
                        portal.transform.position -= portal.transform.forward * 0.05f;
                        ResetItem();
                        this.item[this.itemSelected] = 14;
                        UpdateItemSelection();
                        this.itemSlot[this.itemSelected].texture = this.itemTextures[14];
                        return;
                    }
                    if (currentOrangePortal != null)
                        Destroy(currentOrangePortal);
                    portal = Instantiate(orangePortal);
                    portal.name = "OrangePortal";
                    currentOrangePortal = portal;
                    portal.transform.position = raycastHit8.point;
                    portal.transform.rotation = Quaternion.LookRotation(-raycastHit8.normal);
                    portal.transform.position -= portal.transform.forward * 0.05f;
                    ResetItem();
                }
            }
        }
	}

	public float charginTimer;
	public GameObject bluePortal;
	public GameObject orangePortal;
    public GameObject currentBluePortal;
    public GameObject currentOrangePortal;

    // Token: 0x06000977 RID: 2423 RVA: 0x00022B40 File Offset: 0x00020F40
    /*private IEnumerator BootAnimation()
    {
        float time = 15f;
        float height = 1080;
        Vector3 position = default(Vector3);
        this.boots.gameObject.SetActive(true);
        while (height > -1080)
        {
            height -= 1080 * Time.deltaTime;
            time -= Time.deltaTime;
            position = this.boots.localPosition;
            position.y = height;
            this.boots.localPosition = position;
            yield return null;
        }
        position = this.boots.localPosition;
        position.y = -1080;
        this.boots.localPosition = position;
        this.boots.gameObject.SetActive(false);
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        this.boots.gameObject.SetActive(true);
        while (height < 1080)
        {
            height += 1080 * Time.deltaTime;
            position = this.boots.localPosition;
            position.y = height;
            this.boots.localPosition = position;
            yield return null;
        }
        position = this.boots.localPosition;
        position.y = 1080;
        this.boots.localPosition = position;
        this.boots.gameObject.SetActive(false);
        yield break;
    }*/

    // Token: 0x06000978 RID: 2424 RVA: 0x00022B5B File Offset: 0x00020F5B
    private void ResetItem()
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00022B8B File Offset: 0x00020F8B
	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00022BB1 File Offset: 0x00020FB1
	private void UpdateItemName()
	{
		this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00022BD4 File Offset: 0x00020FD4
	public void ExitReached(Transform triggerPos = null, int id = 0)
	{
		this.exitsReached++;
        if (this.exitsReached == 1)
		{
			//RenderSettings.fog = true;
			this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
            if (mode != "trull")
            {
                RenderSettings.ambientLight = Color.red; //Make everything red and start player the weird sound
                RenderSettings.skybox = skyBoxRed; //Make skybox red
                RenderSettings.fogDensity = 0.02f;
                RenderSettings.fog = true;
                this.audioDevice.clip = this.aud_MachineQuiet;
                this.audioDevice.loop = true;
                this.audioDevice.Play();
            }
		}
		if (this.exitsReached == 2) //Play a sound
		{
			if (mode != "trull")
			{
                this.audioDevice.volume = 0.8f;
                this.audioDevice.clip = this.aud_MachineStart;
                this.audioDevice.loop = true;
                this.audioDevice.Play();
            }
            else
                this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
        }
		if (this.exitsReached == 3) //Play a even louder sound
		{
			if (mode != "trull")
			{
				this.audioDevice.clip = this.aud_MachineRev;
				this.audioDevice.loop = false;
				this.audioDevice.Play();
			}
			else
			{
				this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
				baldi.SetActive(false);
			}
        }
		if (this.exitsReached == 4)
        {
            this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
			trullFight.transform.position = triggerPos.position + triggerPos.forward*7f;
            this.trullFight.SetActive(true);
			TrullFight fight = trullFight.GetComponent<TrullFight>();
            LoseItem(0);
            LoseItem(1);
            LoseItem(2);
            if (currentBluePortal)
                Destroy(currentBluePortal);
            if (currentOrangePortal)
                Destroy(currentOrangePortal);
			currentOrangePortal = null;
			currentBluePortal = null;
            switch (id)
            {
                case 0:
                    fight.projectileSet1.SetActive(true); break;
                case 1:
                    fight.projectileSet2.SetActive(true); break;
                case 2:
                    fight.projectileSet3.SetActive(true); break;
                case 3:
                    fight.projectileSet4.SetActive(true); break;
            }
        }
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00022CC1 File Offset: 0x000210C1
	public void DespawnCrafters()
	{
		this.crafters.SetActive(false); //Make Arts And Crafters Inactive
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00022CD0 File Offset: 0x000210D0
	public void Fliparoo()
	{
		this.player.height = 6f;
		this.player.fliparoo = 180f;
		this.player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}

	// Token: 0x040005F7 RID: 1527
	public CursorControllerScript cursorController;

	// Token: 0x040005F8 RID: 1528
	public PlayerScript player;

	// Token: 0x040005F9 RID: 1529
	public Transform playerTransform;

	// Token: 0x040005FA RID: 1530
	public Transform cameraTransform;

	// Token: 0x040005FB RID: 1531
	public Camera PlayerCamera;

	// Token: 0x040005FC RID: 1532
	private int cullingMask;

	// Token: 0x040005FD RID: 1533
	public EntranceScript entrance_0;

	// Token: 0x040005FE RID: 1534
	public EntranceScript entrance_1;

	// Token: 0x040005FF RID: 1535
	public EntranceScript entrance_2;

	// Token: 0x04000600 RID: 1536
	public EntranceScript entrance_3;

	// Token: 0x04000601 RID: 1537
	public GameObject baldiTutor;

	// Token: 0x04000602 RID: 1538
	public GameObject baldi;

	// Token: 0x04000603 RID: 1539
	public BaldiScript baldiScrpt;

	// Token: 0x04000604 RID: 1540
	public AudioClip aud_Prize;

	// Token: 0x04000606 RID: 1542
	public AudioClip aud_AllNotebooks;

	// Token: 0x04000607 RID: 1543
	public GameObject principal;

	// Token: 0x04000608 RID: 1544
	public GameObject crafters;

	// Token: 0x04000609 RID: 1545
	public GameObject playtime;

	// Token: 0x0400060A RID: 1546
	public PlaytimeScript playtimeScript;

	// Token: 0x0400060B RID: 1547
	public GameObject gottaSweep;

	// Token: 0x0400060C RID: 1548
	public GameObject bully;

	// Token: 0x0400060D RID: 1549
	public GameObject firstPrize;

	// Token: 0x0400060D RID: 1549
	public GameObject TestEnemy;

	// Token: 0x0400060E RID: 1550
	public FirstPrizeScript firstPrizeScript;

	// Token: 0x0400060F RID: 1551
	public GameObject quarter;

	// Token: 0x04000610 RID: 1552
	public AudioSource tutorBaldi;

	// Token: 0x04000611 RID: 1553
	public RectTransform boots;

	// Token: 0x04000612 RID: 1554
	public string mode;

	// Token: 0x04000613 RID: 1555
	public int notebooks;

	// Token: 0x04000724 RID: 8723
	public int maxNotebooks;

	// Token: 0x04000614 RID: 1556
	public GameObject[] notebookPickups;

	// Token: 0x04000615 RID: 1557
	public int failedNotebooks;

	// Token: 0x04000616 RID: 1558
	public bool spoopMode;

	// Token: 0x04000617 RID: 1559
	public bool finaleMode;

	// Token: 0x04000618 RID: 1560
	public bool debugMode;

	// Token: 0x04000619 RID: 1561
	public bool mouseLocked;

	// Token: 0x0400061A RID: 1562
	public int exitsReached;

	// Token: 0x0400061B RID: 1563
	public int itemSelected;

	// Token: 0x0400061C RID: 1564
	public int[] item = new int[3];

	// Token: 0x0400061D RID: 1565
	public RawImage[] itemSlot = new RawImage[3];

	// Token: 0x0400061E RID: 1566
	public string[] itemNames = new string[]
	{
		"Nothing",
		"Feastables Bar",
		"Swinging Door Lock",
		"Principal's Keys",
		"BSODA",
		"Quarter",
        "Crazy Frog Arcade Racer",
		"Alarm Clock",
		"Galaxy Gas",
        "Xbox 360 Controller",
		"Squeaky Boots",
        "Crowbar",
        "Chargin' Targe",
		"Portal Gun (B)",
        "Portal Gun (O)",
    };

    public List<string> itemGameObjectNames;
    public List<Sprite> itemSprites;

    // Token: 0x0400061F RID: 1567
    public TMP_Text itemText;

	// Token: 0x04000620 RID: 1568
	public UnityEngine.Object[] items = new UnityEngine.Object[10];

	// Token: 0x04000621 RID: 1569
	public Texture[] itemTextures = new Texture[10];

	// Token: 0x04000622 RID: 1570
	public GameObject bsodaSpray;

	// Token: 0x04000623 RID: 1571
	public GameObject alarmClock;

	// Token: 0x04000624 RID: 1572
	public TMP_Text notebookCount;

	// Token: 0x04000625 RID: 1573
	public GameObject pauseMenu;

	// Token: 0x04000626 RID: 1574
	public GameObject highScoreText;

	// Token: 0x04000628 RID: 1576
	public GameObject reticle;

	// Token: 0x0400062A RID: 1578
	public RawImage[] itemSlotBgs = new RawImage[3];

	// Token: 0x0400062B RID: 1579
	private bool gamePaused;

	// Token: 0x0400062C RID: 1580
	private bool learningActive;

	// Token: 0x0400062D RID: 1581
	private float gameOverDelay;

	// Token: 0x0400062E RID: 1582
	public AudioSource audioDevice { get; private set; }

	// Token: 0x0400062F RID: 1583
	public AudioClip aud_Soda;

	// Token: 0x04000630 RID: 1584
	public AudioClip aud_Spray;

	// Token: 0x04000631 RID: 1585
	public AudioClip aud_buzz;

	// Token: 0x04000632 RID: 1586
	public AudioClip aud_Hang;

	// Token: 0x04000633 RID: 1587
	public AudioClip aud_MachineQuiet;

	// Token: 0x04000634 RID: 1588
	public AudioClip aud_MachineStart;

	// Token: 0x04000635 RID: 1589
	public AudioClip aud_MachineRev;

	// Token: 0x04000636 RID: 1590
	public AudioClip aud_MachineLoop;

	// Token: 0x04000637 RID: 1591
	public AudioClip aud_Switch;

	// Token: 0x04000638 RID: 1592
	public AudioSource schoolMusic;

	// Token: 0x04000639 RID: 1593
	public AudioSource learnMusic;

	// Token: 0x0400063A RID: 1594
	public Material skyBoxRed;

	// Token: 0x0400063B RID: 1595
	public CraftersScript AAC;

	// Token: 0x0400063C RID: 1596
	public AudioClip aud_Crunch;

	// Token: 0x0400063D RID: 1597
	public AudioClip aud_Snip;

	// Token: 0x0400063E RID: 1598
	public AudioClip aud_Drop;

	// Token: 0x0400063F RID: 1599
	public Texture[] BigItemTextures = new Texture[10];

	// Token: 0x0400063G RID: 1600
	public Material OutOfBsoda;

	// Token: 0x0400063H RID: 1601
	public Material OutOfZesty;

	// Token: 0x0400063I RID: 1602
	private AudioQueueScript audioQueue;

	// Token: 0x0400063J RID: 1603
	public bool useEmptyMachine = false;

	// Token: 0x0400063K RID: 1604
	//private Player playerInput;

	public void CrackWall()
	{
		wall.Crack();
		if (wall.currentCrack <= wall.cracks.Capacity+1)
			audioDevice.PlayOneShot(glass[UnityEngine.Random.Range(0, glass.Count - 1)]);
    }
	public CrackedWall wall;
	public List<AudioClip> glass;
	public TrullJumpscare toJumpscare;

	void SwitchBaldi(Sprite newSprite, Color lightColor, Vector3 spriteScale, Animator animator = null, List<AudioClip> slap = null)
	{
		baldiScrpt.baldiAnimator.GetComponent<SpriteRenderer>().sprite = newSprite;
        baldiScrpt.baldiAnimator.transform.localScale = spriteScale;
		if (animator == null && baldiScrpt.baldiAnimator)
			baldiScrpt.baldiAnimator.enabled = false;
        baldiScrpt.baldiAnimator = animator;
		baldiScrpt.GetComponentInChildren<Light>().color = lightColor;
		baldiScrpt.slap = slap;
    }
}
