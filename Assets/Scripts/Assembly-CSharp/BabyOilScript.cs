using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabyOilScript : MonoBehaviour
{
    public Animator oilAnimator;
    public AudioSource source;
    public AudioClip clip;
    public float oilTimer;

    private void Update()
    {
        if (oilTimer > 0f)
        {
            oilTimer -= Time.deltaTime;
            if (oilTimer <= 0f)
                oilAnimator.SetTrigger("Disappear");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.PlayOneShot(clip);
            oilTimer = 10f;
            oilAnimator.gameObject.SetActive(true);
            oilAnimator.SetTrigger("Appear");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oilTimer = 10f;
        }
    }
}
