using UnityEngine;
using System.Collections.Generic;


public class interactive_lights : MonoBehaviour, Interactable
{
    public Light Light;
    public float lightRange;

    public float onLight;
    public float offLight;

    private void Start()
    {
        Light = GetComponent<Light>();
    }
    public void Interact()
    {
        if (Light.intensity == onLight)
        {
            Light.intensity = offLight;
        }
        else
        {
            Light.intensity = onLight;
        }
    }

  
}
