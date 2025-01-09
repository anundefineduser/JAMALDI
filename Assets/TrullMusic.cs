using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrullMusic : MonoBehaviour
{
    public AudioSource source;
    public AudioClip mango;
    public List<AudioClip> music;
    private void Start()
    {
        if (PlayerPrefs.GetInt("TrullUnlocked") == 1)
        {
            source.Stop();
            source.clip = mango;
            source.Play();
        }
        else
        {

            source.Stop();
            source.clip = music[Random.Range(0,music.Count)];
            source.Play();
        }
    }
}
