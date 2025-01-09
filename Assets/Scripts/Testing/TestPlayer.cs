using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class TestPlayer : MonoBehaviour
{
    public string path;
    public VideoPlayer video;
    public int current;

    string[] videos;

    private void Awake()
    {
        videos = Directory.GetFiles($"{Application.streamingAssetsPath}/{path}");
    }

    public void ChangeVideo(int add)
    {
        this.video.Stop();
        ClearOutRenderTexture(this.video.targetTexture);
        current += add;
        Debug.Log($"{current + 1} out of {videos.Length}.");
        if (current > videos.Length-1)
            current -= videos.Length;
        else if (current < 0)
            current += videos.Length-1;
        Debug.Log($"{current+1} (fixed) out of {videos.Length}.");

        var video = videos[current];
        this.video.url = video;
        this.video.Play();
    }

    // for external use
    public void ChangeFromField(TMP_InputField field)
    {
        ChangeVideo(int.Parse(field.text));
    }

    private void Start()
    {
        video.loopPointReached += penis;
        ChangeVideo(0);
    }

    private void penis(VideoPlayer source)
    {
        ChangeVideo(1);
    }

    void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}
