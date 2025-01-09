using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public static ProjectileScript current;

    public AudioSource audio;
    public BsodaSparyScript spary;
    public Rigidbody rb;
    public bool pickedUp;
    public bool thrown;
    public bool heals;
    public ProjectileSpawner spawner;
    
    PlayerScript player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!pickedUp && !thrown && current == null)
            {
                pickedUp = true;
                current = this;
                player = other.GetComponent<PlayerScript>();
            }
        }
    }

    private void LateUpdate()
    {
        if (current == this)
        {
            transform.position = player.transform.position + player.gc.PlayerCamera.transform.forward*4f;
            transform.rotation = player.gc.PlayerCamera.transform.rotation;
            if (Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact))
            {
                current = null;
                spary.enabled = true;
                thrown = true;
                pickedUp = false;
                audio.Play();
                if (spawner)
                {
                    spawner.objects -= 1;
                }
            }
        }
    }
}
