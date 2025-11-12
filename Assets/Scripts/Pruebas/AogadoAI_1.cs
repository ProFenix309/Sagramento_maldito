using UnityEngine.AI;
using UnityEngine;


public class AhogadoAI_1 : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform Distraction;

    [SerializeField] string nameTarget;

    [Space]
    [Header("Layers")]
    public LayerMask whatIsGround, whatIsPlayer, whatIsDistraction;

    Transform StartingPoint;
    float Velocity;

    [Space]
    [Header("Patroling")]
    public float initialRandomTime;
    float randomTime;
    Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Space]
    [Header("Chasing")]
    public float velocity;
    public float ChaseVelocity;

    [Space]
    [Header("Atacking")]
    public float timeBetweenAtacks;
    public bool alreadyAtacked;
    public float AttackingTime;
    [SerializeField] float damage;
    private bool isAttacking = false; // Nuevo: controla si está en animación de ataque


    [Header("Ranges")]
    public float sightRange, attackRange, distractionRange;


    [Header("States")]
    public bool playerInSightRange, playerInAttackRange, DistractionISinRange;

    [Header("Animator")]
    public Animator animator;

    private void Awake()
    {
        //detects object by names on scene

        StartingPoint = GameObject.Find("StartingPoint").transform;
        player = GameObject.Find(nameTarget).transform;

        if (GameObject.Find("Distraction"))
            Distraction = GameObject.Find("Distraction").transform;

        //gets the agent of the enemy
        agent = GetComponent<NavMeshAgent>();

        //gets the animation of the enemy
        animator = GetComponent<Animator>();

        //sets the velocity of the agent to the one from the before pressing start
        velocity = agent.speed;
    }

    private void Start()
    {
        randomTime = initialRandomTime;
        Velocity = agent.speed;
    }
    private void Update()
    {
        //timer to not have a seizure/epilepsy
        randomTime -= Time.deltaTime;
       
        //Check for layer to attack/follow
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        DistractionISinRange = Physics.CheckSphere(transform.position, distractionRange, whatIsDistraction);

        //Enemy checks states to be in
        if (playerInAttackRange && playerInSightRange && !DistractionISinRange) AttackPlayer();

        if (randomTime <= 0.2f)
        {
            if (!playerInSightRange && !playerInAttackRange && !DistractionISinRange) Patroling();
            else  if (!playerInSightRange && !playerInAttackRange && DistractionISinRange || playerInSightRange && !playerInAttackRange && DistractionISinRange) 
            {
                Distraction.TryGetComponent<Disappear>(out Disappear spawn);
                if (spawn.Spawned == false)
                {
                    ChaseDistraction();
                }
                
            }
            else if (playerInSightRange && !playerInAttackRange && !DistractionISinRange) ChasePlayer();
            randomTime = initialRandomTime;
        }
        else
            walkPointSet = false;
    }

    private void Patroling()
    {
        //sets speed by default
        agent.speed = Velocity;

        //sets animation states
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunnig", false);
        animator.SetBool("isAttacking", false);

        //checks for points to travel to
        if (!walkPointSet) SearchWalkPoint();

        //walks to selected point
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        //the distance between the enemy and the point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude >= 0)
            walkPointSet = true;
        else walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
        walkPoint = hit.position;
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        //increases agent velocity
        agent.speed = ChaseVelocity;

        //sets animation states
        //animator.SetBool("isWalking", false);
        //animator.SetBool("isRunnig", true);
        //animator.SetBool("isAttacking", false);

        //agent moves towards player
        agent.SetDestination(player.position);
    }
    private void ChaseDistraction()
    {
        agent.speed += ChaseVelocity;

        //sets animation states
        //animator.setbool("iswalking", false);
        //animator.setbool("isrunnig", true);
        //animator.setbool("isattacking", false);

        if (Distraction != null)
            agent.SetDestination(Distraction.position);


        if (!alreadyAtacked)
        {
            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }
    private void AttackPlayer()
    {
        //Makes sure enemy doesn´t move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        //sets animation states
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunnig", false);

        if (!alreadyAtacked && !isAttacking)
        {
            // Inicia la animación de ataque
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            animator.SetTrigger("Attack"); // Trigger para iniciar la animación

            // Hace que el jugador mire al enemigo
            PlayerLookAtEnemy playerLook = player.GetComponent<PlayerLookAtEnemy>();
            if (playerLook != null)
            {
                playerLook.StartLookingAtEnemy(transform);
            }

            // Espera el tiempo de la animación antes de hacer daño
            Invoke(nameof(DealDamage), AttackingTime);

            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }

    // Nuevo método: se ejecuta después de que termine la animación de ataque
    private void DealDamage()
    {
        HealthManager health;

        if (player.gameObject.TryGetComponent(out health))
        {
            health.RecibirDaño(damage);
        }
        Debug.Log("Player attacked - Damage dealt!");

        //sets position to starting one (optional)
        transform.position = StartingPoint.position;

        // Termina la animación de ataque
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void ResetAttack()
    {
        alreadyAtacked = false;
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