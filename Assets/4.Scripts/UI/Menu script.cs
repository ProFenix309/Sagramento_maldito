using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuControlscript : MonoBehaviour
{
    public static MenuControlscript instance;

    public SceneTransition TransitionPanel;
    public GameObject panelMenuPrincipal;
    public GameObject panelConfiguracion;
    public GameObject subpanelSonido;
    public GameObject subpanelGraficos;
    public GameObject panelCreditos;
    public GameObject panelSonido;
    public GameObject continueButton; // Referencia al botón Continue para habilitarlo/deshabilitarlo

    Resolution[] resoluciones;

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

    void Start()
    {
        ShowMenuPrincipal();

        if (Screen.resolutions != null)
        {
            resoluciones = Screen.resolutions;
        }

        // Verificar si hay datos guardados para habilitar el botón Continue
        CheckSaveData();
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            panelConfiguracion = GameObject.Find("Configuracion");
        }
    }

    private void CheckSaveData()
    {
        // Verificar si existe el archivo de guardado
        string savePath = Application.persistentDataPath + "/GameData.json";
        bool hasSaveFile = File.Exists(savePath);

        // También verificar si el GameManager tiene datos cargados
        bool hasGameData = false;
        if (GameManager.instance != null)
        {
            hasGameData = GameManager.instance.HasSaveData();
        }

        // Habilitar/deshabilitar el botón Continue
        if (continueButton != null)
        {
            continueButton.SetActive(hasSaveFile || hasGameData);
        }

        Debug.Log($"Save data found: {hasSaveFile || hasGameData}");
    }

    public void CambiarResolucionPorIndice(int indice)
    {
        if (resoluciones != null && indice >= 0 && indice < resoluciones.Length)
        {
            Resolution res = resoluciones[indice];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);

            // Guardar la resolución en PlayerPrefs
            PlayerPrefs.SetInt("ResolutionWidth", res.width);
            PlayerPrefs.SetInt("ResolutionHeight", res.height);
            PlayerPrefs.SetInt("ResolutionIndex", indice);
            PlayerPrefs.Save();
        }
    }

    public void ShowMenuPrincipal()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            panelMenuPrincipal.SetActive(true);
            panelConfiguracion.GetComponent<CanvasGroup>().alpha = 0;
            panelConfiguracion.GetComponent<CanvasGroup>().interactable = false;
            panelConfiguracion.GetComponent<CanvasGroup>().blocksRaycasts = false;
            panelCreditos.SetActive(false);
        }
        else
        {
            GameObject pauseManager = GameObject.Find("ManagerPause");
            if (pauseManager != null)
            {
                PauseMenu pauseMenu = pauseManager.GetComponent<PauseMenu>();
                if (pauseMenu != null)
                {
                    pauseMenu.CloseConfiguration();
                }
            }
        }
    }

    public void ShowConfiguracion()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.GetComponent<CanvasGroup>().alpha = 1;
        panelConfiguracion.GetComponent<CanvasGroup>().interactable = true;
        panelConfiguracion.GetComponent<CanvasGroup>().blocksRaycasts = true;
        panelCreditos.SetActive(false);
    }

    public void ShowCreditos()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.GetComponent<CanvasGroup>().alpha = 0;
        panelConfiguracion.GetComponent<CanvasGroup>().interactable = false;
        panelConfiguracion.GetComponent<CanvasGroup>().blocksRaycasts = false;
        panelCreditos.SetActive(true);
    }

    public void Salir()
    {
        // Guardar datos antes de salir
        if (DataPersistenceManager.instance != null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            DataPersistenceManager.instance.SaveGameData();
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnContinueClicked()
    {
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("DataPersistenceManager not found!");
            return;
        }

        // Cargar los datos guardados
        DataPersistenceManager.instance.LoadGameData();

        // Obtener el acto guardado del GameManager
        int actToLoad = 1;
        if (GameManager.instance != null)
        {
            actToLoad = GameManager.instance.loadAct;
        }

        // Asegurar que el acto sea válido
        if (actToLoad < 1) actToLoad = 1;
        if (actToLoad > 3) actToLoad = 3;

        Debug.Log($"Continuing game at Act {actToLoad}");

        // Iniciar transición
        if (TransitionPanel != null)
        {
            TransitionPanel.gameObject.SetActive(true);
            TransitionPanel.StartCoroutine(TransitionPanel.Transition(actToLoad));
        }
        else
        {
            SceneManager.LoadScene(actToLoad);
        }
    }

    public void OnNewGameClicked()
    {
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("DataPersistenceManager not found!");
            return;
        }

        // Crear nuevo juego
        DataPersistenceManager.instance.NewGame();

        Debug.Log("Starting new game at Act 1");

        // Iniciar transición al primer acto
        if (TransitionPanel != null)
        {
            TransitionPanel.gameObject.SetActive(true);
            TransitionPanel.StartCoroutine(TransitionPanel.Transition(1));
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadGraphicsSettings()
    {
        // Cargar fullscreen
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen") == 1;
        }

        // Cargar resolución
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            int width = PlayerPrefs.GetInt("ResolutionWidth");
            int height = PlayerPrefs.GetInt("ResolutionHeight");
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}