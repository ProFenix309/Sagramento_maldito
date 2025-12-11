using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-10)]
public class DataPersistenceManager : MonoBehaviour
{
    string path;
    [SerializeField] private GameData gameData;
    GameData newGameData = new GameData();
    public static DataPersistenceManager instance { get; private set; }

    public float saveTime = 5f;

    private Coroutine autoSaveCoroutine;
    private bool hasLoadedOnce = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/GameData.json";
        Debug.Log("Save path: " + path);
    }

    private void Start()
    {
        CheckGameData();
    }

    public void NewGame()
    {
        gameData = new GameData();
        SaveGameData();
        Debug.Log("New game created");
    }

    public void CheckGameData()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file found, creating new game data");
            NewGame();
        }
        else
        {
            LoadGameData();
        }
    }

    public void LoadGameData()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file to load");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Save file is empty, creating new game");
                NewGame();
                return;
            }

            gameData = JsonUtility.FromJson<GameData>(json);

            if (gameData == null)
            {
                Debug.LogWarning("Failed to parse save data, creating new game");
                NewGame();
                return;
            }

            Debug.Log("Game data loaded successfully");
            GameEvents.GameDataLoaded?.Invoke(gameData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading game data: " + e.Message);
            NewGame();
        }
    }

    public void SaveGameData()
    {
        if (gameData == null)
        {
            Debug.LogWarning("No game data to save");
            return;
        }

        try
        {
            // Invocar evento para que otros sistemas guarden sus datos
            GameEvents.GameDataSaved?.Invoke(gameData);

            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(path, json);
            Debug.Log("Game data saved successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game data: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SaveGameData();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Detener autoguardado anterior si existe
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = null;
        }

        // Solo procesar si no estamos en el menú principal
        if (scene.buildIndex == 0)
        {
            hasLoadedOnce = false;
            Debug.Log("Main menu loaded - autosave disabled");
            return;
        }

        Debug.Log($"Scene loaded: {scene.name} (Index: {scene.buildIndex})");

        // Guardar antes de cargar nueva escena para no perder progreso
        if (hasLoadedOnce && gameData != null)
        {
            Debug.Log("Saving data before loading new scene data");
            SaveGameData();
        }

        // Cargar datos para esta escena
        LoadGameData();
        hasLoadedOnce = true;

        // Iniciar autoguardado solo en escenas de juego
        if (scene.buildIndex > 0)
        {
            autoSaveCoroutine = StartCoroutine(AutoSaveCoroutine());
            Debug.Log($"Autosave started - interval: {saveTime} seconds");
        }
    }

    IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveTime);

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene > 0)
            {
                Debug.Log($"Autosaving... (Scene: {currentScene})");
                SaveGameData();
            }
        }
    }

    public GameData GetGameData()
    {
        return gameData;
    }
}
