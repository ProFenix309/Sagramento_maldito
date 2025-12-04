using UnityEngine.AI;
using UnityEngine;

public class AhogadoAI_1 : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform Distraction;
    
    [SerializeField] string nameTarget = "Player";
    
    [Space]
    [Header("Layers")]
    public LayerMask whatIsGround, whatIsPlayer, whatIsDistraction;
    
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
    public float velocity = 3.5f;
    public float ChaseVelocity = 6f;
    
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
    public float distractionRange = 20f;
    
    [Header("States")]
    public bool playerInSightRange, playerInAttackRange, DistractionISinRange;
    
    [Header("Animator")]
    public Animator animator;
    
    private void Awake()
    {
        // Gets the agent and animator
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Try to find StartingPoint (optional)
        GameObject startObj = GameObject.Find("StartingPoint");
        if (startObj != null)
            StartingPoint = startObj.transform;
        else
            StartingPoint = transform; // Use current position if not found
        
        // Try to find player (optional - can be null)
        if (!string.IsNullOrEmpty(nameTarget))
        {
            GameObject playerObj = GameObject.Find(nameTarget);
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        // Try to find distraction (optional)
        GameObject distractionObj = GameObject.Find("Distraction");
        if (distractionObj != null)
            Distraction = distractionObj.transform;
        
        // Set velocity
        if (agent != null)
            velocity = agent.speed;
    }
    
    private void Start()
    {
        randomTime = initialRandomTime;
        Velocity = velocity;
    }
    
    private void Update()
    {
        // Timer to control state changes
        randomTime -= Time.deltaTime;
        
        // Check for layers only if player/distraction exist
        playerInSightRange = player != null && Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = player != null && Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        DistractionISinRange = Distraction != null && Physics.CheckSphere(transform.position, distractionRange, whatIsDistraction);
        
        // Enemy checks states
        if (playerInAttackRange && playerInSightRange && !DistractionISinRange)
        {
            AttackPlayer();
        }
        else if (randomTime <= 0.2f)
        {
            if (DistractionISinRange)
            {
                ChaseDistraction();
            }
            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else
            {
                // Default: always patrol if nothing else to do
                Patroling();
            }
            
            randomTime = initialRandomTime;
        }
    }
    
    private void Patroling()
    {
        if (agent == null) return;
        
        // Set speed by default
        agent.speed = Velocity;
        
        // Set animation states
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        // Check for points to travel to
        if (!walkPointSet)
            SearchWalkPoint();
        
        // Walk to selected point
        if (walkPointSet && agent.isOnNavMesh)
            agent.SetDestination(walkPoint);
        
        // Check if walkpoint reached
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                walkPointSet = true;
        }
    }
    
    private void ChasePlayer()
    {
        if (agent == null || player == null) return;
        
        // Increase agent velocity
        agent.speed = ChaseVelocity;
        
        // Set animation states
        if (animator != null)
            animator.SetBool("isAttack", true);
        
        // Agent moves towards player
        if (agent.isOnNavMesh)
            agent.SetDestination(player.position);
    }
    
    private void ChaseDistraction()
    {
        if (agent == null || Distraction == null) return;
        
        agent.speed = ChaseVelocity;
        
        // Set animation states
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        if (agent.isOnNavMesh)
            agent.SetDestination(Distraction.position);
        
        if (!alreadyAtacked)
        {
            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }
    
    private void AttackPlayer()
    {
        if (agent == null || player == null) return;
        
        // Makes sure enemy doesn't move
        if (agent.isOnNavMesh)
            agent.SetDestination(transform.position);
        
        transform.LookAt(player);
        
        // Set animation states
        if (animator != null)
            animator.SetBool("isAttack", true);
        
        if (!alreadyAtacked && !isAttacking)
        {
            isAttacking = true;
            
            // Make player look at enemy
            PlayerLookAtEnemy playerLook = player.GetComponent<PlayerLookAtEnemy>();
            if (playerLook != null)
            {
                playerLook.StartLookingAtEnemy(transform);
            }
            
            // Wait for animation time before dealing damage
            Invoke(nameof(DealDamage), AttackingTime);
            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }
    
    private void DealDamage()
    {
        if (player == null) return;
        
        HealthManager health = player.GetComponent<HealthManager>();
        if (health != null)
        {
            health.RecibirDaño(damage);
        }
        
        Debug.Log("Player attacked - Damage dealt!");
        
        // Set position to starting one (optional)
        if (StartingPoint != null)
            transform.position = StartingPoint.position;
        
        // End attack animation
        if (animator != null)
            animator.SetBool("isAttack", false);
        
        isAttacking = false;
    }
    
    private void ResetAttack()
    {
        alreadyAtacked = false;
        isAttacking = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distractionRange);
    }
}