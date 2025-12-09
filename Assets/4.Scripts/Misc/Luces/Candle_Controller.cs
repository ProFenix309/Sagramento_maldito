using UnityEngine;

public class Candle_Controller : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] Light lightPoint;
    [SerializeField] GameObject flame;
    [SerializeField] float intencity = 5f;
    [SerializeField] float range = 10f;
    private bool actived = false;
    
    [Header("Audio Settings")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    
    [Header("Enemy Detection")]
    [SerializeField] LayerMask enemyLayer; // Capa del enemigo para detectar cuando te ataque

    void Start()
    {
        // Obtener componentes si no están asignados
        if (lightPoint == null)
            lightPoint = GetComponent<Light>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Iniciar con la luz apagada
        TurnOffLight();
    }

    void Update()
    {
        // Presionar F para encender/apagar la luz
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    // Método para cambiar el estado de la luz
    public void ToggleLight()
    {
        actived = !actived;
        
        if (actived)
        {
            TurnOnLight();
        }
        else
        {
            TurnOffLight();
        }
    }
    
    // Encender la luz
    private void TurnOnLight()
    {
        if (lightPoint != null)
        {
            lightPoint.intensity = intencity;
            lightPoint.range = range;
            lightPoint.enabled = true;
        }
        
        if (flame != null)
            flame.SetActive(true);
        
        // Reproducir sonido de encendido
        if (audioSource != null && audioClip != null)
            audioSource.PlayOneShot(audioClip);
        
        actived = true;
        Debug.Log("Luz del jugador ENCENDIDA");
    }
    
    // Apagar la luz
    private void TurnOffLight()
    {
        if (lightPoint != null)
        {
            lightPoint.intensity = 0f;
            lightPoint.range = 0f;
            lightPoint.enabled = false;
        }
        
        if (flame != null)
            flame.SetActive(false);
            OffLightSound();
        
        actived = false;
        Debug.Log("Luz del jugador APAGADA");
    }
    
    // Forzar apagar la luz (cuando el enemigo ataca)
    public void ForceOffLight()
    {
        TurnOffLight();
    }

    // Detectar cuando el enemigo toca al jugador
    private void OnTriggerEnter(Collider other)
    {
        // Si el enemigo toca al jugador, apaga la luz automáticamente
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            TurnOffLight();
            Debug.Log("¡El enemigo apagó tu luz!");
        }
    }
    
    // Método para que el enemigo apague la luz
    public bool IsLightOn()
    {
        return actived;
    }

        private void OffLightSound()
    {
        AudioManager.Instance.PlaySFX3D("Soplido", transform.position);
    }

}