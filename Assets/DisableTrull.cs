using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrull : MonoBehaviour
{
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("TrullUnlocked") == 1)
            gameObject.SetActive(false);
    }
}
