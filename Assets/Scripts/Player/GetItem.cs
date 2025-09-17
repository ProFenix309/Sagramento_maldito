using UnityEngine;

public class GetItem : MonoBehaviour
{
    public GameObject handPoint;
    public Camera playerCamera; 
    public float launchForce = 500f;
    public string objectName;

    private GameObject pikedObject = null;
    private bool isHolding = false;

    void Update()
    {
        if (isHolding && pikedObject != null && Input.GetMouseButtonDown(0))
        {
            LanzarObjeto();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(objectName) && Input.GetKeyDown(KeyCode.E))
        {
            isHolding = !isHolding;
            if (isHolding)
            {
                AgarrarObjeto(other.gameObject);
            }
            else
            {
                SoltarObjeto();
                pikedObject = null;
            }
        }
    }

    private void AgarrarObjeto(GameObject objeto)
    {
        objeto.GetComponent<Rigidbody>().useGravity = false;
        objeto.GetComponent<Rigidbody>().isKinematic = true;

        objeto.transform.position = handPoint.transform.position;

        objeto.transform.SetParent(handPoint.transform);

        pikedObject = objeto;
        isHolding = true;
    }

    private void SoltarObjeto()
    {
        if (pikedObject != null)
        {
            pikedObject.GetComponent<Rigidbody>().useGravity = true;
            pikedObject.GetComponent<Rigidbody>().isKinematic = false;

            pikedObject.transform.SetParent(null);

            pikedObject = null;
            isHolding = false;
        }
    }

    private void LanzarObjeto()
    {
        if (pikedObject == null) return;

        Rigidbody rb = pikedObject.GetComponent<Rigidbody>();

        // Soltar objeto para que la física actúe sobre él
        SoltarObjeto();

        // Aplicar fuerza hacia donde mira la cámara
        Vector3 direction = playerCamera.transform.forward;

        rb.AddForce(direction * launchForce);

        // Opcional: si quieres que se suelte inmediatamente, isHolding debe ser false y pikedObject null
    }
}
