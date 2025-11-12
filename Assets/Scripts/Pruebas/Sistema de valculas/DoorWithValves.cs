using UnityEngine;

public class DoorWithValves : MonoBehaviour, Interactable
{
    [Header("Configuración de Puerta")]
    [SerializeField] private Transform doorTransform; // El objeto visual que se rotará
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closePosition;
    [SerializeField] private float speed = 90f;

    [Header("Sistema de Bloqueo")]
    [SerializeField] private bool locked = true;
    [SerializeField] private AudioSource lockedSound;

    private Transform targetPosition;
    private bool isOpen = false;

    private void Start()
    {
        openPosition.SetParent(null);
        closePosition.SetParent(null);
        targetPosition = closePosition;
    }

    public void Interact()
    {
        if (locked)
        {
            Debug.Log("La puerta está bloqueada. Activa todas las válvulas primero.");

            if (lockedSound != null)
            {
                lockedSound.Play();
            }

            return;
        }

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        targetPosition = isOpen ? openPosition : closePosition;
        Debug.Log($"Puerta {(isOpen ? "abriendo" : "cerrando")}...");
    }

    public void Unlock()
    {
        locked = false;
        Debug.Log("Puerta desbloqueada!");
    }

    public void Lock()
    {
        locked = true;
        isOpen = false;
        targetPosition = closePosition;
        Debug.Log("La Puerta esta abierta");
    }

    private void Update()
    {
        if (transform.rotation != targetPosition.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPosition.rotation, speed * Time.deltaTime);
        }
    }

    public bool IsLocked()
    {
        return locked;
    }
}