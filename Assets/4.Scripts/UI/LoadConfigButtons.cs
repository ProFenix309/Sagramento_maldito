using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadConfigButtons : MonoBehaviour
{
    public Button continueGame;
    public Button openConfig;
    public Button closeConfig;
    public Button Mainmenu;

    public PauseMenu pauseMenu;

    // Update is called once per frame



  
   
    void Update()
    {
        pauseMenu = GameObject.Find("ManagerPause").GetComponent<PauseMenu>();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {


            if (continueGame == null || openConfig == null || closeConfig == null || Mainmenu == null)
            {
                continueGame = GameObject.Find("Button Continuar Partida").GetComponent<Button>();
                openConfig = GameObject.Find("Button Configuracion").GetComponent<Button>();
                closeConfig.onClick.RemoveAllListeners();
                closeConfig = GameObject.Find("Volver").GetComponent<Button>();
                Mainmenu = GameObject.Find("Button Volver al menu principal").GetComponent<Button>();
            }


                continueGame.onClick.AddListener(pauseMenu.ResumeGame);
            
         
                openConfig.onClick.AddListener(pauseMenu.OpenConfiguration);
         
                closeConfig.onClick.AddListener(pauseMenu.CloseConfiguration);
          
           
                Mainmenu.onClick.AddListener(pauseMenu.ReturnPrincipalMenu);
            
        }
    }
}
