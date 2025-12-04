using UnityEngine;

public class AudioObject3D : MonoBehaviour
{
    public Sound sound; // El Sound que este objeto usa

    private void Start()
    {
        // Le pedimos al Audio Manager que configure este sonido 3D
        Audio_Manager.instance.ConfigurarSonido3D(sound, gameObject);
    }

    public void Play()
    {
        sound.audioSource.Play();
    }

    public void Stop()
    {
        sound.audioSource.Stop();
    }
}
