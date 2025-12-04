using System;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance { get; private set; }

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string backgroundMusic;

    // Lista para trackear todos los AudioObject3D registrados
    private List<AudioObject3D> registeredAudioObjects = new List<AudioObject3D>();

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

    // Registrar un AudioObject3D en el manager
    public void RegisterAudioObject(AudioObject3D audioObject)
    {
        if (!registeredAudioObjects.Contains(audioObject))
        {
            registeredAudioObjects.Add(audioObject);
        }
    }

    // Desregistrar un AudioObject3D
    public void UnregisterAudioObject(AudioObject3D audioObject)
    {
        if (registeredAudioObjects.Contains(audioObject))
        {
            registeredAudioObjects.Remove(audioObject);
        }
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

    // Actualizar un AudioObject3D específico por nombre
    public void UpdateAudioObject3D(string soundName)
    {
        Sound s = Array.Find(sfxSounds, x => x.nameSound == soundName);
        
        if (s == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found in sfxSounds array");
            return;
        }

        int updated = 0;
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj.sound.nameSound == soundName && audioObj.sound.audioSource != null)
            {
                CopySoundToSource(s, audioObj.sound.audioSource);
                updated++;
            }
        }
        
        if (updated > 0)
        {
            Debug.Log($"Updated {updated} AudioObject3D(s) with sound '{soundName}'");
        }
    }

    // Actualizar todos los AudioObject3D que usan sounds de la lista
    public void UpdateAllAudioObjects3D()
    {
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            Sound s = Array.Find(sfxSounds, x => x.nameSound == audioObj.sound.nameSound);
            
            if (s != null)
            {
                CopySoundToSource(s, audioObj.sound.audioSource);
            }
        }
        Debug.Log($"Updated {registeredAudioObjects.Count} AudioObject3D instances");
    }

    // Actualizar volumen de todos los AudioObject3D
    public void UpdateAllAudioObjects3DVolume(float volume)
    {
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj.sound.audioSource != null)
            {
                audioObj.sound.audioSource.volume = volume;
            }
        }
    }

    // Actualizar volumen de un AudioObject3D específico por nombre
    public void UpdateAudioObject3DVolume(string soundName, float volume)
    {
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj.sound.nameSound == soundName && audioObj.sound.audioSource != null)
            {
                audioObj.sound.audioSource.volume = volume;
            }
        }
    }

    // Obtener todos los AudioObject3D registrados
    public List<AudioObject3D> GetRegisteredAudioObjects()
    {
        return new List<AudioObject3D>(registeredAudioObjects);
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