using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class RaldiYCTPScript : MonoBehaviour
{
    public VideoPlayer video;
    public MathGameScript mgs;
    public RenderTexture texture;

    public bool random;
    public string path;

    private void Awake()
    {
        ClearOutRenderTexture(texture);

        if (random)
        {
            var videos = Directory.GetFiles($"{Application.streamingAssetsPath}/{path}");
            var video = videos[Random.Range(0, videos.Length - 1)];
            this.video.url = video;
        }
    }

    private void Start()
    {
        video.loopPointReached += penis;
        this.video.Play();
    }

    private void penis(VideoPlayer source)
    {
        mgs.Deactivate();
    }

    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }
}
