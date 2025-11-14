using UnityEngine;
using System.Collections.Generic;

public class ValveController : MonoBehaviour
{
    [Header("Válvulas Requeridas")]
    [SerializeField] private List<Valve> valves = new List<Valve>();

    [Header("Plataformas a Controlar")]
    [SerializeField] private PlatformWithValves platforms;

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
            UnlockPlatforms();
        }
    }

    private void UnlockPlatforms()
    {
        if (platforms != null)
        {
            platforms.Unlock();
            Debug.Log("¡Todas las válvulas activadas! Descendiendo plataformas...");
        }
        else
        {
            Debug.LogWarning("No hay plataformas asignadas al ValveController.");
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