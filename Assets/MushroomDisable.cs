using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDisable : MonoBehaviour
{
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("CanUnlockTrull") != 1)
            gameObject.SetActive(false);
    }
}
