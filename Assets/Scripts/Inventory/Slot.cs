using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Action<Items> SlotClicked;
    Items currentItem;

    public Image icon;

    public void UpdateSlot(Items item)
    {
        currentItem = item;

        if (item != null)
        {
            icon.sprite = item.icon;
        }
        else
        {
            icon.sprite = null;
        }
    }
    public void OnPointerClick(PointerEventData pointer)
    {
        if (currentItem != null)
        {
            SlotClicked?.Invoke(currentItem);
        }
    }
}
