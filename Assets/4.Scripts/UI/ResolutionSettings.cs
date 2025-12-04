using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    public static ResolutionSettings instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }


    public TMP_Dropdown resolutionsDropdown;
    public Toggle fullscreenToggle;
    private Resolution[] resolutions;
    public Slider brightSlider;
    public Light directionalLight;

    void Start()
    {
        // 1. Obtener todas las resoluciones del sistema
        resolutions = Screen.resolutions;

        // 2. Limpiar las opciones actuales del Dropdown
        resolutionsDropdown.ClearOptions();

        // 3. Crear una lista de strings para las resoluciones
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // 4. Encontrar la resolución actual para seleccionarla por defecto
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // 5. Agregar las opciones al Dropdown y establecer el valor por defecto
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        // Obtener el índice seleccionado en el Dropdown
        int resolutionIndex = resolutionsDropdown.value;

        // Obtener la resolución del array 'resolutions'
        Resolution resolution = resolutions[resolutionIndex];

        // El segundo parámetro, 'fullscreen', ahora viene del estado del Toggle.
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);

    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetBrightness()
    {
        // Asigna el valor del slider a la intensidad de la luz.
        directionalLight.intensity = brightSlider.value;
    }
}
