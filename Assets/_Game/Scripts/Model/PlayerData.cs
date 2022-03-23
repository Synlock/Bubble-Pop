using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int coins;
    public List<MeshData> unlockedMeshes = new List<MeshData>();
    public Mesh selectedMesh;
    //public List<ParticleSystem> unlockedParticleSystems = new List<ParticleSystem>();

    public bool isFirstLoad = true;

    #region Getters
    public int GetLevel() => level;
    public int GetCoins() => coins;
    #endregion

    #region Setters
    public void SetLevel(int newLevel) => level = newLevel;
    public void SetCoins(int newCoins) => coins = newCoins;
    #endregion

    public PlayerData()
    {
        level = 1;
        coins = 0;
        unlockedMeshes = new List<MeshData>();
    }
    public PlayerData(int level)
    {
        this.level = level;
    }

    public static void LoadPlayerData(PlayerData playerData)
    {
        SaveLoad.ConvertAndLoadData(playerData, TAGS.PLAYER_FILE_NAME, Application.persistentDataPath);
    }
    public static void SavePlayerData(PlayerData playerData)
    {
        SaveLoad.SaveAndConvertData(playerData, TAGS.PLAYER_FILE_NAME, Application.persistentDataPath);
    }
}