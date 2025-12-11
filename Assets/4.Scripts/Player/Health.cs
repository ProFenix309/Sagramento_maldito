using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour, IDataPersistence
{
    [Header("Configuracion de Vida")]
    public int vidaMaxima;
    [SerializeField] public float vidaActual;

    [Header("Deteccion de enemigo")]
    public string etiquetaEnemigo = "Enemy";
    
    public float tiempoMuerte;
    Animator animatior;

    [Header("Panel de Muerte")]
    [SerializeField] private GameObject panelMuerte;

    private PlayerController_Original playerController;
    private Camera_FPS_Controller cameraController;

    private void Awake()
    {
        animatior = GameObject.Find("Altered State").GetComponent<Animator>();
        vidaActual = vidaMaxima;
        
        // Obtener referencias a los controladores
        playerController = GetComponent<PlayerController_Original>();
        cameraController = GetComponentInChildren<Camera_FPS_Controller>();
        
        // Asegurarse de que el panel de muerte esté desactivado al inicio
        if (panelMuerte != null)
        {
            panelMuerte.SetActive(false);
        }
    }

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    private void Update()
    {
        if (vidaActual < vidaMaxima)
        {
            //regeneration
            vidaActual += Time.deltaTime * 1.4f;
        }
        else
        {
            vidaActual = vidaMaxima;
        }
    }

    public void RecibirDaño(float daño)
    {
        vidaActual -= daño;
        Debug.Log("Daño recibido: " + daño + " | Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            StartCoroutine(Morir(tiempoMuerte));
        }
        else
        {
            StartEffect();
        }
    }

    IEnumerator Morir(float time)
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // Detener el movimiento del jugador
        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.stop = false;
        }

        // Detener el movimiento de la cámara
        if (cameraController != null)
        {
            cameraController.canMove = false;
        }

        // Desbloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Esperar el tiempo de muerte
        yield return new WaitForSeconds(time);

        // Activar el panel de muerte
        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true);
        }
    }

    // Función para el botón de reiniciar nivel
    public void ReiniciarNivel()
    {
        // Bloquear el cursor nuevamente
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Recargar la escena actual
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }

    // Función para el botón de volver al menú principal
    public void VolverAlMenu()
    {
        // Desbloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargar la escena del menú principal (asume que es la escena 0)
        // Si tu menú principal tiene otro índice, cámbialo aquí
        SceneManager.LoadScene(0);
        
        // O si conoces el nombre de la escena del menú:
        // SceneManager.LoadScene("MenuPrincipal");
    }

    public void StartEffect()
    {
        animatior.SetBool("StartedEffect", true);
    }

    private void OnEnable()
    {
        GameEvents.GameDataLoaded += LoadData;
    }

    private void OnDisable()
    {
        GameEvents.GameDataLoaded -= LoadData;
    }

    public void LoadData(GameData data)
    {
        vidaActual = data.SavedPlayerData.CurrentHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            RecibirDaño(19);
        }
    }
}