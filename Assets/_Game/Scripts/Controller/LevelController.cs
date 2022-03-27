using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LevelController : MonoBehaviour
{
    #region Member Variables
    static PlayerData playerData = new PlayerData();
    [SerializeField] BubbleController bubbleController;

    GameObject defaultBubble;
    ParticleSystem defaultVfx;

    [Tooltip("Set this to be the maximum number of levels")]
    public int maxLevel = 100;

    static float staticLevelTime = 45f;
    [Tooltip("Time of levels in seconds")]
    [SerializeField] float levelTime = 45f;

    [Tooltip("Set this to be the number of starting hitpoints")]
    [SerializeField] int maxHitPoints = 3;
    static int hitPoints = 3;

    [SerializeField] List<LevelData> levelDatas = new List<LevelData>();

    public static Action<int> OnNextLevel;
    public static Action<Slider, float> OnWinLevel;
    public static Action<Slider> OnLoseLevel;
    public static Action OnBubbleHitWall;
    #endregion

    #region Getters
    public static PlayerData GetPlayerData() => playerData;
    public static float GetLevelTime() => staticLevelTime;
    public static int GetHitPoints() => hitPoints;
    #endregion

    #region Setters
    public static void SetHitPoints(int newHitPoints) => hitPoints = newHitPoints;
    public static void SetLevelTime(int newLevelTime) => staticLevelTime = newLevelTime;
    #endregion

    #region Initializers
    void Awake()
    {
        PlayerData.LoadPlayerData(playerData);

        defaultBubble = bubbleController.GetDefaultBubblePrefab();
        defaultVfx = bubbleController.GetDefaultPopVFX();

        hitPoints = maxHitPoints;
        staticLevelTime = levelTime;

        OnNextLevel += OnNextLevelHandler;
        OnWinLevel += OnWinLevelHandler;
        OnLoseLevel += OnLoseLevelHandler;
        OnBubbleHitWall += OnBubbleHitWallHandler;

        InitLevelData();
    }
    void InitLevelData()
    {
        for (int i = 0; i < maxLevel; i++)
        {
            //initialize list with maxLevel amount, if list is less than maxLevel
            if (levelDatas.Count < maxLevel)
                levelDatas.Add(new LevelData());

            int newLevel = i + 1;

            //init level data with default values
            LevelData data = levelDatas[i];
            LevelData levelData = new LevelData(
                newLevel, defaultBubble, defaultVfx,
                data.bubbles, data.vfxs, data.bubblePrefabs,
                data.maxBubbles, data.amountAllowedAtOnce, data.chooseBubblesRandomly
                );

            //assign default value to level data list
            levelDatas[i] = levelData;
        }
    }
    #endregion

    #region Event Handlers
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
    void OnWinLevelHandler(Slider levelTimeSlider, float levelTime)
    {
        bool levelEnded = false;

        //if level ends by time
        if (levelTimeSlider.value >= levelTime)
            levelEnded = true;

        if (levelTimeSlider.value <= 0 && BubbleController.GetGameStarted())
            BonusController.endBonus = true;

        if (levelEnded)
            BonusController.OnStartBonus.Invoke();

        if (!BonusController.endBonus) return;

        //reset the bubble controller to prepare for next level
        bubbleController.ResetBubbleController();
        
        //load main menu scene
        SceneManager.LoadSceneAsync(TAGS.MAIN_MENU_SCENE_NAME, LoadSceneMode.Additive);

        //increase player level by 1 and save to local disk
        playerData.SetLevel(playerData.GetLevel() + 1);
        PlayerData.SavePlayerData(playerData);

        //reset values
        UiController.OnDisableUI.Invoke();
        levelTimeSlider.value = 0f;
        PlayerController.SetScore(0);
        SetHitPoints(maxHitPoints);
        BonusController.endBonus = false;
        BonusController.isBonus = false;
        BubbleController.activeBubbles = 0;
        levelEnded = false;
        return;
    }
    void OnLoseLevelHandler(Slider levelTimeSlider)
    {
        bool levelEnded = false;

        //if level ends by hitpoints
        if (hitPoints <= 0)
            levelEnded = true;

        if (levelEnded)
        {
            bubbleController.ResetBubbleController();

            SceneManager.LoadSceneAsync(TAGS.MAIN_MENU_SCENE_NAME, LoadSceneMode.Additive);

            //reset UI values
            UiController.OnDisableUI.Invoke();
            levelTimeSlider.value = 0f;
            PlayerController.SetScore(0);
            SetHitPoints(maxHitPoints);
            levelEnded = false;
            return;
        }
    }
    void OnBubbleHitWallHandler()
    {
        int hp = GetHitPoints();
        SetHitPoints(hp - 1);
    }
    #endregion

    #region Save-Load
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
    #endregion
}