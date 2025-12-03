using Unity.Mathematics;
using UnityEngine;

public class Candle_Controller : MonoBehaviour
{
    [SerializeField] Light lightPoint;
    [SerializeField] GameObject flame;
    [SerializeField] float maxIntencity;
    [SerializeField] bool actived;
    [SerializeField] LayerMask layerOff;

    void Start()
    {
        lightPoint.GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            actived = !actived;
            if (!actived)
            {
                lightPoint.intensity = maxIntencity;
                flame.SetActive(true);
            }
            else
            {
                lightPoint.intensity = 0f;
                flame.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerOff)
        {
            actived = false;
        }
    }
}
