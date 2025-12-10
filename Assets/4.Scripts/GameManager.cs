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
    public Vector3 playerPosition1 = new Vector3(2.16f, 3.05f, 3.95f);
    public Vector3 playerPosition2 = new Vector3(0.1f, 5.81f, 6.92f);
    public Vector3 playerPosition3 = new Vector3();//toca mirar punto de spawneo
    [HideInInspector] public int loadAct;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        // player.transform.position, health.vidaMaxima, player,itemsInInv

        GameEvents.PlayerLoaded?.Invoke(new PlayerData(player,playerPosition1,playerPosition2,playerPosition3));

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
                Debug.Log("se cargó el jugador en la escena 2");
                Instantiate(playerPrefab);
                playerPrefab.transform.position = playerPosition1;
                if (playerPosition == new Vector3())
                {
                    playerPosition = playerPosition1;
                }
                playerPrefab.transform.position = playerPosition;

            
               
            }


            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                Debug.Log("se cargó el jugador en la escena 2");
                Instantiate(playerPrefab);
                playerPrefab.transform.position = playerPosition2;
                if (playerPosition == new Vector3())
                {
                    playerPosition = playerPosition2;
                }
                playerPrefab.transform.position = playerPosition;
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Instantiate(playerPrefab);
                playerPrefab.transform.position = playerPosition3;
                if (playerPosition == new Vector3())
                {
                    playerPosition = playerPosition3;
                }
                playerPrefab.transform.position = playerPosition;

            }
        }
    
    }



    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            loadAct = 1;
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            loadAct = SceneManager.GetActiveScene().buildIndex;
        }
        GameEvents.GameDataLoaded += LoadData;
        Debug.Log("guardandoinfo");
        GameEvents.GameDataSaved += SaveData;
    }
    private void OnDisable()
    {
        GameEvents.GameDataLoaded -= LoadData;

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
            Debug.LogWarning("getting info");
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

        //inventory.AddItem(item)
    }
}
