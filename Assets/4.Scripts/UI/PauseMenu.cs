using System.ComponentModel;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;

    int sceneManager;

    // Asigna este objeto de UI desde el Inspector (PanelPausa)
    public GameObject pausePanel;

    public GameObject menuPanel;

    public Inventory inventory;

    public PlayerController_Original playerController;


    // Bandera para saber si el juego está pausado
    private bool gamePause = false;

    // Nombre de la escena del menú principal (ej: "MenuPrincipal")
    public string namePrincipalMenu = "PrincipalMenu";

    // En tu script ControladorPausa:
    public GameObject configutionPanel; // Asigna este panel en el Inspector

    

    private void Awake()
    {
       
        sceneManager = SceneManager.GetActiveScene().buildIndex;

        if (sceneManager != 0)
        {
            if (instance == null)
            {

                DontDestroyOnLoad(gameObject);

            }
            else if (instance != null) 
            {
                instance = this;
            }

        }
    }

    private void Start()
    {

    }

    void Update()
    {
        if (inventory == null && playerController == null && configutionPanel == null && pausePanel == null && menuPanel == null && GameObject.Find("Player_Original"))
        {
            inventory = GameObject.Find("Player_Original(Clone)").GetComponent<Inventory>();
            playerController = GameObject.Find("Player_Original(Clone)").GetComponent<PlayerController_Original>();
            configutionPanel = GameObject.Find("Configuración").gameObject;
            pausePanel = GameObject.Find("Pause Menu").gameObject;
            menuPanel = GameObject.Find("Menú Pausa").gameObject;
        }
        // Detecta si el jugador presiona la tecla Escape (o la que definas)
        if (Input.GetKeyDown(KeyCode.Q))
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

    public void PauseGame()
    {
        inventory.UnlockInputs = false;
        playerController.stop = false; 

        // Desbloquea el curso
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Muestra el panel del menú de pausa
        pausePanel.GetComponent<CanvasGroup>().alpha = 1;
        pausePanel.GetComponent<CanvasGroup>().interactable = true;
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Detiene el tiempo en el juego (escalado a 0)
        Time.timeScale = 0f;

        // Actualiza el estado
        gamePause = true;
    }

    // --- Funciones que se asignan a los botones ---

    public void ResumeGame()
    {
        inventory.UnlockInputs = true;
        playerController.stop = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Oculta el panel del menú de pausa
        pausePanel.GetComponent<CanvasGroup>().alpha = 0;
        pausePanel.GetComponent<CanvasGroup>().interactable = false;
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        // Restaura el tiempo normal (escalado a 1)
        Time.timeScale = 1f;

        // Actualiza el estado
        gamePause = false;
    }

    public void ReturnPrincipalMenu()
    {
        // **IMPORTANTE:** Primero hay que restaurar el tiempo, 
        // de lo contrario la carga de la escena puede fallar o comportarse mal.
        Time.timeScale = 1f;

        // Carga la escena del menú principal 
        SceneManager.LoadScene(namePrincipalMenu);
    }

    public void OpenConfiguration()
    {
        menuPanel.GetComponent<CanvasGroup>().alpha = 0;
        menuPanel.GetComponent<CanvasGroup>().interactable = false;
        menuPanel.GetComponent<CanvasGroup>().blocksRaycasts = false; // Oculta el menú de pausa

        configutionPanel.GetComponent<CanvasGroup>().alpha = 1;
        configutionPanel.GetComponent<CanvasGroup>().interactable = true;
        configutionPanel.GetComponent<CanvasGroup>().blocksRaycasts = true; // Muestra el panel de configuración
    }

    public void CloseConfiguration()
    {
        configutionPanel.GetComponent<CanvasGroup>().alpha = 0;
        configutionPanel.GetComponent<CanvasGroup>().interactable = false;
        configutionPanel.GetComponent<CanvasGroup>().blocksRaycasts = false; // Oculta el panel de configuración

        menuPanel.GetComponent<CanvasGroup>().alpha = 1;
        menuPanel.GetComponent<CanvasGroup>().interactable = true;
        menuPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;  // Muestra de nuevo el menú de pausa
    }
}