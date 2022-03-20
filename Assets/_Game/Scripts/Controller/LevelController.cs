using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LevelController : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] BubbleController bubbleController;

    GameObject defaultBubble;
    ParticleSystem defaultVfx;

    public int maxLevel = 100;
    public int currentLevel = 1;
    static int level;

    [SerializeField] List<LevelData> levelDatas = new List<LevelData>();

    public static Action<int> OnNextLevel;

    public static int GetCurrentLevel() => level;
    public static void SetCurrentLevel(int newLevel) => level = newLevel;

    void Awake()
    {
        defaultBubble = bubbleController.GetDefaultBubblePrefab();
        defaultVfx = bubbleController.GetDefaultPopVFX();

        OnNextLevel += OnNextLevelHandler;

        level = currentLevel;

        InitLevelData();
    }

    void InitLevelData()
    {
        for (int i = 0; i < maxLevel; i++)
        {
            if (levelDatas.Count < maxLevel)
                levelDatas.Add(new LevelData());

            int newLevel = i + 1;

            LevelData data = levelDatas[i];
            LevelData levelData = new LevelData(
                newLevel, defaultBubble, defaultVfx,
                data.bubbles, data.vfxs, data.bubblePrefabs,
                data.maxBubbles, data.amountAllowedAtOnce, data.chooseBubblesRandomly
                );

            levelDatas[i] = levelData;
        }
    }

    void OnNextLevelHandler(int index)
    {
        LevelData levelData = levelDatas[index];

        GameObject defaultBubblePrefab = levelData.defaultBubblePrefab;
        ParticleSystem defaultVFXPrefab = levelData.defaultVfx;

        List<BubbleData> bubbles = new List<BubbleData>(levelData.bubbles);
        ParticleSystem[] vfx = levelData.vfxs;
        GameObject[] prefabs = levelData.bubblePrefabs;
        int maxBubs = levelData.maxBubbles;
        int amountAllowed = levelData.amountAllowedAtOnce;
        bool isRandom = levelData.chooseBubblesRandomly;

        if (isRandom && bubbles.Count > 0)
            isRandom = false;

        bubbleController.SetDefaultBubblesPrefab(defaultBubblePrefab);
        bubbleController.SetDefaultPopVFX(defaultVFXPrefab);
        bubbleController.SetBubbles(bubbles);
        bubbleController.SetPopVFXs(vfx);
        bubbleController.SetBubblePrefabs(prefabs);
        bubbleController.SetMaxBubbles(maxBubs);
        bubbleController.SetAmountAllowed(amountAllowed);
        bubbleController.SetChooseRandomly(isRandom);
    }

    public void Save(string filePath, string dirPath)
    {
        string json = SaveLoad.SaveToJSON(this, filePath, dirPath);
        File.WriteAllText(filePath, json);
    }
    public void Load(string path)
    {
        string json = File.ReadAllText(path);
        SaveLoad.LoadFromJSON(json, this);
    }
}