using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<int, Items> Items { get => _items; }

    private Dictionary<int, Items> _items = new Dictionary<int, Items>();

    public Action InventoryUpdated;

    [SerializeField] public bool inventoryEnabled = false;

    public GameObject inventory;

    public InventoryUIHandler inventoryUIHandler;

    public GameObject slotHalder;

    PlayerController_Original playerController;
    Camera_FPS_Controller cameraController;

    [Header("Items Iniciales")]
    [SerializeField] public List<Items> initialItems = new List<Items>();

    public GameObject HandDetection;
    [HideInInspector] public GetItem itemH;
    [HideInInspector] public GameObject inventoryItem;

    bool unlockInputs = true;
    public bool UnlockInputs { get => unlockInputs; set => unlockInputs = value; }

    private void OnEnable()
    {
            LoadInfo();
    }
    public void LoadInfo()
    {
        if (inventoryUIHandler == null)
        {
            inventoryUIHandler = inventoryUIHandler.GetComponent<InventoryUIHandler>();
        }
        if (itemH == null)
        {
            itemH = HandDetection.GetComponent<GetItem>();
        }
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController_Original>();
        }
        if (cameraController == null)
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<Camera_FPS_Controller>();
        }

        if (Items != null)
        {
            foreach (var item in initialItems)
            {
                if (item != null)
                {
                    AddItem(item);
                    Debug.Log($"Item inicial '{item.type}' agregado al inventario");
                }
            }
        }

    }

    void Update()
    {
        LoadInfo();
        if (unlockInputs)
        {
            if (itemH != null) 
            {
                if (itemH.Item != null && !inventoryEnabled)
                {
                    inventoryItem = itemH.Item.gameObject;
                    if (inventoryItem.TryGetComponent(out Items item) && Input.GetKeyDown(KeyCode.E))
                    {
                        AddItem(item);
                        inventoryItem.gameObject.SetActive(false);
                        Debug.Log(inventoryItem + "is in your inventory");
                    }
                }
            }
            else
            {
                inventoryItem = null;
            }

            if (Input.GetKeyDown(KeyCode.I) && inventoryUIHandler.ItemInfoPanel.activeSelf == false)
            {
                inventoryEnabled = !inventoryEnabled;
                if (inventoryEnabled)
                {
                    if (playerController.canMove)
                    {
                        /*   foreach (var items in Items)
                           {
                               string name = items.Value.name;
                               Debug.Log(name);
                        }*/

                        Cursor.lockState = CursorLockMode.None;
                        playerController.canMove = false;
                        cameraController.canMove = false;
                    }
                }
                else
                {

                    playerController.canMove = true;
                    cameraController.canMove = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                if (!inventoryEnabled)
                {
                    inventory.GetComponent<CanvasGroup>().alpha = 0;
                    inventory.GetComponent<CanvasGroup>().interactable = false;
                    inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

                }
                if (inventoryEnabled)
                {
                    inventory.GetComponent<CanvasGroup>().alpha = 1;
                    inventory.GetComponent<CanvasGroup>().interactable = true;
                    inventory.GetComponent<CanvasGroup>().blocksRaycasts = true;

                }

            }
        }
    }
    public void AddItem(Items item)
    {
        if (!_items.ContainsKey(item.ID))
        {
            _items.Add(item.ID, item);
            InventoryUpdated?.Invoke();
            Debug.Log(item + "added to your inv");
        }
    }
    public bool TrySpendItem(int id)
    {
        if (_items.ContainsKey(id))
        {
            _items.Remove(id);
            InventoryUpdated?.Invoke();
            return true;
        }
        return false;
    }
}
