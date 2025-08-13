using Unity.VisualScripting;
using UnityEngine;

public enum AudioType
{
    CROUCH,
    GRABOBJECT,
    HURT,
    ALTERED,
    WOODFOOTSTEPS,
    STONESTEPS,

    

}

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] AudioList;
    private static AudioManager instance;
    private AudioSource audioSource;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
     audioSource = GetComponent<AudioSource>();   
    }

    public static void playAudio(AudioType audio, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.AudioList[(int)audio]);
    }
}
