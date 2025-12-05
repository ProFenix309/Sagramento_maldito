using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public Inventory inventory;
    public GameObject player;


    PlayerController_Original playerController;
    PlayerMovement playerMovement;
    Camera_FPS_Controller cameraController;

    public float itemAmount;
    public int[] itemsInInv;

    [SerializeField] public List<GameObject> items;
    [SerializeField] public List<Items> itemsOut;
   




    public Vector3 playerPosition;
    [HideInInspector]public int loadAct =1;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
 
        GameEvents.PlayerLoaded?.Invoke(new PlayerData(player.transform.position));
        GameEvents.Worldloaded?.Invoke(new WorldData());
       // Debug.Log(GameObject.Find("Content Panel").transform.childCount);

    }
    private void Update()
    {
        GetGameInfo();
        StartCoroutine(SpawnPlayer());
       
    }


    IEnumerator SpawnPlayer()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && GameObject.Find("Player_Original(Clone)") == null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                Debug.LogWarning("more than 1 players in scene");
                player.transform.position = playerPosition;
                Instantiate(player);
               
            }


            if (SceneManager.GetActiveScene().buildIndex == 2 && GameObject.Find("Player_Original(Clone)") == null)
            {
                player.transform.position = new Vector3 (0.1f, 5.81f, 6.92f);
                Instantiate(player);
               
            }
        }
        yield return new WaitForSeconds(3);
    }



    private void OnEnable()
    {
        GameEvents.GameDataLoaded += LoadData;
    }
    private void OnDisable()
    {
        GameEvents.GameDataLoaded -= LoadData;
    }
    public void LoadData(GameData data)
    {
       loadAct = data.SavedWorldData.SavedAct;
       playerPosition = data.SavedPlayerData.PlayerPosition;
       ObtainItems();
       
    }

    public void GetGameInfo()
    {
        
        if (GameObject.Find("Player_Original") && inventory == null)
        {
            Debug.Log("waos");
            inventory = GameObject.Find("Player_Original").GetComponent<Inventory>();
            player = GameObject.Find("Player_Original");
            player.transform.position = playerPosition;
            itemAmount = inventory.initialItems.Count -1;
            GetItems();
            
        }
        else
        {
            return;
        }
    }
    public void GetItems()
    {
        foreach (var item in inventory.initialItems) 
        {
            for (int i = 1; i < inventory.initialItems.Count; i++)
            {
                foreach (var waos in itemsInInv)
                {
                    for (int j = 0; j < itemsInInv.Length; j++)
                    {
                        if (itemsInInv[j] != item.ID)
                        {
                            itemsInInv[i] = item.ID;
                        }
                    }
                }    
            }
        }
    }
    public void ObtainItems()
    {
        foreach(var item in itemsInInv)
        {
         foreach(var iItem in items)
            {
                Items currentItem = iItem.GetComponent<Items>();
                if (item == currentItem.ID)
                {
                    inventory.AddItem(currentItem);
                }
            }
           
        }
        
       //inventory.AddItem(item)
    }
}
