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
            Debug.LogWarning("No se encontro un componente Light en este objeto interactuable.");
        }
    }

    public void Interact()
    {
        if (luz == null) return;

        SwitchButtonLight();

        Debug.Log($"Luz {(encendida ? "encendida" : "apagada")}");
    }

    public void SwitchButtonLight()
    {
        encendida = !encendida;
        luz.enabled = encendida;
        if (encendida)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}