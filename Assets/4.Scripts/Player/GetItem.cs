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
        objeto.GetComponent<Rigidbody>().useGravity = false;
        objeto.GetComponent<Rigidbody>().isKinematic = true;
        objeto.transform.position = handPoint.transform.position;
        objeto.transform.SetParent(handPoint.transform);
        pickedObject = objeto;
        isHolding = true;
    }

    private void SoltarObjeto()
    {
        if (pickedObject != null)
        {
            pickedObject.GetComponent<Collider>().isTrigger = false;
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;

            Disappear disappearComponent = pickedObject.GetComponent<Disappear>();
            if (disappearComponent != null)
            {
                disappearComponent.Spawned = false;
            }

            pickedObject.transform.SetParent(null);
            pickedObject = null;
            isHolding = false;
        }
    }

    private void LanzarObjeto()
    {
        if (pickedObject == null) return;

        // CAMBIA el layer SOLO al lanzar
        pickedObject.layer = launchedLayer;

        Disappear disappearComponent = pickedObject.GetComponent<Disappear>();
        if (disappearComponent != null)
        {
            disappearComponent.Spawned = false;
        }

        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        pickedObject.GetComponent<Collider>().isTrigger = false;

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