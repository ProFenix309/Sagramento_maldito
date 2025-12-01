using UnityEngine;

public class Sonido_Player : MonoBehaviour
{
    public AudioSource pasos;

    [SerializeField] private PlayerController_Original playerController;
    [SerializeField] private Rigidbody rb; // Referencia al Rigidbody del jugador

    [Header("Configuración de Sonido")]
    [SerializeField] private float minVelocityToPlaySound = 0.1f; // Velocidad mínima para reproducir pasos
    [SerializeField] private float normalPitch = 0.5f;
    [SerializeField] private float runPitch = 1f;

    private bool wasGrounded;

    private void Update()
    {
        ManageFootsteps();
    }

    private void ManageFootsteps()
    {
        // Verificar si estamos en el suelo
        bool isGrounded = playerController != null && playerController.isGround;

        // Verificar si nos estamos moviendo (velocidad horizontal)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isMoving = horizontalVelocity.magnitude > minVelocityToPlaySound;

        // Ajustar pitch según si está corriendo
        if (playerController.run)
        {
            pasos.pitch = runPitch;
        }
        else
        {
            pasos.pitch = normalPitch;
        }

        // Lógica del sonido de pasos
        if (isGrounded && isMoving)
        {
            // Estamos en el suelo y moviéndonos -> reproducir sonido
            if (!pasos.isPlaying)
            {
                pasos.Play();
            }
        }
        else
        {
            // No estamos en el suelo O no nos movemos -> pausar sonido
            if (pasos.isPlaying)
            {
                pasos.Pause();
            }
        }

        wasGrounded = isGrounded;
    }
}