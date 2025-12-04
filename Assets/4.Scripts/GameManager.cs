using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor.Timeline;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public Inventory inventory;
    public GameObject player;


    PlayerController_Original playerController;
    PlayerMovement playerMovement;
    Camera_FPS_Controller cameraController;




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
                Instantiate(player);
                player.transform.position = playerPosition;
            }


            if (SceneManager.GetActiveScene().buildIndex == 2 && GameObject.Find("Player_Original(Clone)") == null)
            {
                Instantiate(player);
                player.transform.position = playerPosition;
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
    }

    public void GetGameInfo()
    {
        
        if (GameObject.Find("Player_Original") && inventory == null)
        {
            Debug.Log("waos");
            inventory = GameObject.Find("Player_Original").GetComponent<Inventory>();
            player = GameObject.Find("Player_Original");
            player.transform.position = playerPosition;

            
        }
        else
        {
            return;
        }
    }
}
