using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima = 100;
    private float vidaActual;

    [Header("Daño por colisión")]
    public string etiquetaEnemigo = "Enemy";
    public int dañoPorColision = 10;

    

    void Start()
    {
        vidaActual = vidaMaxima;
        Debug.Log("Vida inicial: " + vidaActual);
    }

    private void Update()
    {
        if (vidaActual < vidaMaxima)
        {
            vidaActual += Time.deltaTime;
        }
        else
        {
            vidaActual = vidaMaxima;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(etiquetaEnemigo))
        {
            RecibirDaño(dañoPorColision);
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