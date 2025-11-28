using UnityEngine;

public class AudioItem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool playOnStart = true;
    public bool loop = true;

    [Header("3D Sound Settings")]
    public float minDistance = 1f;
    public float maxDistance = 10f;
    [Range(0f, 1f)]
    public float spatialBlend = 1f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

    private AudioSource audioSource;

    void Awake()
    {
        SetupAudioSource();
    }

    void Start()
    {
        if (playOnStart && audioClip != null)
        {
            Play();
        }
    }

    private void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = rolloffMode;
    }

    public void Play()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void Pause()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public void SetDistance(float min, float max)
    {
        minDistance = min;
        maxDistance = max;

        if (audioSource != null)
        {
            audioSource.minDistance = min;
            audioSource.maxDistance = max;
        }
    }

    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }

    void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.spatialBlend = spatialBlend;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.rolloffMode = rolloffMode;
        }
    }
}