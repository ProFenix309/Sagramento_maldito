using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.Overlays;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public float gameSavingTime;


    public GameObject playerPrefab;

    public GameObject player;
    public Inventory inventory;
    public Health health;


    PlayerController_Original playerController;
    PlayerMovement playerMovement;
    Camera_FPS_Controller cameraController;


    public float itemAmount;
    public List<int> itemsInInv;

    [SerializeField] public List<GameObject> items;
   // [SerializeField] public List<Items> itemsOut;
   




    public Vector3 playerPosition;
    [HideInInspector]public int loadAct =1;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        // player.transform.position, health.vidaMaxima, player,itemsInInv

        GameEvents.PlayerLoaded?.Invoke(new PlayerData(player, itemsInInv,playerPosition));

        GameEvents.Worldloaded?.Invoke(new WorldData());
        // Debug.Log(GameObject.Find("Content Panel").transform.childCount);
    }


    private void Update()
    {
        SpawnPlayer();
        StartCoroutine(GameInfo(gameSavingTime));

    }


    public void SpawnPlayer()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && GameObject.Find("Player_Original(Clone)") == null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameEvents.PlayerLoaded?.Invoke(new PlayerData(player, itemsInInv,playerPosition));
                Debug.LogWarning("more than 1 players in scene");
                Instantiate(playerPrefab);
                playerPrefab.transform.position = playerPosition;
            
               
            }


            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameEvents.PlayerLoaded?.Invoke(new PlayerData(player, itemsInInv, playerPosition));
                Instantiate(playerPrefab);
                playerPrefab.transform.position = new Vector3 (0.1f, 5.81f, 6.92f);
                
               
            }
        }
    
    }



    private void OnEnable()
    {
        GameEvents.GameDataLoaded += LoadData;
        Debug.Log("guardandoinfo");
        GameEvents.GameDataSaved += SaveData;
    }
    private void OnDisable()
    {
        GameEvents.GameDataLoaded -= LoadData;
         GameEvents.GameDataSaved -= SaveData;

    }
    public void LoadData(GameData data)
    {
       loadAct = data.SavedWorldData.SavedAct;
     
        
        SpawnPlayer();
        

       playerPosition = data.SavedPlayerData.PlayerPosition;
        if (player == null)
        {
            player = data.SavedPlayerData.Player;
        }


        itemsInInv = data.SavedPlayerData.Items;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            ObtainItems();
        }

    }

    public void SaveData(GameData data)
    {
        data.SavedWorldData.SavedAct = loadAct;
        if (data.SavedPlayerData.Player != null)
        {
            data.SavedPlayerData.Player = player;
        }
        data.SavedPlayerData.PlayerPosition = playerPosition;
        data.SavedPlayerData.Items  = itemsInInv ;
    }
        


    IEnumerator GameInfo(float savingTime)
    {
        yield return new WaitForSeconds(savingTime);
        GetGameInfo();
        GameEvents.GameDataSaved += SaveData;
    }

    public void GetGameInfo()
    {

        // if (GameObject.Find("Player_Original") && inventory == null)
        //  {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        { 
            Debug.LogWarning("waos");
        inventory = GameObject.Find("Player_Original(Clone)").GetComponent<Inventory>();
        player = GameObject.Find("Player_Original(Clone)");
        playerPosition = player.transform.position;
        health = player.GetComponent<Health>();
        itemAmount = inventory.initialItems.Count - 1;

        GetItems();
         }
        else
        {
            return;
        }
    }
    public void GetItems()
    {
        if (inventory.Items != null)
        {
            if (inventory.Items.Count > 0 && inventory.Items != null)
            {

                foreach (var item in inventory.Items)
                {
                  //  Debug.Log(item.Value.name + " se detectó en el inv");

                    if (!itemsInInv.Contains(item.Value.ID))
                    {
                        itemsInInv.Add(item.Value.ID);
                    }
                    for (int i = 0;i < itemsInInv.Count; i++)
                    {
                        if (!inventory.Items.ContainsKey(itemsInInv[i]))
                        {
                            itemsInInv.Remove(itemsInInv[i]);
                        }
                    }
 
                }
                for (int i = 0; i < itemsInInv.Count; i++)
                {
                    Debug.Log(itemsInInv[i]);
                }
            }
        }
        else
        {
            return;
        }

        
    }
    public void ObtainItems()
    {
        if (itemsInInv != null)
        {
            foreach (var item in itemsInInv)
            {
                foreach (var iItem in items)
                {
                    Items currentItem = iItem.GetComponent<Items>();
                    if (item == currentItem.ID)
                    {
                        inventory.AddItem(currentItem);
                    }
                }

            }
        }
        
       //inventory.AddItem(item)
    }
}
