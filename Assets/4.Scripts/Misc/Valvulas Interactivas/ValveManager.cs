using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValveManager : MonoBehaviour
{
    [Header("Valve Slots")]
    [SerializeField] private List<ValveSlot> valveSlots = new List<ValveSlot>();

    [Header("Completion Event")]
    [SerializeField] private UnityEvent onAllValvesActivated;

    [Header("Options")]
    [SerializeField] private bool autoFindSlotsInChildren = true;
    [SerializeField] private bool searchInSceneIfNotFound = true;
    [SerializeField] private bool checkOnStart = true;

    private bool allValvesActivated = false;
    public bool AllValvesActivated => allValvesActivated;

    private void Start()
    {
        if (autoFindSlotsInChildren)
        {
            FindValvesInChildren();
        }

        if (checkOnStart)
        {
            CheckAllValves();
        }
    }

    private void FindValvesInChildren()
    {
        ValveSlot[] foundSlots = GetComponentsInChildren<ValveSlot>(true);
        valveSlots.Clear();
        valveSlots.AddRange(foundSlots);
        
        if (valveSlots.Count == 0 && searchInSceneIfNotFound)
        {
            ValveSlot[] sceneSlots = FindObjectsByType<ValveSlot>(FindObjectsSortMode.None);
            
            if (sceneSlots.Length > 0)
            {
                valveSlots.AddRange(sceneSlots);
            }
        }

        Debug.Log($"Válvulas encontradas: {valveSlots.Count}");
    }

    public void OnValveActivated(ValveSlot activatedValve)
    {
        if (allValvesActivated) return;

        Debug.Log($"Válvula activada. Verificando estado...");
        CheckAllValves();
    }

    public void CheckAllValves()
    {
        if (valveSlots.Count == 0)
        {
            Debug.LogWarning("No hay válvulas registradas en el ValveManager.");
            return;
        }

        bool allActivated = true;
        int activatedCount = 0;
        
        foreach (ValveSlot valve in valveSlots)
        {
            if (valve == null) continue;

            if (valve.IsActivated)
            {
                activatedCount++;
            }
            else
            {
                allActivated = false;
            }
        }

        if (allActivated && !allValvesActivated)
        {
            allValvesActivated = true;
            Debug.Log("¡Todas las válvulas están activadas! Sistema completado.");
            onAllValvesActivated?.Invoke();
            SoundComplete();
        }
        else if (!allActivated)
        {
            int remaining = valveSlots.Count - activatedCount;
            Debug.Log($"Válvulas activadas: {activatedCount}/{valveSlots.Count} - Restantes: {remaining}");
        }
    }

    public void ResetAllValves()
    {
        allValvesActivated = false;
        foreach (ValveSlot valve in valveSlots)
        {
            if (valve != null)
            {
                valve.SetActivated(false);
            }
        }
        Debug.Log("Todas las válvulas han sido reiniciadas.");
    }

    public void AddValve(ValveSlot valve)
    {
        if (valve != null && !valveSlots.Contains(valve))
        {
            valveSlots.Add(valve);
            Debug.Log($"Válvula añadida. Total: {valveSlots.Count}");
        }
    }

    public void RemoveValve(ValveSlot valve)
    {
        if (valveSlots.Contains(valve))
        {
            valveSlots.Remove(valve);
            Debug.Log($"Válvula removida. Total: {valveSlots.Count}");
        }
    }

    private void SoundComplete()
    {
        AudioManager.Instance?.PlaySFX3D("ValveSystemComplete", transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (ValveSlot valve in valveSlots)
        {
            if (valve != null)
            {
                Gizmos.DrawLine(transform.position, valve.transform.position);
            }
        }

        Gizmos.color = allValvesActivated ? Color.green : Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}