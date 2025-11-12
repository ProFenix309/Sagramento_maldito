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
    Resolution[] resoluciones;
    void Start()
    {
        ShowMenuPrincipal();
        resoluciones = Screen.resolutions; // obtiene resoluciones disponibles
    }

    public void CambiarResolucionPorIndice(int indice)
    {
        Resolution res = resoluciones[indice];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    public void Jugar()
    {
        SceneManager.LoadScene(1);  // Cambia por el nombre real de tu escena
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

    public void OnNewGameClicked()
    {
        TransitionPanel.LoadScene("Casa");
        DataPersistenceManager.instance.NewGame();
    }


}
