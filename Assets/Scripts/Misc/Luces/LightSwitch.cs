using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private Light targetLight;

    private void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponentInChildren<Light>();
    }

    public void ToggleLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }
        else
        {
            Debug.LogWarning("LightSwitch: No se encontró ninguna luz.");
        }
    }
}
