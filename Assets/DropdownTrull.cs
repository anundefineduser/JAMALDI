using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownTrull : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("TrullUnlocked") == 1)
        {
            List<string> list = new List<string>();
            list.Add("Trull");
            GetComponent<TMP_Dropdown>().ClearOptions();
            GetComponent<TMP_Dropdown>().AddOptions(list);
        }
    }
}
