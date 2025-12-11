
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la sincronización de configuración entre escenas
/// Asegura que UI_Audio_Manager y GraphicsSettingsManager se actualicen correctamente
/// </summary>
public class ConfigSyncManager : MonoBehaviour
{
    public static ConfigSyncManager instance;

    private UI_Audio_Manager audioManager;
    private ResolutionSettings graphicsManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.GameDataLoaded += OnGameDataLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.GameDataLoaded -= OnGameDataLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Limpiar referencias
        audioManager = null;
        graphicsManager = null;

        // Buscar managers después de un delay
        Invoke(nameof(FindManagers), 0.2f);
    }

    private void OnGameDataLoaded(GameData data)
    {
        // Aplicar configuración de audio
        if (data != null && data.SavedConfigData != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(data.SavedConfigData.MasterVolume);
            AudioManager.Instance.SetMusicVolume(data.SavedConfigData.MusicVolume);
            AudioManager.Instance.SetSFXVolume(data.SavedConfigData.SFXVolume);
        }

        // Aplicar configuración gráfica
        if (data != null && data.SavedConfigData != null)
        {
            ApplyGraphicsSettings(data.SavedConfigData);
        }
    }

    private void FindManagers()
    {
        // Buscar UI_Audio_Manager
        audioManager = FindAnyObjectByType<UI_Audio_Manager>();
        if (audioManager != null)
        {
            Debug.Log("ConfigSyncManager: UI_Audio_Manager found");
        }

        // Buscar GraphicsSettingsManager
        graphicsManager = FindAnyObjectByType<ResolutionSettings>();
        if (graphicsManager != null)
        {
            Debug.Log("ConfigSyncManager: GraphicsSettingsManager found");
        }

        // Aplicar configuración actual si los managers se encontraron
        if (DataPersistenceManager.instance != null)
        {
            GameData data = DataPersistenceManager.instance.GetGameData();
            if (data != null && data.SavedConfigData != null)
            {
                ApplyAllSettings(data.SavedConfigData);
            }
        }
    }

    private void ApplyAllSettings(ConfigData config)
    {
        ApplyAudioSettings(config);
        ApplyGraphicsSettings(config);
    }

    private void ApplyAudioSettings(ConfigData config)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(config.MasterVolume);
            AudioManager.Instance.SetMusicVolume(config.MusicVolume);
            AudioManager.Instance.SetSFXVolume(config.SFXVolume);
            Debug.Log($"ConfigSyncManager: Audio settings applied - Master: {config.MasterVolume}, Music: {config.MusicVolume}, SFX: {config.SFXVolume}");
        }
    }

    private void ApplyGraphicsSettings(ConfigData config)
    {
        // Aplicar fullscreen
        Screen.fullScreen = config.IsFullscreen;

        // Aplicar resolución
        if (config.ResolutionWidth > 0 && config.ResolutionHeight > 0)
        {
            Screen.SetResolution(config.ResolutionWidth, config.ResolutionHeight, config.IsFullscreen);
            Debug.Log($"ConfigSyncManager: Graphics settings applied - Resolution: {config.ResolutionWidth}x{config.ResolutionHeight}, Fullscreen: {config.IsFullscreen}");
        }

        // Aplicar sensibilidad a la cámara si existe
        ApplySensitivityToCamera(config.Sensitivity);
    }

    private void ApplySensitivityToCamera(float sensitivity)
    {
        // Buscar la cámara del jugador
        Camera_FPS_Controller cameraController = FindAnyObjectByType<Camera_FPS_Controller>();
        if (cameraController != null)
        {
            // Asumiendo que tu Camera_FPS_Controller tiene una propiedad sensitivity
            // Ajusta esto según tu implementación real
            cameraController.sencitivily = sensitivity;
            Debug.Log($"ConfigSyncManager: Sensitivity applied to camera - {sensitivity}");
        }
    }

    /// <summary>
    /// Fuerza la recarga de configuración desde GameData
    /// </summary>
    public void ForceReloadConfig()
    {
        if (DataPersistenceManager.instance != null)
        {
            GameData data = DataPersistenceManager.instance.GetGameData();
            if (data != null && data.SavedConfigData != null)
            {
                ApplyAllSettings(data.SavedConfigData);

                // Actualizar UI managers si existen
                if (audioManager != null)
                {
                    audioManager.LoadData(data);
                }

                if (graphicsManager != null)
                {
                    graphicsManager.LoadData(data);
                }
            }
        }
    }

    /// <summary>
    /// Guarda la configuración actual inmediatamente
    /// </summary>
    public void SaveConfigNow()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
            Debug.Log("ConfigSyncManager: Configuration saved");
        }
    }
}