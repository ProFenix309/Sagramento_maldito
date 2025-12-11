using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;



public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public GameObject playerPrefab;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Health health;

    public List<int> itemsInInv = new List<int>();
    [SerializeField] public List<GameObject> items;

    public Vector3 playerPosition1 = new Vector3(2.16f, 3.05f, 3.95f);
    public Vector3 playerPosition2 = new Vector3(0.1f, 5.81f, 6.92f);
    public Vector3 playerPosition3 = new Vector3(2.16f, 3.05f, 3.95f);

    private Vector3 currentPlayerPosition;
    [HideInInspector] public int loadAct = 1;
    private bool hasSpawnedPlayer = false;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedHandler;
        GameEvents.GameDataLoaded += LoadData;
        GameEvents.GameDataSaved += SaveData;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedHandler;
        GameEvents.GameDataLoaded -= LoadData;
        GameEvents.GameDataSaved -= SaveData;
    }

    private void OnSceneLoadedHandler(Scene scene, LoadSceneMode mode)
    {
        hasSpawnedPlayer = false;

        // Solo spawnear en escenas de juego (no en menú)
        if (scene.buildIndex > 0)
        {
            StartCoroutine(SpawnPlayerDelayed());
        }
    }

    private IEnumerator SpawnPlayerDelayed()
    {
        // Esperar un frame para que la escena esté completamente cargada
        yield return null;

        SpawnPlayer();

        // Esperar otro frame antes de obtener referencias
        yield return null;

        UpdatePlayerReferences();
    }

    public void SpawnPlayer()
    {
        if (hasSpawnedPlayer || SceneManager.GetActiveScene().buildIndex == 0)
            return;

        // Verificar si ya existe un jugador
        GameObject existingPlayer = GameObject.Find("Player_Original(Clone)");
        if (existingPlayer != null)
        {
            player = existingPlayer;
            hasSpawnedPlayer = true;
            return;
        }

        Vector3 spawnPosition = GetSpawnPositionForCurrentScene();

        // Usar la posición guardada si existe y estamos en el acto correcto
        if (currentPlayerPosition != Vector3.zero && SceneManager.GetActiveScene().buildIndex == loadAct)
        {
            spawnPosition = currentPlayerPosition;
        }

        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.name = "Player_Original(Clone)";
        hasSpawnedPlayer = true;

        Debug.Log($"Player spawned in scene {SceneManager.GetActiveScene().name} at position {spawnPosition}");
    }

    private Vector3 GetSpawnPositionForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 1:
                return playerPosition1;
            case 2:
                return playerPosition2;
            case 3:
                return playerPosition3;
            default:
                return playerPosition1;
        }
    }

    private void UpdatePlayerReferences()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        GameObject playerObj = GameObject.Find("Player_Original(Clone)");

        if (playerObj != null)
        {
            player = playerObj;
            inventory = player.GetComponent<Inventory>();
            health = player.GetComponent<Health>();

            Debug.Log("Player references updated successfully");

            // Restaurar items si es necesario
            if (itemsInInv != null && itemsInInv.Count > 0 && inventory != null)
            {
                StartCoroutine(RestoreItemsDelayed());
            }
        }
        else
        {
            Debug.LogWarning("Could not find player object to update references");
        }
    }

    private IEnumerator RestoreItemsDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        ObtainItems();
    }

    public void LoadData(GameData data)
    {
        if (data == null)
        {
            Debug.LogWarning("LoadData called with null data");
            return;
        }

        loadAct = data.SavedWorldData.SavedAct;
        currentPlayerPosition = data.SavedPlayerData.PlayerPosition;

        if (data.SavedPlayerData.Items != null)
        {
            itemsInInv = new List<int>(data.SavedPlayerData.Items);
        }

        Debug.Log($"GameManager - Data loaded: Act {loadAct}, Position {currentPlayerPosition}, Items: {itemsInInv.Count}");
    }

    public void SaveData(GameData data)
    {
        if (data == null)
        {
            Debug.LogWarning("SaveData called with null data");
            return;
        }

        // Actualizar posición del jugador si existe
        if (player != null)
        {
            currentPlayerPosition = player.transform.position;
        }

        // Actualizar items del inventario
        UpdateItemsList();

        // Guardar en GameData
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        data.SavedWorldData.SavedAct = currentScene > 0 ? currentScene : loadAct;
        data.SavedPlayerData.PlayerPosition = currentPlayerPosition;
        data.SavedPlayerData.Items = new List<int>(itemsInInv);

        Debug.Log($"GameManager - Data saved: Act {data.SavedWorldData.SavedAct}, Position {currentPlayerPosition}, Items: {itemsInInv.Count}");
    }

    private void UpdateItemsList()
    {
        if (inventory == null || inventory.Items == null)
            return;

        // Limpiar lista y reconstruir desde el inventario actual
        itemsInInv.Clear();

        foreach (var item in inventory.Items)
        {
            if (!itemsInInv.Contains(item.Value.ID))
            {
                itemsInInv.Add(item.Value.ID);
            }
        }
    }

    public void ObtainItems()
    {
        if (inventory == null || itemsInInv == null || itemsInInv.Count == 0)
        {
            Debug.Log("No items to restore or inventory not ready");
            return;
        }

        foreach (int itemId in itemsInInv)
        {
            // Verificar si ya tiene el item
            if (inventory.Items != null && inventory.Items.ContainsKey(itemId))
            {
                continue;
            }

            // Buscar el item en la lista de items disponibles
            foreach (GameObject itemObj in items)
            {
                Items itemComponent = itemObj.GetComponent<Items>();
                if (itemComponent != null && itemComponent.ID == itemId)
                {
                    inventory.AddItem(itemComponent);
                    Debug.Log($"Item {itemId} restored to inventory");
                    break;
                }
            }
        }
    }

    public void LoadAct(int actNumber)
    {
        if (actNumber > 0 && actNumber <= 3)
        {
            SceneManager.LoadScene(actNumber);
        }
    }

    public bool HasSaveData()
    {
        return loadAct > 1 || currentPlayerPosition != Vector3.zero || itemsInInv.Count > 0;
    }
}