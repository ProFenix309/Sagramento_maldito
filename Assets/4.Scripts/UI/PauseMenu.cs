using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public int sceneManager;

    // Asigna este objeto de UI desde el Inspector (PanelPausa)
    public GameObject pausePanel;
    public GameObject menuPanel;
    public GameObject configurationPanel;

    private Inventory inventory;
    private PlayerController_Original playerController;
    private Camera_FPS_Controller cameraController;

    // Bandera para saber si el juego está pausado
    private bool gamePause = false;

    // Nombre de la escena del menú principal (ej: "MenuPrincipal")
    public string namePrincipalMenu = "PrincipalMenu";

    private bool referencesFound = false;

    private void Awake()
    {
        sceneManager = SceneManager.GetActiveScene().buildIndex;

        if (sceneManager != 0)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reiniciar referencias cuando cambia la escena
        referencesFound = false;
        inventory = null;
        playerController = null;
        cameraController = null;
        configurationPanel = null;
        pausePanel = null;
        menuPanel = null;

        sceneManager = scene.buildIndex;

        if (sceneManager != 0)
        {
            // Intentar encontrar referencias después de un pequeño delay
            Invoke(nameof(FindReferences), 0.5f);
        }
    }

    void Update()
    {
        if (sceneManager == 0)
            return;

        // Intentar encontrar referencias si no se han encontrado
        if (!referencesFound)
        {
            FindReferences();
        }

        // Solo permitir pausar si todas las referencias están encontradas
        if (referencesFound && Input.GetKeyDown(KeyCode.Q))
        {
            if (gamePause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void FindReferences()
    {
        // Buscar jugador
        GameObject playerObj = GameObject.Find("Player_Original(Clone)");
        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<PlayerController_Original>();
            inventory = playerObj.GetComponent<Inventory>();
        }

        // Buscar cámara
        GameObject cameraObj = GameObject.Find("Main Camera");
        if (cameraObj != null)
        {
            cameraController = cameraObj.GetComponent<Camera_FPS_Controller>();
        }

        // Buscar paneles UI
        GameObject configObj = GameObject.Find("Configuracion");
        if (configObj != null)
        {
            configurationPanel = configObj;
        }

        GameObject pauseObj = GameObject.Find("Pause Menu");
        if (pauseObj != null)
        {
            pausePanel = pauseObj;
        }

        GameObject menuObj = GameObject.Find("Menu Pausa");
        if (menuObj != null)
        {
            menuPanel = menuObj;
        }

        // Verificar si todas las referencias fueron encontradas
        if (playerController != null && inventory != null && cameraController != null &&
            configurationPanel != null && pausePanel != null && menuPanel != null)
        {
            referencesFound = true;
            Debug.Log("PauseMenu: All references found successfully");
        }
    }

    public void PauseGame()
    {
        if (!referencesFound)
        {
            Debug.LogWarning("Cannot pause: References not found yet");
            return;
        }

        // Deshabilitar inputs del jugador
        if (inventory != null)
            inventory.UnlockInputs = false;

        if (playerController != null)
            playerController.stop = false;

        if (cameraController != null)
            cameraController.unlockInputs = false;

        // Desbloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Mostrar el panel del menú de pausa
        if (pausePanel != null)
        {
            pausePanel.GetComponent<CanvasGroup>().alpha = 1;
            pausePanel.GetComponent<CanvasGroup>().interactable = true;
            pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        // Detener el tiempo en el juego
        Time.timeScale = 0f;

        // Actualizar el estado
        gamePause = true;
    }

    public void ResumeGame()
    {
        if (!referencesFound)
        {
            Debug.LogWarning("Cannot resume: References not found yet");
            return;
        }

        // Habilitar inputs del jugador
        if (inventory != null)
            inventory.UnlockInputs = true;

        if (playerController != null)
            playerController.stop = true;

        if (cameraController != null)
            cameraController.unlockInputs = true;

        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ocultar el panel del menú de pausa
        if (pausePanel != null)
        {
            pausePanel.GetComponent<CanvasGroup>().alpha = 0;
            pausePanel.GetComponent<CanvasGroup>().interactable = false;
            pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        // Restaurar el tiempo normal
        Time.timeScale = 1f;

        // Actualizar el estado
        gamePause = false;

        // Cerrar configuración si está abierta
        CloseConfiguration();
    }

    public void ReturnPrincipalMenu()
    {
        // Guardar datos antes de volver al menú
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
            Debug.Log("Game saved before returning to main menu");
        }

        // Restaurar el tiempo
        Time.timeScale = 1f;

        // Actualizar estado de pausa
        gamePause = false;

        // Desbloquear cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargar la escena del menú principal
        SceneManager.LoadScene(0);
    }

    public void OpenConfiguration()
    {
        if (menuPanel == null || configurationPanel == null)
        {
            Debug.LogWarning("Cannot open configuration: Panels not found");
            return;
        }

        // Ocultar el menú de pausa
        menuPanel.GetComponent<CanvasGroup>().alpha = 0;
        menuPanel.GetComponent<CanvasGroup>().interactable = false;
        menuPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        // Mostrar el panel de configuración
        configurationPanel.GetComponent<CanvasGroup>().alpha = 1;
        configurationPanel.GetComponent<CanvasGroup>().interactable = true;
        configurationPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseConfiguration()
    {
        if (menuPanel == null || configurationPanel == null)
        {
            Debug.LogWarning("Cannot close configuration: Panels not found");
            return;
        }

        // Ocultar el panel de configuración
        configurationPanel.GetComponent<CanvasGroup>().alpha = 0;
        configurationPanel.GetComponent<CanvasGroup>().interactable = false;
        configurationPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        // Mostrar de nuevo el menú de pausa
        menuPanel.GetComponent<CanvasGroup>().alpha = 1;
        menuPanel.GetComponent<CanvasGroup>().interactable = true;
        menuPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public bool IsPaused()
    {
        return gamePause;
    }
}