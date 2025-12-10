using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-10)]
public class DataPersistenceManager : MonoBehaviour
{
    string path;
    [SerializeField] private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    public float saveTime;

    int load = 1;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
        GameEvents.Worldloaded += gameData.SetWorldData;
        GameEvents.EnemyLoaded += gameData.AddEnemyData;
        GameEvents.PlayerLoaded += gameData.SetPlayerData;
    }
    private void OnDisable()
    { 
        GameEvents.Worldloaded -= gameData.SetWorldData;
        GameEvents.EnemyLoaded -= gameData.AddEnemyData;
        GameEvents.PlayerLoaded -= gameData.SetPlayerData;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        path = Application.persistentDataPath + "/GameData.txt";
        Debug.Log(path);
    }

    private void Start()
    { 

        Debug.LogWarning(path);
        CheckGameData();
    }

    private void Update()
    {
     if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(SaveData(saveTime));
        }
        
    }
    public void NewGame()
    {
        gameData = new GameData();
        SaveGameData();
    }
    public void CheckGameData()
    {
        if (!File.Exists(path))
        {
            StreamWriter file = File.CreateText(path);
            file.Close();
        }
        else
        {
            LoadGameData();
        }
    }
    public void LoadGameData()
    {
        Debug.LogWarning("loading data");

        string json = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(json);
        GameEvents.GameDataLoaded?.Invoke(gameData);
    }

    public void SaveGameData()
    {
        Debug.LogWarning("SavingData");
         string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, json);
    }
    private void OnApplicationQuit()
    {

        SaveGameData();
    }    

    private void OnSceneLoaded(Scene scene, Scene newScene)
    {
        load = 1;
        SaveGameData();
        LoadGameData();

        
    }

    IEnumerator SaveData(float timeBetweenSaves)
    {
        if (!File.Exists(path))
        { }
        if (load == 1)
        {
            LoadGameData();
            load = 0;
        }
    

        yield return new WaitForSeconds(timeBetweenSaves);

        SaveGameData();
        
    }


}



