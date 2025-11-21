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

    public GameData()
    {
        this._savedPlayerData = new PlayerData();
        this._enemies = new();
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
}

[Serializable]
public class PlayerData
{
    public float VidaActual { get => _vidaActual; set => _vidaActual = value; }
    [SerializeField] private float _vidaActual;

    public PlayerData()
    {
    }
    public PlayerData(float vida)
    {
        _vidaActual = vida;
    }
}

[Serializable]
public class EnemyData
{
    public string Id { get => _id; set => _id = value; }
    [SerializeField] string _id;

    public bool Active { get => _active; set => _active = value; }
    [SerializeField] bool _active;
}
