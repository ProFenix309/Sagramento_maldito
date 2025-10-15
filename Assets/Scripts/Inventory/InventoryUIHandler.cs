using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] List<Slot> slots;
    [SerializeField] Button useButton;
    [SerializeField] TextMeshProUGUI useButtonText;

    private void OnEnable()
    {
        ShowInventory();

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SlotClicked += ShowItem;
        }

        inventory.InventoryUpdated += ShowInventory;
    }
    private void OnDisable()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SlotClicked -= ShowItem;
        }

        inventory.InventoryUpdated -= ShowInventory;
    }
    public void ShowItem(Items item)
    {
        if (item.useInInventory)
        {
            useButton.onClick.RemoveAllListeners();
            //useButton.onClick.AddListener(() => );
            useButtonText.text = "Use " + item.type;
        }

        useButton.gameObject.SetActive(item.useInInventory);
    }
    public void ShowInventory()
    {
        int index = 0;
        foreach (var item in inventory.Items)
        {
            slots[index].UpdateSlot(item.Value);
            index++;
        }

        for (int i = index; i < slots.Count; i++)
        {
            slots[i].UpdateSlot(null);
        }
    }
}
