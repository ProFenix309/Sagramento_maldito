using UnityEngine;

public class LightSwitch : MonoBehaviour, Interactable
{
    private Light lightComponent;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            lightComponent = GetComponentInChildren<Light>(); 
        }
    }

    public void Interact()
    {
        if (lightComponent != null)
        {
            lightComponent.enabled = !lightComponent.enabled;
        }
    }
}
