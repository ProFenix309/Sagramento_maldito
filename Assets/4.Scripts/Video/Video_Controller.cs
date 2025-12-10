using UnityEngine;
using UnityEngine.Video;

public class Video_Controller : MonoBehaviour
{
    public GameObject panelVideo;

    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.GetComponent<VideoPlayer>();

        if (panelVideo != null)
        {
            panelVideo.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.isLooping = false;
        }
    }


    public void ActivedVideo()
    {
        panelVideo.SetActive(true);
        Debug.LogError("Panel activo");
    }

    public void DeactivatedVideo()
    {
        panelVideo.SetActive(false);
        Debug.LogError("Panel inactivo");
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        Debug.LogError("video activo");
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        Debug.LogError("video inactivo");
    }
}
