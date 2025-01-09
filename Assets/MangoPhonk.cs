using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangoPhonk : MonoBehaviour
{
    public bool isTrigger;
    public GameObject link;
    private void Start()
    {
        if (!isTrigger)
        {
            PlayerPrefs.SetInt("TrullUnlocked", 1);
            StartCoroutine(MANGOMANGOMANGOMANGOMANGOMANGOMANGO());
        }
    }

    IEnumerator MANGOMANGOMANGOMANGOMANGOMANGOMANGO()
    {
        yield return new WaitForSecondsRealtime(2f);
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            link.SetActive(true);
        }
    }
}
