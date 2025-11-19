using System.Collections;
using UnityEngine;

public class WaterTeleportSystem : MonoBehaviour
{
    [Header("Configuración de Agua")]
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float checkRadius = 0.5f;

    [Header("Configuración de Animación del Ahogado")]
    [SerializeField] private Animator drownedAnimator; // Animator del enemigo ahogado
    [SerializeField] private string attackAnimationName = "Attack"; // Nombre de la animación de ataque
    [SerializeField] private float attackAnimationDuration = 1.5f; // Duración de la animación

    [Header("Configuración de Teleporte")]
    [SerializeField] private string spawnPointTag = "SpawnPoint"; // Tag para los puntos de spawn en las tablas
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0); // Offset para aparecer sobre la tabla

    [Header("Referencias")]
    [SerializeField] private GameObject drownedEnemy; // GameObject del enemigo ahogado (opcional, para mostrarlo)

    private bool isInWater = false;
    private bool isTeleporting = false;

    // Referencias a los componentes del jugador
    private Rigidbody rb;
    private PlayerController_Original playerController;
    private Camera_FPS_Controller cameraController;

    void Start()
    {
        // Obtener componentes del jugador
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController_Original>();

        // Buscar el controlador de cámara en "Main Camera"
        Transform mainCamera = transform.Find("Main Camera");
        if (mainCamera != null)
        {
            cameraController = mainCamera.GetComponent<Camera_FPS_Controller>();
        }

        // Buscar como respaldo si no se encuentra
        if (cameraController == null)
        {
            cameraController = GetComponentInChildren<Camera_FPS_Controller>();
        }

        if (cameraController == null)
        {
            Debug.LogWarning("No se encontró Camera_FPS_Controller. La cámara no se desactivará durante el teleporte.");
        }
    }

    void Update()
    {
        CheckWaterCollision();
    }

    void CheckWaterCollision()
    {
        // Si ya está teleportándose, no hacer nada
        if (isTeleporting) return;

        // Verificar si el jugador está tocando agua usando OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, waterLayer);

        if (hitColliders.Length > 0 && !isInWater)
        {
            isInWater = true;
            StartCoroutine(HandleWaterEntry());
        }
        else if (hitColliders.Length == 0)
        {
            isInWater = false;
        }
    }

    IEnumerator HandleWaterEntry()
    {
        isTeleporting = true;

        // Desactivar movimiento del jugador
        DisablePlayerMovement();

        // Mostrar el enemigo ahogado si está asignado
        if (drownedEnemy != null)
        {
            drownedEnemy.SetActive(true);
            // Posicionar al ahogado cerca del jugador
            drownedEnemy.transform.position = transform.position + transform.forward * 2f;
            drownedEnemy.transform.LookAt(transform);
        }

        // Ejecutar animación de ataque del ahogado
        if (drownedAnimator != null)
        {
            drownedAnimator.SetTrigger(attackAnimationName);
            // También puedes usar: drownedAnimator.Play(attackAnimationName);
        }

        // Esperar a que termine la animación
        yield return new WaitForSeconds(attackAnimationDuration);

        // Ocultar el enemigo ahogado
        if (drownedEnemy != null)
        {
            drownedEnemy.SetActive(false);
        }

        // Encontrar el SpawnPoint más cercano
        GameObject nearestSpawnPoint = FindNearestSpawnPoint();

        if (nearestSpawnPoint != null)
        {
            // Teleportar al jugador
            TeleportPlayer(nearestSpawnPoint.transform.position + spawnOffset);
        }
        else
        {
            Debug.LogWarning("¡No se encontró ningún SpawnPoint cercano! Asegúrate de tener objetos con el tag '" + spawnPointTag + "'");
        }

        // Reactivar movimiento del jugador
        yield return new WaitForSeconds(0.1f); // Pequeño delay para estabilizar
        EnablePlayerMovement();

        isTeleporting = false;
    }

    GameObject FindNearestSpawnPoint()
    {
        // Encontrar todos los objetos con el tag especificado
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);

        if (spawnPoints.Length == 0)
        {
            Debug.LogError($"No se encontraron objetos con el tag '{spawnPointTag}'. Por favor crea objetos vacíos en las tablas y asígnales este tag.");
            return null;
        }

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        // Buscar el más cercano
        foreach (GameObject spawnPoint in spawnPoints)
        {
            float distance = Vector3.Distance(transform.position, spawnPoint.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = spawnPoint;
            }
        }

        Debug.Log($"SpawnPoint más cercano encontrado a {minDistance:F2} metros de distancia");
        return nearest;
    }

    void TeleportPlayer(Vector3 targetPosition)
    {
        // Detener completamente el rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Teleportar
        transform.position = targetPosition;

        Debug.Log($"Jugador teleportado a: {targetPosition}");
    }

    void DisablePlayerMovement()
    {
        // Desactivar el movimiento del jugador
        if (playerController != null)
        {
            playerController.canMove = false;
        }

        // Desactivar el movimiento de la cámara
        if (cameraController != null)
        {
            cameraController.canMove = false;
        }

        // Detener velocidad del Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Movimiento del jugador desactivado");
    }

    void EnablePlayerMovement()
    {
        // Reactivar el movimiento del jugador
        if (playerController != null)
        {
            playerController.canMove = true;
        }

        // Reactivar el movimiento de la cámara
        if (cameraController != null)
        {
            cameraController.canMove = true;
        }

        Debug.Log("Movimiento del jugador reactivado");
    }

    // Método alternativo usando OnTriggerEnter si prefieres usar colliders trigger
    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto tiene el layer de agua
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && !isTeleporting)
        {
            isInWater = true;
            StartCoroutine(HandleWaterEntry());
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detectar cuando sale del agua
        if (((1 << other.gameObject.layer) & waterLayer) != 0)
        {
            isInWater = false;
        }
    }

    // Visualizar el radio de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

        // Mostrar los SpawnPoints en el editor
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);
        Gizmos.color = Color.green;
        foreach (GameObject sp in spawnPoints)
        {
            if (sp != null)
            {
                Gizmos.DrawWireCube(sp.transform.position, Vector3.one * 0.5f);
                Gizmos.DrawLine(transform.position, sp.transform.position);
            }
        }
    }
}