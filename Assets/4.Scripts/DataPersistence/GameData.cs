using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


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


    public Vector3 PlayerPosition1 { get => _playerPosition1; set => _playerPosition1 = value; }
    [SerializeField] private Vector3 _playerPosition1 = new Vector3(2.16f, 3.05f, 3.95f);

    public Vector3 PlayerPosition2 { get => _playerPosition2; set => _playerPosition2 = value; }
    [SerializeField] private Vector3 _playerPosition2 = new Vector3(0.1f, 5.81f, 6.92f);

    public Vector3 PlayerPosition3 { get => _playerPosition3; set => _playerPosition3 = value; }
    [SerializeField] private Vector3 _playerPosition3 = new Vector3(2.16f, 3.05f, 3.95f);

    public GameObject Player { get => _player; set => _player = value; }
    [SerializeField] private GameObject _player; 

    public List<int> Items { get => _items; set => _items = value; }
    [SerializeField] private List<int> _items;


  
    
    public PlayerData()
    {
    }
    public PlayerData(GameObject pObject, Vector3 posicion1, Vector3 posicion2,Vector3 posicion3)
    {
        // Vector3 posición, float vida, GameObject pObject, int[] invItems

        //_playerPosition = posicion;
        _playerPosition1 = posicion1;
        _playerPosition2 = posicion2;
        _playerPosition3 = posicion3;

        //        _currentHealth = vida;
        _player = pObject;
    

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

