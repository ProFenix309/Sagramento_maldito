using UnityEngine;
using UnityEngine.UI;

public class UI_Audio_Manager : MonoBehaviour, IDataPersistence
{
    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isInitialized = false;

    void Start()
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

        // Si hay DataPersistenceManager, cargar desde GameData
        if (DataPersistenceManager.instance != null)
        {
            GameData data = DataPersistenceManager.instance.GetGameData();
            if (data != null && data.SavedConfigData != null)
            {
                LoadData(data);
            }
            else
            {
                // Si no hay datos guardados, usar valores por defecto
                LoadDefaultValues();
            }
        }
        else
        {
            // Fallback a PlayerPrefs si no hay DataPersistenceManager
            LoadFromPlayerPrefs();
        }

        // Añadir listeners para cambios en tiempo real
        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        isInitialized = true;
    }

    private void LoadDefaultValues()
    {
        if (masterSlider != null)
        {
            masterSlider.value = 1f;
            OnMasterVolumeChanged(1f);
        }
        if (musicSlider != null)
        {
            musicSlider.value = 1f;
            OnMusicVolumeChanged(1f);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = 1f;
            OnSFXVolumeChanged(1f);
        }
    }

    private void LoadFromPlayerPrefs()
    {
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (masterSlider != null)
        {
            masterSlider.value = masterVol;
            OnMasterVolumeChanged(masterVol);
        }
        if (musicSlider != null)
        {
            musicSlider.value = musicVol;
            OnMusicVolumeChanged(musicVol);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVol;
            OnSFXVolumeChanged(sfxVol);
        }
    }

    public void LoadData(GameData data)
    {
        if (data == null || data.SavedConfigData == null)
        {
            LoadDefaultValues();
            return;
        }

        // Cargar valores sin triggear el evento (para evitar loops)
        if (masterSlider != null)
        {
            masterSlider.SetValueWithoutNotify(data.SavedConfigData.MasterVolume);
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetMasterVolume(data.SavedConfigData.MasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(data.SavedConfigData.MusicVolume);
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetMusicVolume(data.SavedConfigData.MusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(data.SavedConfigData.SFXVolume);
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetSFXVolume(data.SavedConfigData.SFXVolume);
        }

        Debug.Log($"Audio settings loaded: Master={data.SavedConfigData.MasterVolume}, Music={data.SavedConfigData.MusicVolume}, SFX={data.SavedConfigData.SFXVolume}");
    }

    public void SaveData(GameData data)
    {
        if (data == null || data.SavedConfigData == null)
        {
            Debug.LogWarning("UI_Audio_Manager: Cannot save - data is null");
            return;
        }

        try
        {
            if (masterSlider != null)
                data.SavedConfigData.MasterVolume = masterSlider.value;
            if (musicSlider != null)
                data.SavedConfigData.MusicVolume = musicSlider.value;
            if (sfxSlider != null)
                data.SavedConfigData.SFXVolume = sfxSlider.value;

            Debug.Log($"Audio settings saved: Master={data.SavedConfigData.MasterVolume}, Music={data.SavedConfigData.MusicVolume}, SFX={data.SavedConfigData.SFXVolume}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in UI_Audio_Manager.SaveData: {e.Message}");
        }
    }

    public void OnMasterVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(value);
        }

        // Guardar inmediatamente
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
        }

        // También guardar en PlayerPrefs como backup
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }

        // Guardar inmediatamente
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }

        // Guardar inmediatamente
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGameData();
        }

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
}
