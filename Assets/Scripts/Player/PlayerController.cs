using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;

    [Header("Movimiento")]
    float horizantalAxis, verticalAxis;
    [SerializeField] float speed;
    [SerializeField] float speedRun;
    [SerializeField] float speedCrouched;
    Vector3 move;
    bool crouched;

    [Space]
    [Header("Fuerza jump y Grabedad")]

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    [SerializeField] float jumpForce;

    [Space]
    [Header("Deteccion de suelos")]

    bool isGround;
    [SerializeField] LayerMask layerColision;
    [SerializeField] Transform checketGround;
    [SerializeField] float radiusGround;

    [Space]
    [Header("Animation")]
    [SerializeField] Animator animator;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    private void Update()
    {
        MovePlayer();
        Jump();
        Crouched();
        Run();
    }

    void MovePlayer()
    {
        horizantalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        move = transform.right * horizantalAxis + transform.forward * verticalAxis;
        character.Move(move * speed * Time.deltaTime);

        if (horizantalAxis != 0 || verticalAxis != 0)
        {
            //animator.SetBool("IsWalking", true);
        }

        else
        {
            //animator.SetBool("IsWalking", false);
        }
    }

    void Jump()
    {
        isGround = Physics.CheckSphere(checketGround.position, radiusGround, layerColision);

        if (isGround && velocity.y < -2)
        {
            velocity.y = -2;
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);
    }

    void Crouched()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouched = !crouched;
            if (crouched == true)
            {
                animator.SetBool("Crouched", true);
                character.Move(move * speedCrouched * Time.deltaTime);

            }
            else
            {
                animator.SetBool("Crouched", false);
            }
        }

    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Crouched", false);
            crouched = false;
            character.Move(move * speedRun * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checketGround.position, radiusGround);
    }

}
