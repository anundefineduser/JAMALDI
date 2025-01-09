using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DeathVideoScript : MonoBehaviour
{
    public VideoPlayer player;
    public AudioSource source;
    private void Awake()
    {
        ClearOutRenderTexture(player.targetTexture);
        source.ignoreListenerPause = true;
    }


    void Start()
    {
        player.loopPointReached += Finish;
        player.Play();
    }

    void Finish(VideoPlayer source)
    {
        try
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=V-RDWBhe9PY");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning(e.Message);
        }
        Application.Quit();
    }

    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}
