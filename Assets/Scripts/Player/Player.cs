using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float fuerzaSalto, fuerzaMove;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    Vector3 localMoveDirection;
    Vector3 worldMoveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        localMoveDirection = new Vector3(input.x, 0, input.y);
        worldMoveDirection = transform.TransformDirection(localMoveDirection);
    }
    private void FixedUpdate()
    {
        rb.AddForce(worldMoveDirection * fuerzaMove);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log(callbackContext.phase);
        }
    }
}
