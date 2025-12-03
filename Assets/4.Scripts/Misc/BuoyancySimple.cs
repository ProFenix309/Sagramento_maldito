using UnityEngine;

public class BuoyancySimple : MonoBehaviour
{
    [Header("Configuración de Flotación")]
    [SerializeField] private float buoyancyForce = 50f;
    [SerializeField] private float waterDrag = 2f;
    [SerializeField] private float waterAngularDamping = 1f;
    [SerializeField] private string nameWater;

    private Rigidbody rigidBody;
    private bool isInWater = false;
    private Vector3 waterSurfacePosition;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        if (rigidBody == null)
        {
            Debug.LogError("El objeto debe tener un componente Rigidbody");
        }
    }
    
    void FixedUpdate()
    {
        if (!isInWater)
            return;
        
        ApplyBuoyancyForce();
    }
    
    void ApplyBuoyancyForce()
    {
        float depthInWater = waterSurfacePosition.y - transform.position.y;
        
        if (depthInWater > 0)
        {
            float force = buoyancyForce * depthInWater;
            rigidBody.AddForce(Vector3.up * force, ForceMode.Acceleration);
            
            rigidBody.linearDamping = waterDrag;  
            rigidBody.angularDamping = waterAngularDamping; 
        }
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(nameWater))
        {
            isInWater = true;
            waterSurfacePosition = collision.bounds.max;
        }
    }
    
    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(nameWater))
        {
            waterSurfacePosition = collision.bounds.max;
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(nameWater))
        {
            isInWater = false;
            rigidBody.linearDamping = 0f;  
            rigidBody.angularDamping = 0.05f;  
        }
    }
}