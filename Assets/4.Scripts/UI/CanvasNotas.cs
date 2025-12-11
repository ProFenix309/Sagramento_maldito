using UnityEngine;
using System.Collections.Generic;

public class CanvasNotas : MonoBehaviour, Interactable
{
    public GameObject panelPeriodico;
    bool actived = false;
    [SerializeField] List<KeyCode> keys;

    void Update()
    {
        if (actived)
        {
            InteractionKey();
        }
    }
    
    public void Interact()
    {
        panelPeriodico.SetActive(true);
        actived = true; 
    }

    public void InteractionKey()
    {
        // Verificar si alguna tecla de la lista fue presionada
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                panelPeriodico.SetActive(false);
                actived = false; // Desactivar la detección de teclas
                break;
            }
        }
    }
}