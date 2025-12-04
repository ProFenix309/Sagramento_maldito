using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }


    public AudioMixer audioMixer; // Asignar AudioMixer en el Inspector
    public Slider sliderMaster;
    public Slider sliderMusica;
    public Slider sliderAmbiente;

    void Start()
    {
        // Puedes cargar valores guardados o poner valores por defecto
        sliderMaster.onValueChanged.AddListener(SetMasterVolume);
        sliderMusica.onValueChanged.AddListener(SetMusicaVolume);
        sliderAmbiente.onValueChanged.AddListener(SetAmbienteVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicaVolume(float value)
    {
        audioMixer.SetFloat("MusicaVolume", Mathf.Log10(value) * 20);
    }

    public void SetAmbienteVolume(float value)
    {
        audioMixer.SetFloat("AmbienteVolume", Mathf.Log10(value) * 20);
    }
}

