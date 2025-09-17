using UnityEngine;

public class Items : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;

    [HideInInspector]
    public bool pickedUp;

    [HideInInspector]
    public bool equipped;

    [HideInInspector]
    public GameObject bulletManager;

    [HideInInspector]
    public GameObject bullet;

    public bool playersBullets;

    private void Start()
    {
        bulletManager = GameObject.FindWithTag("Bullet Manager");

        if (!playersBullets)
        {
            /*int allBullet = bulletManager.transform.childCount;

            for (int i = 0; i < allBullet; i++)
            {
                if (bulletManager.transform.GetChild(i).gameObject.GetComponent<Items>().ID == ID)
                {
                    bullet = bulletManager.transform.GetChild(i).gameObject;
                }
        }*/
            }
    }

    private void Update()
    {
        if (equipped)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                equipped = false;
            }
            if (equipped == false)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ItemUsage()
    {
        if (type == "Bullet")
        {
            bullet.SetActive(true);
            equipped = true;
            bullet.GetComponent<Items>().equipped = true;
        }
    }
}
