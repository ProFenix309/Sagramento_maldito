using UnityEngine.AI;
using UnityEngine;
using System.Threading;

public class EnemyAI : MonoBehaviour
{

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
    public float randomTime = 1f;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;


    [Space]
    [Header("Chasing")]
    public float ChaseVelocity;


    [Space]
    [Header("Atacking")]
    public float timeBetweenAtacks;
    public bool alreadyAtacked;
    public float AttackingTime;

    [Space]
    [Header("States")]
    public float sightRange, attackRange, distractionRange;
    public bool playerInSightRange, playerInAttackRange, DistractionISinRange;




    private void Awake()
    {
        //detects object by names on scene

        StartingPoint = GameObject.Find("StartingPoint").transform;
        player = GameObject.Find("Player_2").transform;
        Distraction = GameObject.Find("Distraction").transform;


        agent = GetComponent<NavMeshAgent>();


    }

    private void Start()
    {
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
        if (randomTime <= 0.2f)
        {
            if (!playerInSightRange && !playerInAttackRange && DistractionISinRange || playerInSightRange && !playerInAttackRange && DistractionISinRange) ChaseDistraction();
            if (playerInAttackRange && playerInSightRange && !DistractionISinRange) AttackPlayer();
            if (!playerInSightRange && !playerInAttackRange && !DistractionISinRange) Patroling();
            if (playerInSightRange && !playerInAttackRange && !DistractionISinRange) ChasePlayer();
            
           

            randomTime = 1f;
        }
        else
            walkPointSet = false;





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
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {
        //increases agent velocity
        agent.speed = agent.speed + ChaseVelocity;

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

                Debug.Log("is Attacking");

                ///


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

    //watIsDistraction whatIsGround, whatIsPlayer,velocity, timeBetweenAtacks, sightRange, attackRange, diatractionRange, chaseVelocity, walkPointRange, crear un objetao vacio para StartingPoint (punto inicial) 
}
