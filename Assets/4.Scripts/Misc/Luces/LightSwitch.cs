using UnityEngine;

public class LightSwitch : MonoBehaviour, Interactable
{
    private Light luz;
    private bool encendida = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;


    private void Awake()
    {
        luz = GetComponent<Light>();
        audioSource.GetComponent<AudioSource>();

        if (luz == null)
        {
            Debug.LogWarning("No se encontr� un componente Light en este objeto interactuable.");
        }
    }

    public void Interact()
    {
        if (luz == null) return;

        encendida = !encendida;
        luz.enabled = encendida;
        audioSource.PlayOneShot(audioClip);


        Debug.Log($"Luz {(encendida ? "encendida" : "apagada")}");
    }
}