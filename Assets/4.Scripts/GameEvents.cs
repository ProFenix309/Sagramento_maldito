using System;
using UnityEngine;

public static class GameEvents
{

    public static Action<ConfigData> ConfigLoaded;
    public static Action<GameData> GameDataLoaded;
    public static Action<GameData> GameDataSaved;
    public static Action<WorldData> Worldloaded;
    public static Action<EnemyData> EnemyLoaded;
    public static Action<PlayerData> PlayerLoaded;
    
}
