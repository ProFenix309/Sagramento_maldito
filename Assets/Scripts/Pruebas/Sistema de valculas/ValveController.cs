using UnityEngine;
using System.Collections.Generic;

public class ValveController : MonoBehaviour
{
    [Header("Válvulas Requeridas")]
    [SerializeField] private List<Valve> valves = new List<Valve>();

    [Header("Puerta a Controlar")]
    [SerializeField] private DoorWithValves door;

    private int activatedCount = 0;

    private void Start()
    {
        foreach (Valve valve in valves)
        {
            if (valve != null)
            {
                valve.OnValveActivated += OnValveActivated;
            }
        }
    }

    private void OnValveActivated()
    {
        activatedCount++;
        Debug.Log($"Válvulas activadas: {activatedCount}/{valves.Count}");

        CheckAllValves();
    }

    private void CheckAllValves()
    {
        bool allActivated = true;

        foreach (Valve valve in valves)
        {
            if (valve != null && !valve.IsActivated())
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated)
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        if (door != null)
        {
            door.Unlock();
            Debug.Log("¡Todas las válvulas activadas! Desbloqueando puerta...");
        }
        else
        {
            Debug.LogWarning("No hay puerta asignada al ValveController.");
        }
    }

    public int GetActivatedCount()
    {
        return activatedCount;
    }

    public int GetTotalValves()
    {
        return valves.Count;
    }

    private void OnDestroy()
    {
        foreach (Valve valve in valves)
        {
            if (valve != null)
            {
                valve.OnValveActivated -= OnValveActivated;
            }
        }
    }
}