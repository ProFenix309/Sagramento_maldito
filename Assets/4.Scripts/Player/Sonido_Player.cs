using UnityEngine;

public class Sonido_Player : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip crouchSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayWalkSound()
    {
        audioSource.clip = walkSound;
        audioSource.Play();
    }
    public void PlayRunSound()
    {
        audioSource.clip = runSound;
        audioSource.Play();
    }
    public void PlayCrouchSound()
    {
        audioSource.clip = crouchSound;
        audioSource.Play();
    }
}
