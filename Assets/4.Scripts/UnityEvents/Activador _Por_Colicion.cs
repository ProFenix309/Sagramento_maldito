using System.Collections.Generic;
using UnityEngine;

public class Activador_Por_Colicion : Activador_Eventos
{
    [SerializeField] List<string> tagName;

    private void OnTriggerEnter(Collider other)
    {
        if (tagName.Contains(other.tag))
        {
            eventosFinales?.Invoke();
        }
    }
}
