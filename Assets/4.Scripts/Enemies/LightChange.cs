using UnityEngine;

public class LightChange : MonoBehaviour
{
    Light lightDistraction;

    public float radioLight;
    float inicialLight;
    float currentLight =0;

    

    bool enemyInsightRange;


    public LayerMask whatIsEnemy;

    void Start()
    {
        lightDistraction = GetComponent<Light>();

        radioLight = lightDistraction.range;
        inicialLight = lightDistraction.intensity;
    }

    // Update is called once per frame
    void Update()
    {

        enemyInsightRange = Physics.CheckSphere(transform.position,radioLight,whatIsEnemy);

        if (enemyInsightRange)
        {
            lightDistraction.intensity = currentLight;
        }
        else
        {
            lightDistraction.intensity = inicialLight; 
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioLight);
    }
}
