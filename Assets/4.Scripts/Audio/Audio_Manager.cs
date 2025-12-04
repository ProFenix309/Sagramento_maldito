using System;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance { get; private set; }

    [Header("Sound Libraries")]
    public Sound[] musicSounds, sfxSounds;
    
    [Header("Audio Sources")]
    public AudioSource musicSource, sfxSource;

    [Header("Background Music")]
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
        if (!string.IsNullOrEmpty(backgroundMusic))
        {
            PlayMusic(backgroundMusic);
        }
    }

    // === MÉTODOS PARA SFX 2D ===
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.nameSound == name);

        if (s == null)
        {
            Debug.LogWarning($"SFX Sound '{name}' not found");
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }

    // === MÉTODOS PARA MÚSICA ===
    
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.nameSound == name);

        if (s == null)
        {
            Debug.LogWarning($"Music Sound '{name}' not found");
            return;
        }

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void UnpauseMusic()
    {
        musicSource.UnPause();
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
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }

    // === MÉTODOS PARA AUDIO 3D (AudioObject3D) ===

    public void RegisterAudioObject(AudioObject3D audioObject)
    {
        if (!registeredAudioObjects.Contains(audioObject))
        {
            registeredAudioObjects.Add(audioObject);
            Debug.Log($"Registered AudioObject3D: {audioObject.gameObject.name}");
        }
    }

    public void UnregisterAudioObject(AudioObject3D audioObject)
    {
        if (registeredAudioObjects.Contains(audioObject))
        {
            registeredAudioObjects.Remove(audioObject);
            Debug.Log($"Unregistered AudioObject3D: {audioObject.gameObject.name}");
        }
    }

    // Actualizar todos los AudioObject3D con los valores actuales de las listas
    public void UpdateAllAudioObjects3D()
    {
        int updated = 0;
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj != null)
            {
                audioObj.UpdateFromManager();
                updated++;
            }
        }
        Debug.Log($"Updated {updated} AudioObject3D instances from manager lists");
    }

    // Actualizar AudioObject3D que usan un sonido específico
    public void UpdateAudioObject3DBySound(string soundName)
    {
        int updated = 0;
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj != null && audioObj.soundName == soundName)
            {
                audioObj.UpdateFromManager();
                updated++;
            }
        }
        Debug.Log($"Updated {updated} AudioObject3D(s) using sound '{soundName}'");
    }

    // Cambiar volumen de todos los AudioObject3D
    public void SetAllAudioObjects3DVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj != null)
            {
                audioObj.SetVolume(volume);
            }
        }
    }

    // Detener todos los AudioObject3D
    public void StopAllAudioObjects3D()
    {
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj != null)
            {
                audioObj.Stop();
            }
        }
    }

    // Reproducir todos los AudioObject3D
    public void PlayAllAudioObjects3D()
    {
        foreach (AudioObject3D audioObj in registeredAudioObjects)
        {
            if (audioObj != null)
            {
                audioObj.Play();
            }
        }
    }

    // Obtener lista de AudioObject3D registrados
    public List<AudioObject3D> GetRegisteredAudioObjects()
    {
        return new List<AudioObject3D>(registeredAudioObjects);
    }

    // === MÉTODOS DE UTILIDAD ===

    // Verificar si un sonido existe en la lista
    public bool SoundExists(string soundName, bool checkMusic = false)
    {
        Sound[] soundArray = checkMusic ? musicSounds : sfxSounds;
        return Array.Exists(soundArray, x => x.nameSound == soundName);
    }

    // Obtener un sonido de la lista
    public Sound GetSound(string soundName, bool fromMusic = false)
    {
        Sound[] soundArray = fromMusic ? musicSounds : sfxSounds;
        return Array.Find(soundArray, x => x.nameSound == soundName);
    }

    // Listar todos los nombres de sonidos
    public string[] GetAllSoundNames(bool fromMusic = false)
    {
        Sound[] soundArray = fromMusic ? musicSounds : sfxSounds;
        string[] names = new string[soundArray.Length];
        for (int i = 0; i < soundArray.Length; i++)
        {
            names[i] = soundArray[i].nameSound;
        }
        return names;
    }

    // === VALIDACIÓN EN EDITOR ===
    
    private void OnValidate()
    {
        // Verificar duplicados en musicSounds
        CheckDuplicateSoundNames(musicSounds, "Music Sounds");
        
        // Verificar duplicados en sfxSounds
        CheckDuplicateSoundNames(sfxSounds, "SFX Sounds");
    }

    private void CheckDuplicateSoundNames(Sound[] sounds, string listName)
    {
        if (sounds == null) return;

        HashSet<string> names = new HashSet<string>();
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i] != null && !string.IsNullOrEmpty(sounds[i].nameSound))
            {
                if (!names.Add(sounds[i].nameSound))
                {
                    Debug.LogWarning($"Duplicate sound name '{sounds[i].nameSound}' found in {listName}!");
                }
            }
        }
    }
}