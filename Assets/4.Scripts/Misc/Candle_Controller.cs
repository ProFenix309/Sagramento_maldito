using Unity.Mathematics;
using UnityEngine;

public class Candle_Controller : MonoBehaviour
{
    [SerializeField] Light lightPoint;
    [SerializeField] GameObject flame;
    [SerializeField] float intencity;
    [SerializeField] float range;
    [SerializeField] bool actived;
    [SerializeField] LayerMask layerOff;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    void Start()
    {
        lightPoint.GetComponent<Light>();
        audioSource.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            actived = !actived;
            if (!actived)
            {
                lightPoint.intensity = intencity;
                lightPoint.range = range;
                flame.SetActive(true);
                audioSource.PlayOneShot(audioClip);
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
