using UnityEngine.AI;
using UnityEngine;

public class Errante_AI : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    
    [SerializeField] string nameTarget = "Player";
    
    [Space]
    [Header("Layers")]
    public LayerMask whatIsGround, whatIsPlayer;
    
    Transform StartingPoint;
    float Velocity;
    
    [Space]
    [Header("Patroling")]
    public float initialRandomTime = 2f;
    float randomTime;
    Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange = 10f;
    
    [Space]
    [Header("Chasing")]
    public float velocity;
    public float ChaseVelocity;
    
    [Space]
    [Header("Atacking")]
    public float timeBetweenAtacks = 2f;
    public bool alreadyAtacked;
    public float AttackingTime = 1f;
    [SerializeField] float damage = 10f;
    private bool isAttacking = false;
    
    [Header("Ranges")]
    public float sightRange = 15f;
    public float attackRange = 3f;
    public float lightDetectionRange = 20f;
    
    [Header("States")]
    public bool playerInSightRange, playerInAttackRange;
    
    [Header("Animator")]
    public Animator animator;
    
    //Sistema de Audio
    [Space]
    [Header("Audio Settings")]
    [Tooltip("Intervalo mínimo entre suspiros cuando patrulla")]
    public float sighIntervalMin = 4f;
    [Tooltip("Intervalo máximo entre suspiros cuando patrulla")]
    public float sighIntervalMax = 8f;
    
    private float nextSighTime = 0f;
    private bool isChasing = false;
    private bool wasChasing = false;
    
    private Light nearestLight;
    private bool lightInRange;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        GameObject startObj = GameObject.Find("StartingPoint");
        if (startObj != null)
            StartingPoint = startObj.transform;
        else
            StartingPoint = transform;
        
        if (!string.IsNullOrEmpty(nameTarget))
        {
            GameObject playerObj = GameObject.Find(nameTarget);
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        if (agent != null)
            velocity = agent.speed;
    }
    
    private void Start()
    {
        randomTime = initialRandomTime;
        Velocity = velocity;
        
        // Inicializar primer suspiro
        nextSighTime = Time.time + Random.Range(sighIntervalMin, sighIntervalMax);
    }
    
    private void Update()
    {
        randomTime -= Time.deltaTime;
        
        bool playerHasLightOn = false;
        if (player != null)
        {
            Candle_Controller playerCandle = player.GetComponentInChildren<Candle_Controller>();
            if (playerCandle != null)
            {
                playerHasLightOn = playerCandle.IsLightOn();
            }
        }
        
        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : 999f;
        
        bool sphereDetection = player != null && Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        
        playerInSightRange = player != null && playerHasLightOn && distanceToPlayer <= sightRange;
        playerInAttackRange = player != null && playerHasLightOn && distanceToPlayer <= attackRange;
        
        FindNearestLight();
        
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (lightInRange && nearestLight != null)
        {
            ChaseLight();
        }
        else
        {
            Patroling();
        }
        
    }
    
    private void FindNearestLight()
    {
        LightSwitch[] allLights = FindObjectsByType<LightSwitch>(FindObjectsSortMode.None);
        
        nearestLight = null;
        float nearestDistance = lightDetectionRange;
        lightInRange = false;
        
        foreach (LightSwitch lightSwitch in allLights)
        {
            if (lightSwitch.IsLightOn())
            {
                float distance = Vector3.Distance(transform.position, lightSwitch.transform.position);
                
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestLight = lightSwitch.GetComponent<Light>();
                    lightInRange = true;
                }
            }
        }
    }
    
    private void Patroling()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        
        agent.speed = Velocity;
        
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        isChasing = false;
        
        if (wasChasing && !isChasing)
        {
            wasChasing = false;
        }
        
        // Reproducir suspiros aleatorios mientras patrulla
        if (Time.time >= nextSighTime)
        {
            PlayPatrolSigh();
            nextSighTime = Time.time + Random.Range(sighIntervalMin, sighIntervalMax);
        }
        
        if (!walkPointSet)
            SearchWalkPoint();
        
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if (distanceToWalkPoint.magnitude < 2f || !agent.hasPath || agent.velocity.sqrMagnitude < 0.1f)
        {
            walkPointSet = false;
        }
    }
    
    private void SearchWalkPoint()
    {
        int attempts = 0;
        while (attempts < 10 && !walkPointSet)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            
            if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, walkPointRange * 2f, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(walkPoint, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    walkPointSet = true;
                    return;
                }
            }
            
            attempts++;
        }
        
        if (!walkPointSet)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit nearHit, 5f, NavMesh.AllAreas))
            {
                walkPoint = nearHit.position;
                walkPointSet = true;
            }
        }
    }
    
    private void ChasePlayer()
    {
        if (agent == null || player == null || !agent.isOnNavMesh) return;
        
        agent.speed = ChaseVelocity;
        
        if (animator != null)
            animator.SetBool("isAttack", true);
        
        agent.SetDestination(player.position);
        
        isChasing = true;
        wasChasing = true;
    }
    
    private void ChaseLight()
    {
        if (agent == null || nearestLight == null || !agent.isOnNavMesh) return;
        
        agent.speed = ChaseVelocity;
        
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        agent.SetDestination(nearestLight.transform.position);
        
        isChasing = false;
        
        float distanceToLight = Vector3.Distance(transform.position, nearestLight.transform.position);
        if (distanceToLight < 2f)
        {
            LightSwitch lightSwitch = nearestLight.GetComponent<LightSwitch>();
            if (lightSwitch != null && lightSwitch.IsLightOn())
            {
                lightSwitch.SwitchButtonLight();
                
                //Sonido al apagar luz
                PlayTurnOffLightSound();
                
                Debug.Log("Enemigo apagó una luz!");
            }
        }
    }
    
    private void AttackPlayer()
    {
        if (agent == null || player == null || !agent.isOnNavMesh) return;
        
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);
        
        if (animator != null)
            animator.SetBool("isAttack", true);
        
        if (!alreadyAtacked && !isAttacking)
        {
            isAttacking = true;
            
            //Sonido de ataque
            PlayAttackSound();
            
            PlayerLookAtEnemy playerLook = player.GetComponent<PlayerLookAtEnemy>();
            if (playerLook != null)
            {
                playerLook.StartLookingAtEnemy(transform);
            }
            
            Invoke(nameof(DealDamage), AttackingTime);
            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }
    
    private void DealDamage()
    {
        if (player == null) return;
        
        Health health = player.GetComponent<Health>();
        if (health == null)
        {
            health = player.GetComponentInParent<Health>();
        }
        if (health == null)
        {
            health = player.GetComponentInChildren<Health>();
        }
        
        if (health != null)
        {
            health.RecibirDaño(damage);
        }
        else
        {
            Debug.LogError("No se encontró el componente Health en el jugador en ninguna parte!");
        }
        
        if (StartingPoint != null)
            transform.position = StartingPoint.position;
        
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        isAttacking = false;
    }
    
    private void ResetAttack()
    {
        alreadyAtacked = false;
        isAttacking = false;
    }
    
    // NUEVOS MÉTODOS DE AUDIO
    
    // Reproduce suspiros aleatorios mientras patrulla
    private void PlayPatrolSigh()
    {
        // Reproduce uno de 3 suspiros aleatorios en 3D
        AudioManager.Instance.PlayRandomSFX3DAtGameObject(
            gameObject,
            "Suspiro 1",
            "Suspiro 2",
            "Suspiro 3"
        );
    }
    
    // Sonido de ataque
    private void PlayAttackSound()
    {
        // Puedes usar sonidos aleatorios de ataque también
        AudioManager.Instance.PlayRandomSFX3DAtGameObject(gameObject,"Ataque Errante");
    }
    
    // Sonido al apagar una luz
    private void PlayTurnOffLightSound()
    {
        AudioManager.Instance.PlaySFX3D("Soplido", transform.position);
    }
            
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, lightDetectionRange);
    }
}