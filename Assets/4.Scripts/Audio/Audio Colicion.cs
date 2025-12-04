using UnityEngine;

public class AudioColicion : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private float volume = 1f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        // Obtener o agregar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configurar AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Audio 3D
    }
    
    // Para colisiones físicas (con Rigidbody)
    void OnCollisionEnter(Collision collision)
    {
        PlayImpactSound();
    }
    
    // Para triggers (colliders marcados como "Is Trigger")
    void OnTriggerEnter(Collider other)
    {
        PlayImpactSound();
    }
    
    void PlayImpactSound()
    {
        if (impactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactSound, volume);
        }
        else
        {
            Debug.LogWarning("No se asignó un AudioClip al script RockCollision");
        }
    }
}