using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public static BubbleController Instance;

    [SerializeField] GameObject defaultBubblePrefab;
    [SerializeField] GameObject[] bubblePrefabs = new GameObject[1];
    [SerializeField] ParticleSystem defaultPopVFX;
    [Tooltip("Must provide VFX prefabs here")]
    [SerializeField] ParticleSystem[] popVFXs = new ParticleSystem[1];
    List<ParticleSystem> popVFXsList = new List<ParticleSystem>();
    [SerializeField] GameObject popVFXsParent;

    [Tooltip("Randomly or manually add to the list of bubbles - pool selects bubbles from here")]
    [SerializeField] List<BubbleData> bubbles = new List<BubbleData>();
    [SerializeField] GameObject bubblesParent;

    [Tooltip("Set max number of bubbles to enter the pool")]
    [SerializeField] int maxBubbles = 10;
    [SerializeField] int amountAllowedAtOnce = 5;

    float maxSpeed = 100f;
    float timeBetweenSpawns = 2f;

    [Tooltip("Check TRUE if bubble pool will be chosen randomly")]
    [SerializeField] bool chooseRandomly = false;
    static bool gameStarted = false;

    #region Getters
    public GameObject GetDefaultBubblePrefab() => defaultBubblePrefab;
    public ParticleSystem GetDefaultPopVFX() => defaultPopVFX;
    public List<BubbleData> GetBubbles() => bubbles;
    public GameObject GetBubblesParent() => bubblesParent;
    public ParticleSystem[] GetPopVFXs() => popVFXs;
    public GameObject[] GetBubblePrefabss() => bubblePrefabs;
    public int GetMaxBubbles() => maxBubbles;
    public int GetAmountAllowed() => amountAllowedAtOnce;
    public bool GetChooseRandomly() => chooseRandomly;
    public static bool GetGameStarted() => gameStarted;
    #endregion

    #region Setters
    public void SetDefaultBubblesPrefab(GameObject newBubblePrefab) => defaultBubblePrefab = newBubblePrefab;
    public void SetDefaultPopVFX(ParticleSystem newVfxPrefab) => defaultPopVFX = newVfxPrefab;
    public void SetBubbles(List<BubbleData> newBubbles) => bubbles = newBubbles;
    public void SetPopVFXs(ParticleSystem[] newVfx) => popVFXs = newVfx;
    public void SetBubblePrefabs(GameObject[] newPrefabs) => bubblePrefabs = newPrefabs;
    public void SetMaxBubbles(int newMaxBubbles) => maxBubbles = newMaxBubbles;
    public void SetAmountAllowed(int newAmountAllowed) => amountAllowedAtOnce = newAmountAllowed;
    public void SetChooseRandomly(bool isRandom) => chooseRandomly = isRandom;
    #endregion

    public static Action<Vector3> OnBubblePopped;
    public static Action OnInitBubbleController;

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        OnInitBubbleController += OnInitBubbleControllerHandler;
        OnBubblePopped += OnBubblePoppedHandler;
    }
    void OnInitBubbleControllerHandler()
    {
        InitBubblePrefabs();
        InitPopVFXPrefabs();

        if (chooseRandomly)
        {
            InitRandomBubblePool(bubbles, maxBubbles, bubblesParent);
        }
        else InitCustomBubblePool(bubbles);

        OnBubblePopped.Invoke(Vector3.zero);
    }

    void InitBubblePrefabs()
    {
        if (bubblesParent == null)
        {
            bubblesParent = new GameObject();
            bubblesParent.transform.position = Vector3.zero;
            bubblesParent.name = "Bubbles Parent";
        }

        if (bubblePrefabs.Length <= 1)
            bubblePrefabs[0] = defaultBubblePrefab;
    }
    void InitPopVFXPrefabs()
    {
        if (popVFXsParent == null)
        {
            popVFXsParent = new GameObject();
            popVFXsParent.transform.position = Vector3.zero;
            popVFXsParent.name = "VFX Parent";
        }

        if (popVFXs.Length <= 1)
            popVFXs[0] = defaultPopVFX;

        foreach (ParticleSystem ps in popVFXs)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject vfxObj = Instantiate(ps.gameObject, popVFXsParent.transform);
                popVFXsList.Add(vfxObj.GetComponent<ParticleSystem>());
            }
        }
    }

    void InitCustomBubblePool(List<BubbleData> bubbles)
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            GameObject bubbleObj = Instantiate(
                bubblePrefabs[UnityEngine.Random.Range(0, bubblePrefabs.Length - 1)],
                Vector3.zero,
                Quaternion.identity,
                bubblesParent.transform
                );

            GameObject go = bubbleObj;
            BubbleType type = bubbles[i].GetBubbleType();
            Color color = bubbles[i].GetColor();
            ParticleSystem particles = bubbles[i].GetParticleSystem();
            AudioClip clip = bubbles[i].GetAudioClip();
            SpawnPoint point = bubbles[i].GetSpawnPoint();
            float speed = bubbles[i].GetSpeed();
            float spawnTime = bubbles[i].GetTimeBetweenSpawns();
            int minScore = bubbles[i].GetMinScore();
            int maxScore = bubbles[i].GetMaxScore();
            bool standardColor = bubbles[i].GetStandardSpecs();

            BubbleData data = new BubbleData(go, type, color, particles, clip,
                point, minScore, maxScore, speed, spawnTime, standardColor);

            bubbles[i] = data;

            bubbles[i].GetGameObject().SetActive(false);
        }
    }
    void InitRandomBubblePool(List<BubbleData> bubbles, int maxBubbles, GameObject bubblesParent)
    {
        for (int i = 0; i < maxBubbles; i++)
        {
            GameObject bubbleObj = Instantiate(
                bubblePrefabs[UnityEngine.Random.Range(0, bubblePrefabs.Length - 1)],
                Vector3.zero,
                Quaternion.identity,
                bubblesParent.transform
                );

            BubbleData newBubble = new BubbleData(
                bubbleObj,
                ChooseRandomBubbleType(),
                UnityEngine.Random.ColorHSV(0f, 1f),
                popVFXs[UnityEngine.Random.Range(0, popVFXs.Length - 1)],
                null, //<<<< Audio Clip
                ChooseRandomSpawnPoint(),
                (int)ChooseRandomNumber(70),
                (int)ChooseRandomNumber(70),
                ChooseRandomNumber(maxSpeed),
                ChooseRandomNumber(timeBetweenSpawns),
                true
                );

            PlayerData pd = new PlayerData();
            PlayerData.LoadPlayerData(pd);

            newBubble.GetGameObject().GetComponent<MeshFilter>().mesh = pd.selectedMesh;

            bubbles.Add(newBubble);
            bubbleObj.SetActive(false);
        }
    }

    int bubbleIndex = 0;
    void OnBubblePoppedHandler(Vector3 previousPos)
    {
        //TODO: fix object pool - first ball is bugged
        if (bubbleIndex >= bubbles.Count - 1)
            bubbleIndex = 0;

        int activeBubbles = FindObjectsOfType<Bubble>().Length;
        BubbleObjectPooling(previousPos, activeBubbles);
    }

    void BubbleObjectPooling(Vector3 previousPos, int activeBubbles)
    {
        for (int i = bubbleIndex; i < bubbles.Count; i++)
        {
            BubbleData bubble = bubbles[i];

            if (activeBubbles >= amountAllowedAtOnce)
            {
                gameStarted = true;

                break;
            }

            if (gameStarted)
                ActivateVFX(previousPos, bubble);

            bubble.GetGameObject().SetActive(true);

            if (bubble.GetGameObject().activeInHierarchy)
                activeBubbles++;

            ResetBubblePosition(bubble);

            bubbleIndex++;
        }
    }

    void ActivateVFX(Vector3 previousPos, BubbleData bubble)
    {
        bubble.SetParticleSystem(GetNonPlayingVFX());
        bubble.GetParticleSystem().transform.position = previousPos;
        bubble.GetParticleSystem().Play();
    }
    public void ResetBubblePosition(BubbleData bubble)
    {
        bubble.SpawnPointHandler(bubble.GetSpawnPoint(), bubble.GetBubbleTransform());
        bubble.GetGameObject().GetComponent<Bubble>().SetMoveDir(
            BubbleData.CalculateBubbleDirection(bubble.GetBubbleTransform().position, bubble.GetSpeed(), bubble.GetSpawnPoint())
            );
    }

    BubbleType ChooseRandomBubbleType()
    {
        Array bubbleTypes = Enum.GetValues(typeof(BubbleType));

        System.Random rand = new System.Random();
        BubbleType type = (BubbleType)bubbleTypes.GetValue(rand.Next(bubbleTypes.Length));
        return type;
    }
    public static SpawnPoint ChooseRandomSpawnPoint()
    {
        Array spawnPoints = Enum.GetValues(typeof(SpawnPoint));

        System.Random rand = new System.Random();
        SpawnPoint point = (SpawnPoint)spawnPoints.GetValue(rand.Next(spawnPoints.Length));

        return point;
    }

    float ChooseRandomNumber(float maxNumber)
    {
        System.Random rand = new System.Random();
        float num = rand.Next((int)maxNumber + 1);

        if (num == 0) return 1;

        return num;
    }
    ParticleSystem GetNonPlayingVFX()
    {
        for (int i = 0; i < popVFXsList.Count; i++)
        {
            ParticleSystem vfx = popVFXsList[i];

            if (vfx.isPlaying) continue;

            return vfx;
        }
        return null;
    }

    public void ResetBubbleController()
    {
        gameStarted = false;
        bubblePrefabs = new GameObject[1];
        popVFXs = new ParticleSystem[1];
        popVFXsList = new List<ParticleSystem>();
        bubbles = new List<BubbleData>();
        Destroy(bubblesParent);
        Destroy(popVFXsParent);
        maxBubbles = 10;
        amountAllowedAtOnce = 5;
        maxSpeed = 100f;
        timeBetweenSpawns = 2f;
        chooseRandomly = false;
        bubbleIndex = 0;
    }
}