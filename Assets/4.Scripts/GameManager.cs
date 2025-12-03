using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public Inventory inventory;
    PlayerController_Original playerController;
    PlayerMovement playerMovement;
    Camera_FPS_Controller cameraController;

    [HideInInspector]public int loadAct =1;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        GameEvents.Worldloaded?.Invoke(new WorldData());
    }
    private void Update()
    {

        GetGameInfo();
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
    }

    public void GetGameInfo()
    {
        if (GameObject.Find("Player_Original") && inventory == null)
        {
            Debug.Log("waos");
            inventory = GameObject.Find("Player_Original").GetComponent<Inventory>();
        }
        else
        {
            return;
        }
    }
}
