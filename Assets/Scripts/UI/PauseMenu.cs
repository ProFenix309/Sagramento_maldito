using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PauseMenu : MonoBehaviour
{
    // Asigna este objeto de UI desde el Inspector (PanelPausa)
    public GameObject pausePanel;

    // Bandera para saber si el juego está pausado
    private bool gamePause = false;

    // Nombre de la escena del menú principal (ej: "MenuPrincipal")
    public string namePrincipalMenu = "PrincipalMenu";

    // En tu script ControladorPausa:
    public GameObject configutionPanel; // Asigna este panel en el Inspector

    void Update()
    {
        // Detecta si el jugador presiona la tecla Escape (o la que definas)
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Muestra el panel del menú de pausa
        pausePanel.SetActive(true);

        // Detiene el tiempo en el juego (escalado a 0)
        Time.timeScale = 0f;

        // Actualiza el estado
        gamePause = true;
    }

    // --- Funciones que se asignan a los botones ---

    public void ResumeGame()
    {
        // Oculta el panel del menú de pausa
        pausePanel.SetActive(false);

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
        pausePanel.SetActive(false); // Oculta el menú de pausa
        configutionPanel.SetActive(true); // Muestra el panel de configuración
    }

    public void CloseConfiguration()
    {
        configutionPanel.SetActive(false); // Oculta el panel de configuración
        pausePanel.SetActive(true); // Muestra de nuevo el menú de pausa
    }
}