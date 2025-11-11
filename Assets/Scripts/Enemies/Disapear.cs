using System.Threading;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public float Detectionzone;
    public bool InSightRange;
    public LayerMask enemy;
    float timer = 2f;
    public bool Spawned = true;
        
    private void Update()
    {
        if (!Spawned)
        {
            InSightRange = Physics.CheckSphere(transform.position, Detectionzone, enemy);

            if (InSightRange)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                    Destroy(gameObject);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Detectionzone);
    }
}
