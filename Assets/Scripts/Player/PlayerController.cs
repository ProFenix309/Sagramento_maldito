using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movimiento")]

    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    [SerializeField] private float speedCrouched;
    private float horizontalAxis, verticalAxis;
    [SerializeField] private bool crouched;
    [HideInInspector] public bool canMove;

    [Space, Header("Fuerza jump y Gravedad")]

    [SerializeField] private float jumpForce;

    [Space, Header("Detecci�n de suelos")]

    [SerializeField] private LayerMask layerColision;
    [SerializeField] private Transform checkedGround;
    [SerializeField] private float radiusGround;
    [SerializeField] private bool isGround;

    [Space, Header("Animator")]

    [SerializeField] Animator animator;

    bool jump;
    bool run;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            GetInputs();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            GroundDetection();
            MovePlayer();
            Jump();
        }
    }
    public void MovePlayer()
    {
        float currentSpeed = run ? speedRun : (crouched ? speedCrouched : speed);
        Vector3 direction = (transform.right * horizontalAxis) + (transform.forward * verticalAxis);
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);
    }

    void GetInputs()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            run = true;

            if (crouched)
            {
                crouched = false;
                animator.SetBool("Crouched", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouched = !crouched;
            animator.SetBool("Crouched", crouched);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jump = true;
        }
    }

    void GroundDetection()
    {
        isGround = Physics.CheckSphere(checkedGround.position, radiusGround, layerColision);
    }

    public void Jump()
    {
        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkedGround.position, radiusGround);
    }
}
