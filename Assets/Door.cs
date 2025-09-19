using UnityEngine;

public class Door : MonoBehaviour, Interactable, ItemRequierement
{
    public string ItemID { get => _itemID; }
    private string _itemID;

    public void Interact()
    {
        Debug.Log("Usando puerta");
    }
}
