using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour, IDataPersistence
{
    [Header("Configuracion de Vida")]
    public int vidaMaxima;
    [SerializeField] float vidaActual;

    [Header("Deteccion de enemigo")]
    public string etiquetaEnemigo = "Enemy";

    Animator animatior;

    private void Awake()
    {
        animatior = GameObject.Find("Altered State").GetComponent<Animator>();
        vidaActual = vidaMaxima;
        GameEvents.PlayerLoaded?.Invoke(new PlayerData(vidaMaxima));
    }
    void Start()
    {

        vidaActual = vidaMaxima;
        //Debug.Log("Vida inicial: " + vidaActual);

    }
    private void Update()
    {
        if (vidaActual < vidaMaxima)
        {
            //altered state

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
            Morir();
        }
        StartEffect();
    }

    void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // Puedes desactivar, destruir o reiniciar el objeto aqu�:
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);

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