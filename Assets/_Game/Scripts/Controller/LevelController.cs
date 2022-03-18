using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LevelController : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] BubbleController bubbleController;
    [SerializeField] List<LevelData> levelDatas = new List<LevelData>();

    public int currentLevel = 1;
    public int maxLevel = 100;

    public List<BubbleData> bubbles = new List<BubbleData>();
    public ParticleSystem[] vfxs = new ParticleSystem[1];
    public GameObject[] bubblePrefabs = new GameObject[1];

    public int maxBubbles = 10;
    public int amountAllowedAtOnce = 5;

    public bool chooseBubblesRandomly = false;

    public static Action<int> OnNextLevel;

    void Awake()
    {
        OnNextLevel += OnNextLevelHandler;

        InitLevelData();
        SaveLevelDatasToJSON();
    }

    void InitLevelData()
    {
        for (int i = 0; i < maxLevel; i++)
        {
            if (levelDatas.Count - 1 < maxLevel)
                levelDatas.Add(new LevelData());

            int newLevel = i + 1;
            LevelData levelData = new LevelData(
                newLevel, bubbles, vfxs, bubblePrefabs, maxBubbles, amountAllowedAtOnce, chooseBubblesRandomly
                );

            if (levelDatas[i].level == newLevel) continue;

            levelDatas[i] = levelData;
        }
    }

    //TODO:  pull current level data from player data
    void OnNextLevelHandler(int index)
    {
        LevelData levelData = levelDatas[index];
        int level = levelData.level;

        List<BubbleData> bubbles = levelDatas[level].bubbles;
        ParticleSystem[] vfx = levelDatas[level].vfxs;
        GameObject[] prefabs = levelDatas[level].bubblePrefabs;
        int maxBubs = levelDatas[level].maxBubbles;
        int amountAllowed = levelDatas[level].amountAllowedAtOnce;
        bool isRandom = levelDatas[level].chooseBubblesRandomly;

        bubbleController.SetBubbles(bubbles);
        bubbleController.SetPopVFXs(vfx);
        bubbleController.SetBubblePrefabs(prefabs);
        bubbleController.SetMaxBubbles(maxBubs);
        bubbleController.SetAmountAllowed(amountAllowed);
        bubbleController.SetChooseRandomly(isRandom);
    }
}