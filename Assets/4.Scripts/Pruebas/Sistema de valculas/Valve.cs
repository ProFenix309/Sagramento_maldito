using UnityEngine;

public class Valve : MonoBehaviour, Interactable
{
    [Header("Estado")]
    [SerializeField] private bool activated = false;

    [Header("Rotacion del Modelo")]
    [SerializeField] private Transform valveModel;
    [SerializeField] private float maxRotation = 360f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public event System.Action OnValveActivated;

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        if (valveModel != null)
        {
            initialRotation = valveModel.localRotation;
            targetRotation = initialRotation * Quaternion.AngleAxis(maxRotation, rotationAxis);
        }
    }

    private void Update()
    {
        if (isRotating && valveModel != null)
        {
            valveModel.localRotation = Quaternion.RotateTowards(
                valveModel.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(valveModel.localRotation, targetRotation) < 0.1f)
            {
                valveModel.localRotation = targetRotation;
                isRotating = false;
                CompleteActivation();
            }
        }
    }

    public void Interact()
    {
        if (activated)
        {
            Debug.Log($"La valvula {gameObject.name} ya esta activada.");
            return;
        }

        ActivateValve();
    }

    private void ActivateValve()
    {
        isRotating = true;

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void CompleteActivation()
    {
        activated = true;
        Debug.Log($"Valvula {gameObject.name} activada!");
        OnValveActivated?.Invoke();
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void ResetValve()
    {
        activated = false;
        isRotating = false;
        if (valveModel != null)
        {
            valveModel.localRotation = initialRotation;
        }
    }
}