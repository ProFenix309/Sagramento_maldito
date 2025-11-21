using UnityEngine;

public class PlatformWithValves : MonoBehaviour
{
    [Header("Plataformas a Descender")]
    [SerializeField] private Transform platform1;
    [SerializeField] private Transform platform2;

    [Header("Configuración de Descenso")]
    [SerializeField] private float descendDistance = 5f; // Distancia que descenderán las plataformas
    [SerializeField] private float descendSpeed = 2f;

    [Header("Sistema de Bloqueo")]
    [SerializeField] private bool locked = true;
    [SerializeField] private AudioSource unlockSound;

    private Vector3 platform1StartPos;
    private Vector3 platform2StartPos;
    private Vector3 platform1TargetPos;
    private Vector3 platform2TargetPos;

    private bool isDescending = false;
    private bool hasDescended = false;

    private void Start()
    {
        if (platform1 != null)
        {
            platform1StartPos = platform1.position;
            platform1TargetPos = platform1StartPos - new Vector3(0, descendDistance, 0);
        }

        if (platform2 != null)
        {
            platform2StartPos = platform2.position;
            platform2TargetPos = platform2StartPos - new Vector3(0, descendDistance, 0);
        }
    }

    private void Update()
    {
        if (isDescending)
        {
            bool platform1Complete = true;
            bool platform2Complete = true;

            if (platform1 != null)
            {
                platform1.position = Vector3.MoveTowards(
                    platform1.position,
                    platform1TargetPos,
                    descendSpeed * Time.deltaTime
                );
                platform1Complete = Vector3.Distance(platform1.position, platform1TargetPos) < 0.01f;
            }

            if (platform2 != null)
            {
                platform2.position = Vector3.MoveTowards(
                    platform2.position,
                    platform2TargetPos,
                    descendSpeed * Time.deltaTime
                );
                platform2Complete = Vector3.Distance(platform2.position, platform2TargetPos) < 0.01f;
            }

            if (platform1Complete && platform2Complete)
            {
                isDescending = false;
                hasDescended = true;
                Debug.Log("Plataformas han descendido completamente.");
            }
        }
    }

    public void Unlock()
    {
        if (hasDescended)
        {
            Debug.Log("Las plataformas ya han descendido.");
            return;
        }

        locked = false;
        isDescending = true;

        if (unlockSound != null)
        {
            unlockSound.Play();
        }

        Debug.Log("¡Plataformas desbloqueadas! Descendiendo...");
    }

    public void Lock()
    {
        locked = true;
        isDescending = false;
        hasDescended = false;

        // Resetear posiciones
        if (platform1 != null)
        {
            platform1.position = platform1StartPos;
        }

        if (platform2 != null)
        {
            platform2.position = platform2StartPos;
        }

        Debug.Log("Plataformas bloqueadas y reseteadas.");
    }

    public bool IsLocked()
    {
        return locked;
    }

    public bool HasDescended()
    {
        return hasDescended;
    }
}