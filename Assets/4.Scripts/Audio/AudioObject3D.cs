using UnityEngine;

public class AudioObject3D : MonoBehaviour
{
    public Sound sound; // El Sound que este objeto usa

    private void Start()
    {
        // Primero buscamos el Sound en la lista del Audio Manager
        Sound soundFromList = System.Array.Find(Audio_Manager.instance.sfxSounds, x => x.nameSound == sound.nameSound);

        if (soundFromList != null)
        {
            // Copiamos los valores de la lista a nuestro sound local
            sound.clip = soundFromList.clip;
            sound.volume = soundFromList.volume;
            sound.minDistance = soundFromList.minDistance;
            sound.maxDistance = soundFromList.maxDistance;
            sound.spacialBlend = soundFromList.spacialBlend;
            sound.loop = soundFromList.loop;
            sound.usePlayOneShot = soundFromList.usePlayOneShot;
        }
        else
        {
            Debug.LogWarning($"Sound '{sound.nameSound}' not found in Audio Manager's sfxSounds list!");
        }

        // Ahora configuramos el AudioSource con los valores actualizados
        Audio_Manager.instance.ConfigurarSonido3D(sound, gameObject);
        
        // Registramos este objeto en el Audio Manager
        Audio_Manager.instance.RegisterAudioObject(this);
    }

    private void OnDestroy()
    {
        // Desregistramos el objeto cuando se destruye
        if (Audio_Manager.instance != null)
        {
            Audio_Manager.instance.UnregisterAudioObject(this);
        }
    }

    public void Play()
    {
        if (sound.audioSource != null)
        {
            if (sound.usePlayOneShot)
            {
                sound.audioSource.PlayOneShot(sound.clip);
            }
            else
            {
                sound.audioSource.Play();
            }
        }
    }

    public void Stop()
    {
        if (sound.audioSource != null)
        {
            sound.audioSource.Stop();
        }
    }

    // Actualizar este AudioObject3D desde el Audio Manager
    public void UpdateFromManager()
    {
        Sound soundFromList = System.Array.Find(
            Audio_Manager.instance.sfxSounds, 
            x => x.nameSound == sound.nameSound
        );

        if (soundFromList != null && sound.audioSource != null)
        {
            // Actualizamos los valores del AudioSource directamente
            sound.audioSource.clip = soundFromList.clip;
            sound.audioSource.volume = soundFromList.volume;
            sound.audioSource.minDistance = soundFromList.minDistance;
            sound.audioSource.maxDistance = soundFromList.maxDistance;
            sound.audioSource.spatialBlend = soundFromList.spacialBlend;
            sound.audioSource.loop = soundFromList.loop;
            
            Debug.Log($"Updated AudioObject3D '{sound.nameSound}' from manager list");
        }
    }
}