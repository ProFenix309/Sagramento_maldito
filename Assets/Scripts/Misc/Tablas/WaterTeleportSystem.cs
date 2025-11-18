using System.Collections;
using UnityEngine;

public class WaterTeleportSystem : MonoBehaviour
{
    [Header("Configuración de Agua")]
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float checkRadius = 0.5f;

    [Header("Configuración del Enemigo Ahogado")]
    [SerializeField] private GameObject drownedEnemy; // GameObject del enemigo ahogado
    [SerializeField] private Animator drownedAnimator; // Animator del enemigo ahogado
    [SerializeField] private string attackAnimationName = "Attack"; // Nombre de la animación de ataque
    [SerializeField] private float attackAnimationDuration = 1.5f; // Duración de la animación
    [SerializeField] private Vector3 enemySpawnOffset = new Vector3(0, 0, 2); // Offset relativo al jugador
    [SerializeField] private bool resetEnemyAfterAttack = true; // Regresar enemigo a StartingPoint

    [Header("Configuración de Teleporte")]
    [SerializeField] private string spawnPointTag = "SpawnPoint"; // Tag para los puntos de spawn en las tablas
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0); // Offset para aparecer sobre la tabla

    [Header("Configuración del Ataque")]
    [SerializeField] private float damageAmount = 20f; // Daño que hace el ahogado
    [SerializeField] private bool shouldDamagePlayer = true; // Si debe hacer daño al jugador

    private bool isInWater = false;
    private bool isTeleporting = false;

    // Referencias a los componentes del jugador
    private Rigidbody rb;
    private PlayerController_Original playerController;
    private Camera_FPS_Controller cameraController;
    private HealthManager playerHealth;

    // Referencias del enemigo
    private EnemyAI drownedAI;
    private Transform enemyStartingPoint;
    private Vector3 originalEnemyPosition;

    void Start()
    {
        // Obtener componentes del jugador
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController_Original>();
        playerHealth = GetComponent<HealthManager>();

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

        // Configurar el enemigo ahogado
        if (drownedEnemy != null)
        {
            drownedAI = drownedEnemy.GetComponent<EnemyAI>();

            // Guardar la posición original del enemigo
            originalEnemyPosition = drownedEnemy.transform.position;

            // Buscar el StartingPoint del enemigo
            GameObject startingPointObj = GameObject.Find("StartingPoint");
            if (startingPointObj != null)
            {
                enemyStartingPoint = startingPointObj.transform;
            }

            // Obtener el animator si no está asignado
            if (drownedAnimator == null)
            {
                drownedAnimator = drownedEnemy.GetComponent<Animator>();
            }

            // NO desactivar el enemigo, debe estar activo desde el inicio
            // El enemigo patrullará normalmente hasta que el jugador entre al agua
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

        // Activar y posicionar el enemigo ahogado
        if (drownedEnemy != null)
        {
            // Si el enemigo estaba desactivado, activarlo
            if (!drownedEnemy.activeSelf)
            {
                drownedEnemy.SetActive(true);
            }

            // Calcular posición frente al jugador
            Vector3 spawnPosition = transform.position + transform.forward * enemySpawnOffset.z +
                                   transform.right * enemySpawnOffset.x +
                                   transform.up * enemySpawnOffset.y;

            drownedEnemy.transform.position = spawnPosition;
            drownedEnemy.transform.LookAt(transform);

            // Desactivar el AI del enemigo durante la cinemática
            if (drownedAI != null)
            {
                drownedAI.enabled = false;
            }
        }

        // Ejecutar animación de ataque del ahogado
        if (drownedAnimator != null)
        {
            drownedAnimator.SetTrigger(attackAnimationName);
        }

        // Esperar un momento antes de hacer daño (mitad de la animación)
        yield return new WaitForSeconds(attackAnimationDuration * 0.5f);

        // Hacer daño al jugador
        if (shouldDamagePlayer && playerHealth != null)
        {
            playerHealth.RecibirDaño(damageAmount);
            Debug.Log($"Ahogado atacó al jugador causando {damageAmount} de daño");
        }

        // Esperar a que termine el resto de la animación
        yield return new WaitForSeconds(attackAnimationDuration * 0.5f);

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

        // Resetear el enemigo ahogado
        ResetDrownedEnemy();

        // Reactivar movimiento del jugador
        yield return new WaitForSeconds(0.1f); // Pequeño delay para estabilizar
        EnablePlayerMovement();

        isTeleporting = false;
    }

    void ResetDrownedEnemy()
    {
        if (drownedEnemy == null) return;

        // Reactivar el AI si estaba desactivado (ANTES de mover)
        if (drownedAI != null)
        {
            drownedAI.enabled = true;

            // Resetear el estado de ataque del AI
            if (drownedAI.alreadyAtacked)
            {
                drownedAI.alreadyAtacked = false;
            }
        }

        if (resetEnemyAfterAttack)
        {
            // Regresar el enemigo a su StartingPoint
            if (enemyStartingPoint != null)
            {
                drownedEnemy.transform.position = enemyStartingPoint.position;
                Debug.Log("Enemigo regresado a StartingPoint y reactivado");
            }
            else
            {
                // Si no hay StartingPoint, usar la posición original
                drownedEnemy.transform.position = originalEnemyPosition;
                Debug.Log("Enemigo regresado a posición original y reactivado");
            }
        }

        // MANTENER el enemigo ACTIVADO para que siga patrullando
        // NO desactivar: drownedEnemy.SetActive(false);
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

        // Mostrar donde aparecerá el enemigo
        if (Application.isPlaying)
        {
            Vector3 enemySpawnPos = transform.position + transform.forward * enemySpawnOffset.z +
                                   transform.right * enemySpawnOffset.x +
                                   transform.up * enemySpawnOffset.y;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(enemySpawnPos, Vector3.one * 0.5f);
            Gizmos.DrawLine(transform.position, enemySpawnPos);
        }

        // Mostrar los SpawnPoints en el editor
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);
        Gizmos.color = Color.green;
        foreach (GameObject sp in spawnPoints)
        {
            if (sp != null)
            {
                Gizmos.DrawWireCube(sp.transform.position, Vector3.one * 0.5f);
            }
        }
    }
}