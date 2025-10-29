using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour 
{
    public Dictionary<int, Items> Items { get => _items; }
    Dictionary<int, Items> _items = new();

    public Action InventoryUpdated;

    [SerializeField] private bool inventoryEnabled = false;

    public GameObject inventory;

    public GameObject slotHalder;

    PlayerController playerController;
    Camera_FPS_Controller cameraController;

    [HideInInspector]public  GameObject HandDetection;
    [HideInInspector]public GetItem itemH;
    [HideInInspector]public GameObject inventoryItem;

    private void Awake()
    {
        HandDetection = GameObject.Find("Hand");
    }
    void Start()
    {
        itemH = HandDetection.GetComponent<GetItem>();
        playerController = GetComponent<PlayerController>();
        cameraController = GameObject.Find("Main Camera").GetComponent<Camera_FPS_Controller>();
    }

    void Update()
    {
        if (itemH.Item != null)
        {
            Debug.Log(inventoryItem);

            inventoryItem = itemH.Item.gameObject;
            if (inventoryItem.TryGetComponent(out Items item) && Input.GetKeyDown(KeyCode.E))
            {   
                AddItem(item);
                inventoryItem.gameObject.SetActive(false);
                Debug.Log(inventoryItem + "is in your inventory");
            }
        }
        else
        {
            inventoryItem = null;
            Debug.Log(inventoryItem + "is not in your inventory");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            if (inventoryEnabled)
            {
                Cursor.lockState = CursorLockMode.None;
                playerController.canMove = false;
                cameraController.canMove = false;
            }
            else
            {
                playerController.canMove = true;
                cameraController.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            inventory.SetActive(inventoryEnabled);
        }
    }
    public void AddItem(Items item)
    {
        if (!_items.ContainsKey(item.ID))
        {
            _items.Add(item.ID, item);
            InventoryUpdated?.Invoke();
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
