using UnityEngine;

public class Items : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;

    [HideInInspector]
    public bool pickedUp;
}
