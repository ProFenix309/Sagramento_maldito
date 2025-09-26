using UnityEngine;

public class Door : MonoBehaviour, Interactable, ItemRequierement
{
    public int ItemID { get => _itemID; }
    [SerializeField] private int _itemID;

    public void Interact()
    {
        Debug.Log("Usando puerta");
    }
}
