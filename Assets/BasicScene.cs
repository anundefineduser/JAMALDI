using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicScene : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
}
