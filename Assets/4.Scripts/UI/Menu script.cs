using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlscript : MonoBehaviour
{
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
        GetGameData();
    }
    void Start()
    {
        TransitionPanel.gameObject.SetActive(false);
        ShowMenuPrincipal();
        resoluciones = Screen.resolutions; // obtiene resoluciones disponibles
    }

    public void CambiarResolucionPorIndice(int indice)
    {
        Resolution res = resoluciones[indice];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void ShowMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelConfiguracion.SetActive(false);
        panelCreditos.SetActive(false);
    }

    public void ShowConfiguracion()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.SetActive(true);
        panelCreditos.SetActive(false);


    }


    public void ShowCreditos()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguracion.SetActive(false);
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
