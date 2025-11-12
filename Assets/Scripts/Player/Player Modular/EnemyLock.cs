using UnityEngine;

public class EnemyLock : MonoBehaviour
{
    [Header("Look At Enemy Settings")]
    [SerializeField] private float lookSpeed = 5f;
    
    private PlayerMovement movement;
    private Camera_FPS_Controller cameraController;
    private Transform currentEnemy;
    private bool isBeingAttacked;
    
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        cameraController = Camera.main?.GetComponent<Camera_FPS_Controller>();
        
        if (cameraController == null)
        {
            Debug.LogWarning("No se encontró Camera_FPS_Controller en la cámara principal");
        }
    }
    
    private void Update()
    {
        if (isBeingAttacked && currentEnemy != null)
        {
            LookAtEnemy();
        }
    }
    
    private void LookAtEnemy()
    {
        if (currentEnemy == null) return;
        
        // Calcula la dirección hacia el enemigo (solo en el plano horizontal)
        Vector3 directionToEnemy = currentEnemy.position - transform.position;
        directionToEnemy.y = 0;
        directionToEnemy.Normalize();
        
        // Calcula el ángulo horizontal (Y) hacia el enemigo
        float targetYRotation = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;
        
        // Rota suavemente el jugador (solo eje Y)
        float currentYRotation = transform.eulerAngles.y;
        float newYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, Time.deltaTime * lookSpeed);
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
        
        // Calcula el ángulo vertical (X) para que la cámara también mire al enemigo
        if (cameraController != null)
        {
            Vector3 fullDirection = currentEnemy.position - Camera.main.transform.position;
            float targetXRotation = -Mathf.Atan2(fullDirection.y, 
                new Vector2(fullDirection.x, fullDirection.z).magnitude) * Mathf.Rad2Deg;
            
            cameraController.xRotation = Mathf.Lerp(cameraController.xRotation, 
                targetXRotation, Time.deltaTime * lookSpeed);
            cameraController.xRotation = Mathf.Clamp(cameraController.xRotation, -50, 50);
        }
    }
    
    public void StartLookingAtEnemy(Transform enemy)
    {
        currentEnemy = enemy;
        isBeingAttacked = true;
        
        // Desactiva el movimiento del jugador durante el ataque
        if (movement != null)
        {
            movement.CanMove = false;
            movement.StopMovement();
        }
        
        // Desactiva el control de la cámara FPS
        if (cameraController != null)
        {
            cameraController.canMove = false;
        }
        
        Debug.Log("Jugador está mirando al enemigo durante el ataque");
    }
    
    public void StopLookingAtEnemy()
    {
        isBeingAttacked = false;
        currentEnemy = null;
        
        // Restaura el movimiento
        if (movement != null)
        {
            movement.CanMove = true;
        }
        
        // Restaura el control de la cámara FPS
        if (cameraController != null)
        {
            cameraController.canMove = true;
        }
        
        Debug.Log("Jugador dejó de mirar al enemigo");
    }
}
