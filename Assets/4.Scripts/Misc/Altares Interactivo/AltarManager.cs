using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AltarManager : MonoBehaviour
{
    [Header("Altar Slots")]
    [SerializeField] private List<AltarSlot> altarSlots = new List<AltarSlot>();

    [Header("Completion Event")]
    [SerializeField] private UnityEvent onAllSlotsActivated;

    [Header("Options")]
    [SerializeField] private bool autoFindSlotsInChildren = true;
    [SerializeField] private bool searchInSceneIfNotFound = true;
    [SerializeField] private bool checkOnStart = true;

    private bool allSlotsActivated = false;
    public bool AllSlotsActivated => allSlotsActivated;

    private void Start()
    {
        if (autoFindSlotsInChildren)
        {
            FindSlotsInChildren();
        }

        if (checkOnStart)
        {
            CheckAllSlots();
        }
    }

    private void FindSlotsInChildren()
    {
        AltarSlot[] foundSlots = GetComponentsInChildren<AltarSlot>(true);
        altarSlots.Clear();
        altarSlots.AddRange(foundSlots);
        
        if (altarSlots.Count == 0 && searchInSceneIfNotFound)
        {
            AltarSlot[] sceneSlots = FindObjectsOfType<AltarSlot>();
            
            if (sceneSlots.Length > 0)
            {
                altarSlots.AddRange(sceneSlots);
            }
        }
    }

    public void OnSlotActivated(AltarSlot activatedSlot)
    {
        if (allSlotsActivated) return;

        CheckAllSlots();
    }

    public void CheckAllSlots()
    {
        if (altarSlots.Count == 0) return;

        bool allActivated = true;
        int activatedCount = 0;
        
        foreach (AltarSlot slot in altarSlots)
        {
            if (slot == null) continue;

            if (slot.IsActivated)
            {
                activatedCount++;
            }
            else
            {
                allActivated = false;
            }
        }

        if (allActivated && !allSlotsActivated)
        {
            allSlotsActivated = true;
            Debug.Log("¡Todos los slots están activados! Ejecutando evento...");
            onAllSlotsActivated?.Invoke();
        }
        else if (!allActivated)
        {
            int remaining = altarSlots.Count - activatedCount;
            Debug.Log($"Slots activados: {activatedCount}/{altarSlots.Count} - Restantes: {remaining}");
        }
    }

    public void ResetAllSlots()
    {
        allSlotsActivated = false;
        foreach (AltarSlot slot in altarSlots)
        {
            if (slot != null)
            {
                slot.SetActivated(false);
            }
        }
        Debug.Log("Todos los slots han sido reiniciados.");
    }

    public void AddSlot(AltarSlot slot)
    {
        if (slot != null && !altarSlots.Contains(slot))
        {
            altarSlots.Add(slot);
        }
    }

    public void RemoveSlot(AltarSlot slot)
    {
        if (altarSlots.Contains(slot))
        {
            altarSlots.Remove(slot);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (AltarSlot slot in altarSlots)
        {
            if (slot != null)
            {
                Gizmos.DrawLine(transform.position, slot.transform.position);
            }
        }

        Gizmos.color = allSlotsActivated ? Color.green : Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);
    }
}

