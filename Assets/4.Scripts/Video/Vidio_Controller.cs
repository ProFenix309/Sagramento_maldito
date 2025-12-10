using UnityEngine;
using UnityEngine.Video;

public class Vidio_Controller : MonoBehaviour
{
    public GameObject panelVideo;

    public bool video;

    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void ActivedVideo()
    {
        panelVideo.SetActive(video);
    }

    public void DeactivatedVideo()
    {
        panelVideo.SetActive(!video);
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }
}
