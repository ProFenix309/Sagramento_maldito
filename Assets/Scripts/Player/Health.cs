using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima;
    [SerializeField] float vidaActual;

    [Header("Deteccion de enemigo")]
    public string etiquetaEnemigo = "Enemy";

    void Start()
    {
        vidaActual = vidaMaxima;
        Debug.Log("Vida inicial: " + vidaActual);
    }

    private void Update()
    {
        if (vidaActual < vidaMaxima)
        {
            //altered state

            //regeneration
            vidaActual += Time.deltaTime;
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
    }

    void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // Puedes desactivar, destruir o reiniciar el objeto aquí:
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }
}