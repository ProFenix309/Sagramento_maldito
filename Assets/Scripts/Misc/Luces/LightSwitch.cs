using UnityEngine;
using System.Collections.Generic;

public class LightSwitch : MonoBehaviour, Interactable 
{
    public
        Light luz;
    private bool encendida = false;

    private void Awake()
    {
        luz = GetComponent<Light>();
        if (luz == null)
        {
            Debug.LogWarning("No se encontró un componente Light en este objeto interactuable.");
        }
    }

    public  void Interact()
    {
        if (luz == null) return;

        encendida = !encendida;
        luz.enabled = encendida;

        Debug.Log($"Luz {(encendida ? "encendida" : "apagada")}");
    }
}