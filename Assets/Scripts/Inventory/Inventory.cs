using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]private bool inventoryEnabled;

    public GameObject inventory;

    private int allSlots, enbaledSlot;

    private GameObject[] slot;

    public GameObject slotHalder;

    void Start()
    {
        allSlots = slotHalder.transform.childCount;

        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHalder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            inventory.SetActive(inventoryEnabled);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;

            Items items = itemPickedUp.GetComponent<Items>();

            AddItem(itemPickedUp, items.ID, items.type, items.description, items.icon);
        }
    }
    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Items>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();

                slot[i].GetComponent<Slot>().empty = false;
            }
            return;
        }
    }
}
