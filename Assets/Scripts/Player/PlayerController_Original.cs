using UnityEngine;

public class PlayerController_Original : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movimiento")]

    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    [SerializeField] private float speedCrouched;
    private float horizontalAxis, verticalAxis;
    [SerializeField] private bool crouched;
    [HideInInspector] public bool canMove;
    [HideInInspector] float stopMovement = 0f;

    [Space, Header("Fuerza jump y Gravedad")]

    [SerializeField] private float jumpForce;

    [Space, Header("Detecci�n de suelos")]

    [SerializeField] private LayerMask layerColision;
    [SerializeField] private Transform checkedGround;
    [SerializeField] private float radiusGround;
    [SerializeField] private bool isGround;

    [Space, Header("Deteccion del Raycast")]
    [SerializeField] private float maxInteractDistance;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private LayerMask layerInteract;

    [Space, Header("Animator")]

    [SerializeField] Animator animator;
    [SerializeField] Animator meshAnimator;


    [SerializeField] Inventory inventory;

    [Space, Header("Paneles del canvas")]
    [SerializeField] GameObject interact;
    [SerializeField] GameObject grab;

    public GameObject interactableObject;
    public GameObject grabbableObject;

    private Health health;


    bool jump;
    public bool run;

    private void Awake()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshAnimator = GameObject.Find("Lucy").GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (health.vidaMaxima == 0) return;

        else if (canMove)
        {
            GetInputs();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            ChectkInteraction();
            GroundDetection();
            MovePlayer();
            Jump();
        }
        else
        {
            MovePlayer();
        }
    }

    public void ChectkInteraction()
    {
        if (gameObject.GetComponent<Inventory>().inventoryEnabled == true)
        {
            interactableObject = null;
            grabbableObject = null;
            return;
        }
        if (Physics.Raycast(rayPivot.position, rayPivot.forward, out RaycastHit hit, maxInteractDistance, layerInteract, QueryTriggerInteraction.Collide))
        {

            if (hit.collider.TryGetComponent(out Interactable interact))
            {
                interactableObject = hit.collider.gameObject;

                grabbableObject = null;
            }
            else if (hit.collider.CompareTag("Item"))
            {
                grabbableObject = hit.collider.gameObject;

                interactableObject = null;
            }
            else
            {
                interactableObject = null;
                grabbableObject = null;
            }

        }
        else 
        {
            interactableObject = null;
            grabbableObject = null;
        }

        grab.SetActive(grabbableObject != null ? true : false);
        interact.SetActive(interactableObject != null ? true : false);
    }

    public void MovePlayer()
    {
        float currentSpeed = run ? speedRun : (crouched ? speedCrouched : speed);

        if (!canMove)
        {
            currentSpeed = stopMovement;
        }
        Vector3 direction = (transform.right * horizontalAxis) + (transform.forward * verticalAxis);

        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);
        if (rb.maxLinearVelocity < 0.2f)
        {
            meshAnimator.SetBool("Walking", false);
        }
        else
        {
            meshAnimator.SetBool("Walking", true);
        }
    }

    void GetInputs()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && isGround)
        {
            run = true;

            if (crouched)
            {
                crouched = false;
                animator.SetBool("Crouched", false);
                meshAnimator.SetBool("Crouched", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift) && isGround)
        {
            crouched = !crouched;
            animator.SetBool("Crouched", crouched);
            meshAnimator.SetBool("Crouched", crouched);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround && !crouched)
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && interactableObject && isGround && canMove)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (interactableObject.TryGetComponent(out Interactable interactableItem))
        {
            if (interactableObject.TryGetComponent(out ItemRequierement requierement))
            {
                if (inventory.TrySpendItem(requierement.ItemID))
                {
                    interactableItem?.Interact();
                }
            }
            else
            {
                interactableItem?.Interact();
            }
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayPivot.position, rayPivot.position + (rayPivot.forward * maxInteractDistance));
    }
}
