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



    public GameData()
    {
        this._savedPlayerData = new PlayerData();
        this._enemies = new();
        this._savedWorldData = new WorldData();
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
}

[Serializable]
public class ConfigData
{
    public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }
    [SerializeField] private float _sensitivity;

    
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

    public PlayerData()
    {
    }
    public PlayerData(Vector3 posición)
    {
        _playerPosition = posición;
    }
    public PlayerData(float vida)
    {
        _currentHealth = vida;
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
}

