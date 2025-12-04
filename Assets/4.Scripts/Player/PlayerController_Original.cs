using UnityEngine;

public class PlayerController_Original : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movimiento")]

    [SerializeField] private float speed;
    [SerializeField] private float speedRun;
    private float horizontalAxis, verticalAxis;
    [HideInInspector] public bool stop = true;
    [HideInInspector] public bool canMove;
    [HideInInspector] float stopMovement = 0f;

    [Space, Header("Fuerza jump")]

    [SerializeField] private float jumpForce;

    [Space, Header("Detecci�n de suelos")]

    [SerializeField] private LayerMask layerColision;
    [SerializeField] private Transform checkedGround;
    [SerializeField] private float radiusGround;
    [SerializeField] public bool isGround;

    [Space, Header("Deteccion del Raycast")]
    [SerializeField] private float maxInteractDistance;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private LayerMask layerInteract;

    [Space, Header("Animator")]
    [SerializeField] Animator meshAnimator;

    [Space,Header("Inventario")]
    [SerializeField] Inventory inventory;

    [Space, Header("Paneles del canvas")]
    [SerializeField] GameObject interact;
    [SerializeField] GameObject grab;
    public GameObject interactableObject;
    public GameObject grabbableObject;

    [Space, Header("Audio")]
    
    private Health health;

    bool jump;
    public bool run;

    private void Awake()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        meshAnimator =  GameObject.Find("LUCY@Idle").GetComponent<Animator>();
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
        if (stop)
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
                interactableObject = null;
                grabbableObject = null;
                MovePlayer();
            }
        }
    }

    public void ChectkInteraction()
    {
        if (Physics.Raycast(rayPivot.position, rayPivot.forward, out RaycastHit hit, maxInteractDistance, layerInteract, QueryTriggerInteraction.Collide))
        {
            if (!inventory.inventoryEnabled)
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
        meshAnimator.SetBool("isWalking", true);
        float currentSpeed = run ? speedRun : (speed);

        if (!canMove)
        {
            currentSpeed = stopMovement;
        }
        Vector3 direction = (transform.right * horizontalAxis) + (transform.forward * verticalAxis);

        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);

        if (rb.linearVelocity == Vector3.zero)
        {
            meshAnimator.SetBool("isWalking", false);
        }
    }

    void GetInputs()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.LeftShift) && isGround)
        {
            run = true;
            meshAnimator.SetBool("isRun",true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            meshAnimator.SetBool("isRun", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
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

    public void GroundDetection()
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