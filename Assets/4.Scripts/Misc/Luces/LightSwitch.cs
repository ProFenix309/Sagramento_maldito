using UnityEngine;

public class LightSwitch : MonoBehaviour, Interactable
{
    private Light luz;
    private bool encendida = true;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void Awake()
    {
        luz = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        if (luz == null)
        {
            Debug.LogWarning("No se encontró un componente Light en este objeto interactuable.");
        }

        // Asegurar que la luz inicie apagada
        if (luz != null)
            luz.enabled = false;
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

        if (luz != null)
            luz.enabled = encendida;

        if (encendida && audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        if (!encendida)
        {
            OffLightSound();
        }
    }

    // Metodo para que el enemigo apague la luz
    public bool IsLightOn()
    {
        return encendida && luz != null && luz.enabled;
    }
    private void OffLightSound()
    {
        AudioManager.Instance.PlaySFX3D("Soplido", transform.position);
    }

}