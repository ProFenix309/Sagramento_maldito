using UnityEngine;

public class ValveSlot : MonoBehaviour, Interactable, ItemRequierement
{
    [Header("Item Requirement")]
    [SerializeField] private int requiredItemID;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject activatedVisual;
    [SerializeField] private GameObject deactivatedVisual;
    [SerializeField] private GameObject valveModel; // El modelo 3D de la válvula que girará

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.forward; // Eje de rotación (Z por defecto)
    [SerializeField] private float rotationSpeed = 180f; // Grados por segundo
    [SerializeField] private float totalRotation = 720f; // Rotación total (2 vueltas completas)

    private bool isActivated = false;
    private bool isRotating = false;
    private float currentRotation = 0f;

    public bool IsActivated => isActivated;
    public int ItemID => requiredItemID;

    private void Start()
    {
        UpdateVisuals();
    }

    private void Update()
    {
        if (isRotating && valveModel != null)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            currentRotation += rotationStep;

            valveModel.transform.Rotate(rotationAxis, rotationStep, Space.Self);

            if (currentRotation >= totalRotation)
            {
                isRotating = false;
                currentRotation = totalRotation;
            }
        }
    }

    public void Interact()
    {
        if (isActivated)
        {
            Debug.Log("Esta válvula ya está activada.");
            return;
        }

        ActivateValve();
    }

    private void ActivateValve()
    {
        if (isActivated) return;

        isActivated = true;
        UpdateVisuals();
        
        // Iniciar la rotación de la válvula
        isRotating = true;
        currentRotation = 0f;

        SoundValve();

        ValveManager manager = FindValveManager();
        if (manager != null)
        {
            manager.OnValveActivated(this);
        }
    }

    private void UpdateVisuals()
    {
        if (activatedVisual != null)
        {
            activatedVisual.SetActive(isActivated);
        }

        if (deactivatedVisual != null)
        {
            deactivatedVisual.SetActive(!isActivated);
        }
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
        
        if (!activated)
        {
            isRotating = false;
            currentRotation = 0f;
            
            // Resetear la rotación del modelo
            if (valveModel != null)
            {
                valveModel.transform.localRotation = Quaternion.identity;
            }
        }
        
        UpdateVisuals();
    }

    private ValveManager FindValveManager()
    {
        ValveManager manager = GetComponentInParent<ValveManager>();

        if (manager == null)
        {
            manager = FindAnyObjectByType<ValveManager>();
        }

        return manager;
    }

    public void SoundValve()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX3D("Fuego", transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isActivated ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
    }
}