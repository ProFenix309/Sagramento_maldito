using UnityEngine.AI;
using UnityEngine;
using System.Threading;
using UnityEngine.Rendering;

public class ErranteAI : MonoBehaviour
{
    public bool lastChase = false;
    NavMeshAgent agent;
    Transform player;
    Transform Distraction;


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



    [Header("Ranges")]
    public float sightRange, attackRange, distractionRange;


    [Header("States")]
    public bool playerInSightRange, playerInAttackRange, DistractionISinRange;

    private void Awake()
    {
        //detects object by names on scene

        StartingPoint = GameObject.Find("StartingPoint").transform;
        player = GameObject.Find("Player").transform;

        if (GameObject.Find("Vela").transform)
            Distraction = GameObject.Find("Vela").transform;

        //gets the agent of the enemy
        agent = GetComponent<NavMeshAgent>();

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
        if (!lastChase)
        {
            //timer to not have a seizure/epilepsy
            randomTime -= Time.deltaTime;

            //Check for layer to attack/follow
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            DistractionISinRange = Physics.CheckSphere(transform.position, distractionRange, whatIsDistraction);

            //Enemy checks states to be in
            if (playerInAttackRange && playerInSightRange && !DistractionISinRange || playerInAttackRange && playerInSightRange && DistractionISinRange) AttackPlayer();

            if (randomTime <= 0.2f)
            {
                if (playerInSightRange && !playerInAttackRange && !DistractionISinRange || playerInSightRange &&  !playerInSightRange && DistractionISinRange) ChasePlayer();
                if (!playerInSightRange && !playerInAttackRange && !DistractionISinRange) Patroling();
                if (!playerInSightRange && !playerInAttackRange && DistractionISinRange || playerInSightRange && !playerInAttackRange && DistractionISinRange) ChaseDistraction();
                randomTime = initialRandomTime;
            }
            else
                walkPointSet = false;
        }
        else
        {
            agent.speed = ChaseVelocity;
            agent.SetDestination(player.position);
        }
    }

    private void Patroling()
    {
        //sets speed by default
        agent.speed = Velocity;

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

        //agent moves towards player
        agent.SetDestination(player.position);
    }
    private void ChaseDistraction()
    {
        agent.speed = agent.speed + ChaseVelocity;
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


        if (!alreadyAtacked)
        {
            ///Attack code here

            HealthManager health;

            if (player.gameObject.TryGetComponent(out health))
            {
                health.RecibirDaño(damage);
            }
            Debug.Log("Player attacked");

            //sets potsition to starting one (optional)
            transform.position = StartingPoint.position;

            alreadyAtacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
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
    //valores a modificar desde el inspector
    //watIsDistraction whatIsGround, whatIsPlayer,velocity, timeBetweenAtacks, sightRange, attackRange, diatractionRange, chaseVelocity, initialRandomTime, walkPointRange, damage, crear un objetao vacio para StartingPoint (punto inicial) 
}
