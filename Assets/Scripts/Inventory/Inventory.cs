using System;
using System.Collections.Generic;
using UnityEngine;

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


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        cameraController = GameObject.Find("Main Camera").GetComponent<Camera_FPS_Controller>();
    }

    void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Items item))
        {
            AddItem(item);
            item.gameObject.SetActive(false);
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
