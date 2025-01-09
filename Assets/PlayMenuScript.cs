using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuScript : MonoBehaviour
{
    public List<PlayButton> buttons;
    public TMP_Dropdown dropdown;
    public GameObject loading;
    public GameObject hud;
    public void PlayGame()
    {
        switch (ButtonFromName(dropdown.options[dropdown.value].text).mode)
        {
            case StartButton.Mode.Story:
                PlayerPrefs.SetString("CurrentMode", "story");
                loading.SetActive(true);
                SceneManager.LoadSceneAsync("School");
                hud.SetActive(false);
                return;
            case StartButton.Mode.Endless:
                PlayerPrefs.SetString("CurrentMode", "endless");
                loading.SetActive(true);
                SceneManager.LoadSceneAsync("School");
                hud.SetActive(false);
                return;
            case StartButton.Mode.Trull:
                PlayerPrefs.SetString("CurrentMode", "trull");
                loading.SetActive(true);
                SceneManager.LoadSceneAsync("School");
                hud.SetActive(false);
                return;
        }
    }
    public PlayButton ButtonFromName(string name)
    {
        foreach (var button in buttons)
        {
            if (button.name == name)
                return button;
        }
        return null;
    }
}

[System.Serializable]
public class PlayButton
{
    public string name;
    public StartButton.Mode mode;
}