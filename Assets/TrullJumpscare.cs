using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrullJumpscare : MonoBehaviour
{
    public GameObject jumpscare;
    public GameControllerScript gc;
    public AudioClip clip2;
    public GameObject pickup;
    public bool playing;

    private void OnTriggerEnter(Collider other)
    {
        if (playing) return;
        if (other.CompareTag("Player"))
        {
            playing = true;
            StartCoroutine(jumpscareCoro());
        }
    }
    IEnumerator jumpscareCoro()
    {
        jumpscare.GetComponent<AudioSource>().ignoreListenerPause = true;
        AudioListener.pause = true;
        jumpscare.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        AudioListener.pause = false;
        jumpscare.SetActive(false);
        gc.audioDevice.PlayOneShot(clip2);
        pickup.SetActive(true);
        gameObject.SetActive(false);
    }
}
