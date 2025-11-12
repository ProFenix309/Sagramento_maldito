using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float maxInteractDistance = 3f;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private LayerMask layerInteract;

    [Header("UI References")]
    [SerializeField] private GameObject interact;
    [SerializeField] private GameObject grab;

    private Inventory inventory;
    private PlayerMovement movement;
    private GameObject interactableObject;
    private GameObject grabbableObject;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        movement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (movement.CanMove)
        {
            CheckInteraction();
        }
    }

    private void Update()
    {
        if (movement.CanMove)
        {
            HandleInteractionInput();
        }
    }

    private void CheckInteraction()
    {
        if (Physics.Raycast(rayPivot.position, rayPivot.forward, out RaycastHit hit,
            maxInteractDistance, layerInteract, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent(out Interactable interactComponent))
            {
                interactableObject = hit.collider.gameObject;
            }
            else
            {
                interactableObject = null;
            }

            if (hit.collider.CompareTag("Item"))
            {
                grabbableObject = hit.collider.gameObject;
            }
            else
            {
                grabbableObject = null;
            }
        }
        else
        {
            interactableObject = null;
            grabbableObject = null;
        }

        UpdateInteractionUI();
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null && movement.IsGrounded)
        {
            Interact();
        }
    }

    private void Interact()
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

    private void UpdateInteractionUI()
    {
        if (grab != null)
            grab.SetActive(grabbableObject != null);

        if (interact != null)
            interact.SetActive(interactableObject != null);
    }

    private void OnDrawGizmos()
    {
        if (rayPivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rayPivot.position, rayPivot.position + (rayPivot.forward * maxInteractDistance));
        }
    }
}