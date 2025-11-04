using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedRun = 8f;
    [SerializeField] private float speedCrouched = 2.5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask layerColision;
    [SerializeField] private Transform checkedGround;
    [SerializeField] private float radiusGround = 0.2f;

    // Referencias
    private Rigidbody rb;
    private PlayerAnimationController animController;

    // Estado público
    public bool CanMove { get; set; } = true;
    public bool IsGrounded { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCrouched { get; private set; }

    // Input privado
    private float horizontalAxis;
    private float verticalAxis;
    private bool jump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        if (CanMove)
        {
            GetMovementInput();
        }
    }

    private void FixedUpdate()
    {
        GroundDetection();

        if (CanMove)
        {
            MovePlayer();
            Jump();
        }
        else
        {
            MovePlayer();
        }
    }

    private void GetMovementInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        // Running
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded)
        {
            IsRunning = true;

            if (IsCrouched)
            {
                IsCrouched = false;
                animController?.UpdateCrouchState(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsRunning = false;
        }

        // Crouch
        if (Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift) && IsGrounded)
        {
            IsCrouched = !IsCrouched;
            animController?.UpdateCrouchState(IsCrouched);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && !IsCrouched)
        {
            jump = true;
        }
    }

    private void MovePlayer()
    {
        float currentSpeed = IsRunning ? speedRun : (IsCrouched ? speedCrouched : speed);

        if (!CanMove)
        {
            currentSpeed = 0f;
        }

        Vector3 direction = (transform.right * horizontalAxis) + (transform.forward * verticalAxis);
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);

        // Actualizar animaciones
        bool isWalking = rb.maxLinearVelocity >= 0.2f;
        animController?.UpdateMovementAnimation(isWalking);
    }

    private void Jump()
    {
        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    private void GroundDetection()
    {
        IsGrounded = Physics.CheckSphere(checkedGround.position, radiusGround, layerColision);
    }

    public void StopMovement()
    {
        horizontalAxis = 0;
        verticalAxis = 0;
        IsRunning = false;
    }

    private void OnDrawGizmos()
    {
        if (checkedGround != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(checkedGround.position, radiusGround);
        }
    }
}