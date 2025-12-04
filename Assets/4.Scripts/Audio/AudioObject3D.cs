using System.Collections.Generic;
using UnityEngine;

public class AudioObject3D : MonoBehaviour
{
    [Header("Audio Reference")]
    [Tooltip("Nombre del sonido en la lista del Audio Manager")]
    public string soundName;
    
    [Header("Random Audio Settings")]
    public bool useRandomAudio = false;
    [Tooltip("Nombres de los sonidos en la lista del Audio Manager para reproducir aleatoriamente")]
    public List<string> randomSoundNames = new List<string>();
    
    [Header("Timing Settings")]
    public bool useWaitTime = false;
    [Tooltip("Tiempo de espera entre audios (en segundos)")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;
    
    [Header("Auto Play")]
    public bool playOnStart = false;
    
    private AudioSource audioSource;
    private List<string> availableSoundNames = new List<string>();
    private bool isPlaying = false;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private Sound currentSound;

    private void Start()
    {
        // Crear el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // Registramos este objeto en el Audio Manager
        Audio_Manager.instance.RegisterAudioObject(this);
        
        // Configurar el audio inicial
        if (useRandomAudio && randomSoundNames.Count > 0)
        {
            RefillAvailableSounds();
            if (playOnStart)
            {
                PlayNextRandom();
            }
        }
        else if (!string.IsNullOrEmpty(soundName))
        {
            LoadSoundSettings(soundName);
            if (playOnStart)
            {
                Play();
            }
        }
    }

    private void Update()
    {
        // Si estamos usando audio aleatorio
        if (useRandomAudio && audioSource != null)
        {
            // Verificar si el audio terminó de reproducirse
            if (isPlaying && !audioSource.isPlaying)
            {
                isPlaying = false;
                Debug.Log($"[{gameObject.name}] Audio finished playing");
            }

            // Si está esperando, cuenta el tiempo
            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    PlayNextRandom();
                }
            }
            // Si no está reproduciendo ni esperando, reproducir el siguiente
            else if (!isPlaying && !audioSource.isPlaying)
            {
                if (useWaitTime)
                {
                    StartWaiting();
                }
                else
                {
                    PlayNextRandom();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Audio_Manager.instance != null)
        {
            Audio_Manager.instance.UnregisterAudioObject(this);
        }
    }

    // Cargar configuración de un sonido desde el Audio Manager
    private void LoadSoundSettings(string name)
    {
        Sound s = System.Array.Find(Audio_Manager.instance.sfxSounds, x => x.nameSound == name);
        
        if (s == null)
        {
            Debug.LogWarning($"Sound '{name}' not found in Audio Manager's sfxSounds list!");
            return;
        }

        currentSound = s;
        ApplySoundToAudioSource(s);
    }

    // Aplicar la configuración de un Sound al AudioSource
    private void ApplySoundToAudioSource(Sound s)
    {
        if (audioSource == null) return;

        audioSource.clip = s.clip;
        audioSource.volume = s.volume;
        audioSource.spatialBlend = s.spacialBlend;
        audioSource.minDistance = s.minDistance;
        audioSource.maxDistance = s.maxDistance;
        audioSource.loop = s.loop;
    }

    public void Play()
    {
        if (audioSource == null) return;

        if (useRandomAudio)
        {
            PlayNextRandom();
        }
        else
        {
            if (!string.IsNullOrEmpty(soundName))
            {
                LoadSoundSettings(soundName);
                audioSource.Play();
                isPlaying = true;
            }
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isPlaying = false;
            isWaiting = false;
            waitTimer = 0f;
        }
    }

    // Iniciar el tiempo de espera
    private void StartWaiting()
    {
        isWaiting = true;
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
        Debug.Log($"[{gameObject.name}] Waiting {waitTimer:F2} seconds before next audio");
    }

    // Reproduce el siguiente audio aleatorio
    private void PlayNextRandom()
    {
        if (randomSoundNames.Count == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] No hay nombres de sonidos en randomSoundNames");
            return;
        }

        // Si no quedan sonidos disponibles, rellenar la lista
        if (availableSoundNames.Count == 0)
        {
            RefillAvailableSounds();
        }

        // Seleccionar un sonido aleatorio de los disponibles
        int randomIndex = Random.Range(0, availableSoundNames.Count);
        string selectedSoundName = availableSoundNames[randomIndex];
        
        // Remover el sonido seleccionado de los disponibles
        availableSoundNames.RemoveAt(randomIndex);

        // Cargar y reproducir el sonido
        LoadSoundSettings(selectedSoundName);
        
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            isPlaying = true;
            Debug.Log($"[{gameObject.name}] Playing: {selectedSoundName} - Remaining: {availableSoundNames.Count}");
        }
    }

    // Rellenar la lista de sonidos disponibles
    private void RefillAvailableSounds()
    {
        availableSoundNames.Clear();
        availableSoundNames.AddRange(randomSoundNames);
        Debug.Log($"[{gameObject.name}] Refilled sound pool with {availableSoundNames.Count} sounds");
    }

    // Actualizar configuración desde el Audio Manager
    public void UpdateFromManager()
    {
        if (useRandomAudio)
        {
            // En modo aleatorio, solo actualizamos si hay un sonido actual cargado
            if (currentSound != null)
            {
                LoadSoundSettings(currentSound.nameSound);
            }
        }
        else if (!string.IsNullOrEmpty(soundName))
        {
            LoadSoundSettings(soundName);
        }
    }

    // === MÉTODOS PÚBLICOS PARA CONTROL EXTERNO ===

    public void SetRandomMode(bool enable)
    {
        useRandomAudio = enable;
        if (enable && randomSoundNames.Count > 0)
        {
            RefillAvailableSounds();
        }
        else
        {
            isWaiting = false;
            waitTimer = 0f;
        }
    }


    public void SetWaitTime(bool enable, float min = 1f, float max = 5f)
    {
        useWaitTime = enable;
        minWaitTime = min;
        maxWaitTime = max;
    }

    public void SkipWait()
    {
        if (isWaiting)
        {
            isWaiting = false;
            waitTimer = 0f;
            PlayNextRandom();
        }
    }

    public void AddRandomSound(string name)
    {
        if (!string.IsNullOrEmpty(name) && !randomSoundNames.Contains(name))
        {
            randomSoundNames.Add(name);
            if (useRandomAudio)
            {
                RefillAvailableSounds();
            }
        }
    }

    public void RemoveRandomSound(string name)
    {
        if (randomSoundNames.Contains(name))
        {
            randomSoundNames.Remove(name);
            if (useRandomAudio)
            {
                RefillAvailableSounds();
            }
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}