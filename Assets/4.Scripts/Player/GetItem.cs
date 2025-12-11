using UnityEngine;

public class GetItem : MonoBehaviour
{
    public GameObject handPoint;
    public Camera playerCamera;
    public float launchForce = 500f;
    public string objectName;
    public GameObject Item;

    [Header("Layer Settings")]
    public string launchedLayerName = "LaunchedObject"; // Layer cuando es lanzado

    private GameObject pickedObject = null;
    private bool isHolding = false;
    private int launchedLayer;

    void Start()
    {
        // Obtener el índice del layer
        launchedLayer = LayerMask.NameToLayer(launchedLayerName);
    }

    void Update()
    {
        if (isHolding && pickedObject != null && Input.GetMouseButtonDown(0))
        {
            LanzarObjeto();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(objectName) && Input.GetKeyDown(KeyCode.E) && other.gameObject.GetComponent<Items>() == null)
        {
            // Verificar si el objeto tiene Rigidbody antes de intentar agarrarlo
            if (!other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Debug.LogWarning($"El objeto '{other.gameObject.name}' no tiene Rigidbody. No se puede agarrar.");
                return;
            }

            isHolding = !isHolding;
            if (isHolding)
            {
                other.isTrigger = true;
                AgarrarObjeto(other.gameObject);
            }
            else
            {
                SoltarObjeto();
                pickedObject = null;
            }
        }
        else if (other.gameObject.CompareTag(objectName) && other.gameObject.GetComponent<Items>() != null)
        {
            Item = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item = null;
    }

    private void AgarrarObjeto(GameObject objeto)
    {
        // Verificación segura del Rigidbody
        if (!objeto.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Debug.LogError($"No se puede agarrar '{objeto.name}': falta Rigidbody");
            return;
        }

        rb.useGravity = false;
        rb.isKinematic = true;
        objeto.transform.position = handPoint.transform.position;
        objeto.transform.SetParent(handPoint.transform);
        pickedObject = objeto;
        isHolding = true;
    }

    private void SoltarObjeto()
    {
        if (pickedObject == null) return;

        // Verificación segura del Collider
        if (pickedObject.TryGetComponent<Collider>(out Collider col))
        {
            col.isTrigger = false;
        }

        // Verificación segura del Rigidbody
        if (pickedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        Disappear disappearComponent = pickedObject.GetComponent<Disappear>();
        if (disappearComponent != null)
        {
            disappearComponent.Spawned = false;
        }

        pickedObject.transform.SetParent(null);
        pickedObject = null;
        isHolding = false;
    }

    private void LanzarObjeto()
    {
        if (pickedObject == null) return;

        // Verificación segura del Rigidbody
        if (!pickedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Debug.LogError($"No se puede lanzar '{pickedObject.name}': falta Rigidbody");
            pickedObject = null;
            isHolding = false;
            return;
        }

        // CAMBIA el layer SOLO al lanzar
        pickedObject.layer = launchedLayer;

        Disappear disappearComponent = pickedObject.GetComponent<Disappear>();
        if (disappearComponent != null)
        {
            disappearComponent.Spawned = false;
        }

        // Verificación segura del Collider
        if (pickedObject.TryGetComponent<Collider>(out Collider col))
        {
            col.isTrigger = false;
        }

        // Soltar objeto para que la física actúe sobre él
        pickedObject.transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;

        // Aplicar fuerza hacia donde mira la cámara
        Vector3 direction = playerCamera.transform.forward;
        rb.AddForce(direction * launchForce);

        pickedObject = null;
        isHolding = false;
    }
}