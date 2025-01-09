using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TrullFight : MonoBehaviour
{

    public int health;
    public float speed;
    public bool canHit;
    public bool canKill;
    public bool killed;

    public AudioSource audio;
    public NavMeshAgent agent;
    public AudioClip hit;
    public AudioClip speak1;
    public AudioClip speak2;

    public bool fightStart;
    public GameControllerScript gc;
    public GameObject projectileSet1;
    public GameObject projectileSet2;
    public GameObject projectileSet3;
    public GameObject projectileSet4;
    public ProjectileSpawner spawner;

    public GameObject playerHUD;
    public GameObject bossHUD;
    public GameObject flashbang;
    public GameObject whiteFlashbang;
    public GameObject tullWhite;
    public GameObject healthBarBG;
    public Slider healthBar;
    public Volume volume;
    Bloom bloom;

    public Material inivisible;
    public List<MeshRenderer> toSet;

    public bool musicPhase1;
    public bool musicPhase2;
    public float stopTimer;
    public float gameOverDelay = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && canHit)
        {
            ProjectileScript projectile = other.GetComponent<ProjectileScript>();
            if (!projectile.thrown || projectile.pickedUp) return;
            if (projectile.heals)
            {
                health += 1;
            }
            else
                Hurt();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player") && canKill)
        {
            if (killed) return;
            killed = true;
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        AudioListener.pause = true;
        bossHUD.SetActive(true);
    }

    private void Start()
    {
        volume.profile.TryGet<Bloom>(out bloom);
        volume.weight = 1f;

        gc.trullFightMusic.PlayOneShot(gc.trullBegin);
        musicPhase1 = true;

        gc.player.maxStamina = 0;
        gc.player.stamina = 0;
        gc.player.staminaRate = 0;
        playerHUD.SetActive(false);
        flashbang.SetActive(true);

        canHit = true;
        audio.PlayOneShot(speak1);
        stopTimer = float.PositiveInfinity;
    }

    private void Update()
    {
        /*if (gameOverDelay > 0f)
        {
            gameOverDelay -= Time.deltaTime;
            if (gameOverDelay <= 0f)
            {
                try
                {
                    System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=V-RDWBhe9PY");
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning(e.Message);
                }
                Application.Quit();
            }
        }*/

        if (stopTimer > 0f)
        {
            stopTimer -= Time.deltaTime;
            agent.isStopped = stopTimer > 0f;
            canKill = stopTimer <= 0f;
        }
        else
        {
            agent.isStopped = false;
            canKill = true;
        }

        if (!fightStart)
        {
            if (musicPhase1)
            {
                if (!gc.trullFightMusic.isPlaying)
                    gc.trullFightMusic.Play();
            }
        }
        else
        {
            if (musicPhase2)
            {
                if (!gc.trullFightMusic.isPlaying)
                    gc.trullFightMusic.Play();
            }

        }
        speed = gc.player.walkSpeed + 0.5f;
        if (fightStart)
            agent.speed = speed;
        agent.SetDestination(gc.player.transform.position);
        healthBar.value = health;
    }

    void Hurt()
    {
        if (!fightStart)
        {
            DOTween.KillAll();
            canHit = false;
            audio.Stop();
            audio.PlayOneShot(hit);
            audio.PlayOneShot(speak2);
            musicPhase1 = false;
            gc.trullFightMusic.Stop();
            gc.trullFightMusic.clip = gc.trullLoop;
            gc.trullFightMusic.PlayOneShot(gc.trullHit);
            musicPhase2 = true;
            StartCoroutine(StartFight());
            projectileSet1.SetActive(false);
            projectileSet2.SetActive(false);
            projectileSet3.SetActive(false);
            projectileSet4.SetActive(false);
            HurtEffects();
            return;
        }

        health -= 1;
        gc.player.walkSpeed += 4f;
        stopTimer = 2f;
        gc.trullFightMusic.pitch += 0.025f;
        audio.PlayOneShot(hit);
        audio.PlayOneShot(gc.glass[UnityEngine.Random.Range(0,gc.glass.Count-1)]);
        HurtEffects();

        switch (health)
        {
            default: return;
            case 0:
                SceneManager.LoadScene("TullResults");
                return;
            case 1:
                gc.trullFightMusic.Stop();
                gc.trullFightMusic.clip = gc.trullFinal;
                gc.trullFightMusic.Play();
                return;
            case 10:
                RenderSettings.skybox = gc.spaceSkybox;
                foreach(MeshRenderer renderer in toSet)
                {
                    renderer.material = inivisible;
                }
                return;

        }
    }

    IEnumerator StartFight()
    {
        yield return new WaitForSeconds(gc.trullHit.length);
        fightStart = true;
        canHit = true;
        stopTimer = 0f;
        whiteFlashbang.SetActive(true);
        spawner.gameObject.SetActive(true);
        healthBarBG.gameObject.SetActive(true);
    }

    void HurtEffects()
    {
        // todo: hurt effects
        tullWhite.SetActive(false);
        DOVirtual.Float(0.1f, 3f, 3f, b => bloom.threshold.value = b).SetEase(Ease.OutCubic);
        tullWhite.SetActive(true);
    }
}
