using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public Dictionary<int, Items> Items { get => _items; }

    Dictionary<int, Items> _items = new();

    public Action InventoryUpdated;

    [SerializeField] public bool inventoryEnabled = false;

    public GameObject inventory;

    InventoryUIHandler inventoryUIHandler;

    public GameObject slotHalder;

    PlayerController_Original playerController;
    Camera_FPS_Controller cameraController;

    [HideInInspector] public GameObject HandDetection;
    [HideInInspector] public GetItem itemH;
    [HideInInspector] public GameObject inventoryItem;

    bool unlockInputs = true;
    public bool UnlockInputs { get => unlockInputs; set => unlockInputs = value; }

    private void Awake()
    {
        HandDetection = GameObject.Find("Hand");
    }
    void Start()
    {
        inventoryUIHandler = inventory.GetComponent<InventoryUIHandler>();
        itemH = HandDetection.GetComponent<GetItem>();
        playerController = GetComponent<PlayerController_Original>();
        cameraController = GameObject.Find("Main Camera").GetComponent<Camera_FPS_Controller>();
    }

    void Update()
    {
        if (unlockInputs)
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
            else
            {
                inventoryItem = null;
                Debug.Log("no detected item for your inventory");
            }

            if (Input.GetKeyDown(KeyCode.I) && inventoryUIHandler.ItemInfoPanel.activeSelf == false)
            {
                inventoryEnabled = !inventoryEnabled;
                if (inventoryEnabled)
                {
                    if (playerController.canMove)
                    {
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
                inventory.SetActive(inventoryEnabled);
            }
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
