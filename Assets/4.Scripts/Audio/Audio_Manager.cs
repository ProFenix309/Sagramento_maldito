using System;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance { get; private set; }

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.nameSound == name);

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.nameSound == name);

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void ConfigurarSonido3D(Sound s, GameObject owner)
    {
        AudioSource source = owner.AddComponent<AudioSource>();

        s.audioSource = source;
        s.audioSource.clip = s.clip;
        s.audioSource.volume = s.volume;
        s.audioSource.loop = s.loop;

        s.audioSource.minDistance = s.minDistance;
        s.audioSource.maxDistance = s.maxDistance;
        s.audioSource.spatialBlend = s.spacialBlend;

        s.audioSource.playOnAwake = false;
    }

    private void CopySoundToSource(Sound s, AudioSource source)
{
    source.clip = s.clip;
    source.volume = s.volume;
    source.spatialBlend = s.spacialBlend;
    source.minDistance = s.minDistance;
    source.maxDistance = s.maxDistance;
    source.playOnAwake = false;

    if (s.usePlayOneShot)
    {
        source.loop = false;
    }
    else
    {
        source.loop = s.loop;
    }
}



}
