using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Resolution[] resoluciones;


    GameManager gameManager;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        GetGameData();
    }
    void Start()
    {
        
        ShowMenuPrincipal();
        if (Screen.resolutions != null)
        {
            resoluciones = Screen.resolutions; // obtiene resoluciones disponibles
        }
    }

    private void OnEnable()
    {
        panelConfiguracion = GameObject.Find("Configuracion");
    }


    public void CambiarResolucionPorIndice(int indice)
    {
        if (resoluciones != null) 
        { 
        Resolution res = resoluciones[indice];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
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
            GameObject.Find("ManagerPause").GetComponent<PauseMenu>().CloseConfiguration();
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
        Application.Quit();
    }

    public void OnContinueClicked()
    {
        TransitionPanel.gameObject.SetActive(true);
        TransitionPanel.StartCoroutine(TransitionPanel.Transition(gameManager.loadAct));
          // Cambia por el nombre real de tu escena
    }

    public void OnNewGameClicked()
    {
        TransitionPanel.gameObject.SetActive(true);
        DataPersistenceManager.instance.NewGame();
        TransitionPanel.StartCoroutine(TransitionPanel.Transition(gameManager.loadAct));
    }
    
    void GetGameData()
    {
        if (GameObject.Find("GameManager") && gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

}
