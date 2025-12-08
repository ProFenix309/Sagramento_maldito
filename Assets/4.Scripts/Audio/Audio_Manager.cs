using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [Header("Music Sounds")]
    public Sound[] musicSounds;
    
    [Header("SFX Sounds")]
    public Sound[] sfxSounds;
    
    [Header("Audio Sources Pool")]
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    
    [Header("Auto Play Music")]
    [Tooltip("Reproducir música automáticamente al iniciar")]
    public bool autoPlayMusic = true;
    [Tooltip("Nombre de la música que se reproducirá al iniciar")]
    public string initialMusicName = "BackgroundMusic";
    
    // Diccionarios para acceso rápido
    private Dictionary<string, Sound> musicDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> sfxDictionary = new Dictionary<string, Sound>();
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeAudio()
    {
        // Inicializar música
        foreach (Sound s in musicSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            
            musicDictionary[s.name] = s;
        }
        
        // Inicializar SFX
        foreach (Sound s in sfxSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            
            // Configurar sonido 3D
            if (s.is3D)
            {
                s.source.spatialBlend = 1f; // 1 = completamente 3D, 0 = completamente 2D
                s.source.minDistance = s.minDistance;
                s.source.maxDistance = s.maxDistance;
                s.source.rolloffMode = AudioRolloffMode.Linear;
            }
            else
            {
                s.source.spatialBlend = 0f; // Sonido 2D
            }
            
            sfxDictionary[s.name] = s;
        }
        
        // Crear pool de AudioSources para sonidos 3D dinámicos
        CreateAudioSourcePool();
        
        // Reproducir música inicial automáticamente
        if (autoPlayMusic && !string.IsNullOrEmpty(initialMusicName))
        {
            PlayMusic(initialMusicName);
        }
    }
    
    void CreateAudioSourcePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            audioSourcePool.Add(source);
        }
    }
    
    // MÉTODOS PARA REPRODUCIR MÚSICA
    
    public void PlayMusic(string name)
    {
        if (musicDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Music: " + name + " no encontrada!");
        }
    }
    
    public void StopMusic(string name)
    {
        if (musicDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Stop();
        }
    }
    
    public void PauseMusic(string name)
    {
        if (musicDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Pause();
        }
    }
    
    public void StopAllMusic()
    {
        foreach (var music in musicDictionary.Values)
        {
            music.source.Stop();
        }
    }
    
    // MÉTODOS PARA REPRODUCIR SFX
    
    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("SFX: " + name + " no encontrado!");
        }
    }
    
    public void PlaySFXOneShot(string name)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            s.source.PlayOneShot(s.clip, s.volume);
        }
    }
    
    // MÉTODOS PARA REPRODUCIR SONIDOS ALEATORIOS (Variedad)
    
    public void PlayRandomSFX(params string[] soundNames)
    {
        if (soundNames.Length == 0) return;
        
        string randomName = soundNames[Random.Range(0, soundNames.Length)];
        PlaySFX(randomName);
    }
    
    public void PlayRandomSFX3D(Vector3 position, params string[] soundNames)
    {
        if (soundNames.Length == 0) return;
        
        string randomName = soundNames[Random.Range(0, soundNames.Length)];
        PlaySFX3D(randomName, position);
    }
    
    public void PlayRandomSFX3DAtGameObject(GameObject target, params string[] soundNames)
    {
        if (soundNames.Length == 0) return;
        
        string randomName = soundNames[Random.Range(0, soundNames.Length)];
        PlaySFX3DAtGameObject(randomName, target);
    }
    
    // Variación con pitch aleatorio para más variedad
    public void PlaySFXWithRandomPitch(string name, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            float originalPitch = s.source.pitch;
            s.source.pitch = Random.Range(minPitch, maxPitch);
            s.source.Play();
            s.source.pitch = originalPitch; // Restaurar pitch original
        }
    }
    
    public void PlaySFX3DWithRandomPitch(string name, Vector3 position, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            AudioSource source = GetAvailableAudioSource();
            if (source != null)
            {
                source.transform.position = position;
                source.clip = s.clip;
                source.volume = s.volume;
                source.pitch = Random.Range(minPitch, maxPitch);
                source.spatialBlend = 1f;
                source.minDistance = s.minDistance;
                source.maxDistance = s.maxDistance;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.Play();
            }
        }
    }
    
    // MÉTODOS PARA SONIDO 3D EN POSICIÓN ESPECÍFICA
    
    public void PlaySFX3D(string name, Vector3 position)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            AudioSource source = GetAvailableAudioSource();
            if (source != null)
            {
                source.transform.position = position;
                source.clip = s.clip;
                source.volume = s.volume;
                source.pitch = s.pitch;
                source.spatialBlend = 1f;
                source.minDistance = s.minDistance;
                source.maxDistance = s.maxDistance;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.Play();
            }
        }
    }
    
    public void PlaySFX3DAtGameObject(string name, GameObject target)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            AudioSource source = GetAvailableAudioSource();
            if (source != null)
            {
                source.transform.position = target.transform.position;
                source.clip = s.clip;
                source.volume = s.volume;
                source.pitch = s.pitch;
                source.spatialBlend = 1f;
                source.minDistance = s.minDistance;
                source.maxDistance = s.maxDistance;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.Play();
            }
        }
    }
    
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        
        // Si no hay disponible, crear uno nuevo
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSourcePool.Add(newSource);
        return newSource;
    }
    
    // CONTROL DE VOLUMEN CON AUDIO MIXER
    
    public void SetMasterVolume(float volume)
    {
        // Convertir de escala linear (0-1) a decibeles (-80 a 0)
        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
    
    public void SetMusicVolume(float volume)
    {
        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("MusicVolume", dB);
    }
    
    public void SetSFXVolume(float volume)
    {
        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("SFXVolume", dB);
    }
    
    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float volume);
        return Mathf.Pow(10, volume / 20);
    }
    
    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float volume);
        return Mathf.Pow(10, volume / 20);
    }
    
    public float GetSFXVolume()
    {
        audioMixer.GetFloat("SFXVolume", out float volume);
        return Mathf.Pow(10, volume / 20);
    }
}