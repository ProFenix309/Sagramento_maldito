using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(EnemyLock))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(HealthManager))]
public class PlayerManager : MonoBehaviour
{
    // Referencias a componentes
    private PlayerMovement movement;
    private PlayerInteraction interaction;
    private EnemyLock enemyLock;
    private PlayerAnimationController animController;
    private HealthManager health;

    private void Awake()
    {
        // Obtener todos los componentes
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        enemyLock = GetComponent<EnemyLock>();
        animController = GetComponent<PlayerAnimationController>();
        health = GetComponent<HealthManager>();
    }

    private void Start()
    {
        // Inicialización
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Verificar si el jugador está muerto
        if (health.vidaMaxima <= 0)
        {
            DisableAllControls();
            return;
        }
    }

    private void DisableAllControls()
    {
        movement.CanMove = false;
    }

    // Propiedades públicas para acceso externo
    public PlayerMovement Movement => movement;
    public PlayerInteraction Interaction => interaction;
    public EnemyLock Combat => enemyLock;
    public HealthManager Health => health;
}
