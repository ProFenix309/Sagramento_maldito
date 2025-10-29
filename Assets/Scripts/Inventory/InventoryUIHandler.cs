using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject slotPrefab;      // Prefab del Slot
    [SerializeField] Transform slotParent;       // Contenedor donde van los slots
    [SerializeField] GameObject inventoryPanel;

    [SerializeField] Button useButton;
    [SerializeField] TextMeshProUGUI useButtonText;

    [Header("Item Info Panel")]
    [SerializeField] GameObject itemInfoPanel;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;

    private readonly List<Slot> currentSlots = new();

    private void OnEnable()
    {
        inventory.InventoryUpdated += ShowInventory;
        ShowInventory();
        itemInfoPanel.SetActive(false);
    }

    private void OnDisable()
    {
        inventory.InventoryUpdated -= ShowInventory;
    }

    public void ShowInventory()
    {
        // Limpia los slots actuales
        foreach (var s in currentSlots)
        {
            Destroy(s.gameObject);
        }
        currentSlots.Clear();

        // Crea nuevos slots por cada ítem en el inventario
        foreach (var item in inventory.Items.Values)
        {
            var newSlotObj = Instantiate(slotPrefab, slotParent);
            var slot = newSlotObj.GetComponent<Slot>();
            slot.UpdateSlot(item);
            slot.SlotClicked += ShowItem;

            currentSlots.Add(slot);
        }
    }

    public void ShowItem(Items item)
    {
        float alphaIncrease = Mathf.Lerp(200,0, 2.5f*Time.deltaTime);
        float alphaDecrease = Mathf.Lerp(0, 200, 2.5f * Time.deltaTime);

        // Desactivar panel de inventario
        inventoryPanel.GetComponent<Image>().color = new Color(0, 0, 0, alphaIncrease);
        inventoryPanel.SetActive(false);


        // Activar panel de información
        
        itemInfoPanel.SetActive(true);
        inventoryPanel.GetComponent<Image>().color = new Color(0, 0, 0, alphaDecrease);

        // Mostrar datos del ítem
        itemIcon.sprite = item.icon;
        itemNameText.text = item.type;
        itemDescriptionText.text = item.description;

        // Mostrar botón de uso si corresponde
        useButton.gameObject.SetActive(item.useInInventory);
        if (item.useInInventory)
        {
            useButton.onClick.RemoveAllListeners();
            useButtonText.text = "Usar " + item.type;
        }
    }

    public void CloseItemInfoPanel()
    {
        itemInfoPanel.SetActive(false);
        inventoryPanel.SetActive(true); //reactiva el panel de slots
    }

}
