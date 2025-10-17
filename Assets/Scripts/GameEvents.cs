using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<GameData> GameDataLoaded;
    public static Action<EnemyData> EnemyLoaded;
    public static Action<PlayerData> PlayerLoaded;
}
