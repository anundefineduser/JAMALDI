using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TullWinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.pause = false;
        PlayerPrefs.SetInt("CanUnlockTrull", 1);
        PlayerPrefs.SetInt("TrullUnlocked", 0);
        StartCoroutine(frank());
    }

    IEnumerator frank()
    {
        yield return new WaitForSeconds(9.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
