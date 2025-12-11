using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider sensitivitySlider;

    private Resolution[] resolutions;
    private bool isInitialized = false;

    private void Start()
    {
        // Esperar un frame para que el DataPersistenceManager cargue los datos
        Invoke(nameof(Initialize), 0.1f);
    }

    private void OnEnable()
    {
        GameEvents.GameDataLoaded += LoadData;
        GameEvents.GameDataSaved += SaveData;
    }

    private void OnDisable()
    {
        GameEvents.GameDataLoaded -= LoadData;
        GameEvents.GameDataSaved -= SaveData;
    }

    private void Initialize()
    {
        if (isInitialized) return;

        // Obtener resoluciones disponibles
        resolutions = Screen.resolutions;

        // Configurar el dropdown de resoluciones
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        // Cargar configuración guardada
        if (DataPersistenceManager.instance != null)
        {
            GameData data = DataPersistenceManager.instance.GetGameData();
            if (data != null && data.SavedConfigData != null)
            {
                LoadData(data);
            }
            else
            {
                LoadDefaultSettings();
            }
        }
        else
        {
            LoadDefaultSettings();
        }

        // Añadir listeners
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        isInitialized = true;
    }

    private void LoadDefaultSettings()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(1f);
        }
    }

    public void LoadData(GameData data)
    {
        if (data == null || data.SavedConfigData == null)
        {
            LoadDefaultSettings();
            return;
        }

        // Aplicar fullscreen
        Screen.fullScreen = data.SavedConfigData.IsFullscreen;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(data.SavedConfigData.IsFullscreen);
        }

        // Aplicar resolución
        if (data.SavedConfigData.ResolutionWidth > 0 && data.SavedConfigData.ResolutionHeight > 0)
        {
            Screen.SetResolution(
                data.SavedConfigData.ResolutionWidth,
                data.SavedConfigData.ResolutionHeight,
                data.SavedConfigData.IsFullscreen
            );

            if (resolutionDropdown != null && data.SavedConfigData.ResolutionIndex >= 0 &&
                data.SavedConfigData.ResolutionIndex < resolutions.Length)
            {
                resolutionDropdown.SetValueWithoutNotify(data.SavedConfigData.ResolutionIndex);
            }
        }

        // Aplicar sensibilidad
        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(data.SavedConfigData.Sensitivity);
        }

        Debug.Log($"Graphics settings loaded: Fullscreen={data.SavedConfigData.IsFullscreen}, " +
                  $"Resolution={data.SavedConfigData.ResolutionWidth}x{data.SavedConfigData.ResolutionHeight}, " +
                  $"Sensitivity={data.SavedConfigData.Sensitivity}");
    }

    public void SaveData(GameData data)
    {
        if (data == null || data.SavedConfigData == null)
            return;

        if (fullscreenToggle != null)
            data.SavedConfigData.IsFullscreen = fullscreenToggle.isOn;

        if (resolutionDropdown != null && resolutionDropdown.value < resolutions.Length)
        {
            data.SavedConfigData.ResolutionIndex = resolutionDropdown.value;
            data.SavedConfigData.ResolutionWidth = resolutions[resolutionDropdown.value].width;
            data.SavedConfigData.ResolutionHeight = resolutions[resolutionDropdown.value].height;
        }

        if (sensitivitySlider != null)
            data.SavedConfigData.Sensitivity = sensitivitySlider.value;

        Debug.Log($"Graphics settings saved: Fullscreen={data.SavedConfigData.IsFullscreen}, " +
                  $"Resolution={data.SavedConfigData.ResolutionWidth}x{data.SavedConfigData.ResolutionHeight}, " +
                  $"Sensitivity={data.SavedConfigData.Sensitivity}");
    }

    public void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        // Guardar inmediatamente
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
        }
    }

    public void OnResolutionChanged(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            // Guardar inmediatamente
            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.SaveGameData();
            }
        }
    }

    public void OnSensitivityChanged(float sensitivity)
    {
        // Aquí puedes aplicar la sensibilidad a tus controles de cámara
        // Por ejemplo: Camera_FPS_Controller.instance.sensitivity = sensitivity;

        // Guardar inmediatamente
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
        }
    }
}