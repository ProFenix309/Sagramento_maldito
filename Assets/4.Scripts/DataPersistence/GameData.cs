using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<EnemyData> Enemies { get => _enemies; set => _enemies = value; }
    [SerializeField] List<EnemyData> _enemies;

    public PlayerData SavedPlayerData { get => _savedPlayerData; }
    [SerializeField] PlayerData _savedPlayerData;

    public WorldData SavedWorldData { get => _savedWorldData; set => _savedWorldData = value; }
    [SerializeField] WorldData _savedWorldData;

    public ConfigData SavedConfigData { get => _savedConfigData; set => _savedConfigData = value; }
    [SerializeField] ConfigData _savedConfigData;

    public GameData()
    {
        this._savedPlayerData = new PlayerData();
        this._enemies = new List<EnemyData>();
        this._savedWorldData = new WorldData();
        this._savedConfigData = new ConfigData();
    }

    public EnemyData GetEnemyDataById(string id)
    {
        return _enemies.Find(x => x.Id == id);
    }

    public void AddEnemyData(EnemyData data)
    {
        _enemies.Add(data);
    }

    public void SetPlayerData(PlayerData data)
    {
        _savedPlayerData = data;
    }

    public void SetWorldData(WorldData data)
    {
        _savedWorldData = data;
    }

    public void SetConfigData(ConfigData data)
    {
        _savedConfigData = data;
    }
}

[Serializable]
public class ConfigData
{
    // Audio settings
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    [SerializeField] private float _masterVolume = 1f;

    public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
    [SerializeField] private float _musicVolume = 1f;

    public float SFXVolume { get => _sfxVolume; set => _sfxVolume = value; }
    [SerializeField] private float _sfxVolume = 1f;

    // Graphics settings
    public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }
    [SerializeField] private float _sensitivity = 1f;

    public bool IsFullscreen { get => _isFullscreen; set => _isFullscreen = value; }
    [SerializeField] private bool _isFullscreen = true;

    public int ResolutionWidth { get => _resolutionWidth; set => _resolutionWidth = value; }
    [SerializeField] private int _resolutionWidth = 1920;

    public int ResolutionHeight { get => _resolutionHeight; set => _resolutionHeight = value; }
    [SerializeField] private int _resolutionHeight = 1080;

    public int ResolutionIndex { get => _resolutionIndex; set => _resolutionIndex = value; }
    [SerializeField] private int _resolutionIndex = 0;

    public ConfigData()
    {
        // Valores por defecto ya est�n asignados arriba
    }
}

[Serializable]
public class WorldData
{
    public int SavedAct { get => _savedAct; set => _savedAct = value; }
    [SerializeField] private int _savedAct = 1;

    public WorldData()
    {
    }

    public WorldData(int SavedScene)
    {
        _savedAct = SavedScene;
    }
}

[Serializable]
public class PlayerData
{
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    [SerializeField] private float _currentHealth;

    public Vector3 PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
    [SerializeField] private Vector3 _playerPosition;

    public Vector3 PlayerPosition1 { get => _playerPosition1; set => _playerPosition1 = value; }
    [SerializeField] private Vector3 _playerPosition1 = new Vector3(2.16f, 3.05f, 3.95f);

    public Vector3 PlayerPosition2 { get => _playerPosition2; set => _playerPosition2 = value; }
    [SerializeField] private Vector3 _playerPosition2 = new Vector3(0.07f, 5.71f, 20.43f);

    public Vector3 PlayerPosition3 { get => _playerPosition3; set => _playerPosition3 = value; }
    [SerializeField] private Vector3 _playerPosition3 = new Vector3(2.16f, 3.05f, 3.95f);

    public List<int> Items { get => _items; set => _items = value; }
    [SerializeField] private List<int> _items;

    public PlayerData()
    {
        _items = new List<int>();
    }

    public PlayerData(GameObject pObject, Vector3 posicion1, Vector3 posicion2, Vector3 posicion3)
    {
        _playerPosition1 = posicion1;
        _playerPosition2 = posicion2;
        _playerPosition3 = posicion3;
        _items = new List<int>();
    }
}

[Serializable]
public class EnemyData
{
    public string Id { get => _id; set => _id = value; }
    [SerializeField] string _id;

    public bool Active { get => _active; set => _active = value; }
    [SerializeField] bool _active;

    public Vector3 Position { get => _position; set => _position = value; }
    [SerializeField] Vector3 _position;

    public EnemyData()
    {
    }

    public EnemyData(string id, bool active, Vector3 position)
    {
        _id = id;
        _active = active;
        _position = position;
    }
}
