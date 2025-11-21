using UnityEngine;
using UnityEngine.Events;

public class Activador_Eventos : MonoBehaviour
{
    [SerializeField] UnityEvent eventosIniciales;
    [SerializeField] protected UnityEvent eventosFinales;

    private void Start()
    {
        eventosIniciales?.Invoke();
    }
}
