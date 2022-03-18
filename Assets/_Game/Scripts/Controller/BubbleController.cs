using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    //TODO: fix bubbles list - when not empty, random doesnt work
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

    #region Getters
    public List<BubbleData> GetBubbles() => bubbles;
    public GameObject GetBubblesParent() => bubblesParent;
    public ParticleSystem[] GetPopVFXs() => popVFXs;
    public GameObject[] GetBubblePrefabss() => bubblePrefabs;
    public int GetMaxBubbles() => maxBubbles;
    public int GetAmountAllowed() => amountAllowedAtOnce;
    public bool GetChooseRandomly() => chooseRandomly;
    #endregion

    #region Setters
    public void SetBubbles(List<BubbleData> newBubbles) => bubbles = newBubbles;
    public void SetPopVFXs(ParticleSystem[] newVfx) => popVFXs = newVfx;
    public void SetBubblePrefabs(GameObject[] newPrefabs) => bubblePrefabs = newPrefabs;
    public void SetMaxBubbles(int newMaxBubbles) => maxBubbles = newMaxBubbles;
    public void SetAmountAllowed(int newAmountAllowed) => amountAllowedAtOnce = newAmountAllowed;
    public void SetChooseRandomly(bool isRandom) => chooseRandomly = isRandom;
    #endregion

    public static Action<Vector3> OnBubblePopped;

    void Awake()
    {
        OnBubblePopped += OnBubblePoppedHandler;

        InitBubblePrefabs();
        InitPopVFXPrefabs();

        if (chooseRandomly)
        {
            InitRandomBubblePool(maxBubbles, bubblesParent);
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
            GameObject vfxObj = Instantiate(ps.gameObject, popVFXsParent.transform);
            popVFXsList.Add(vfxObj.GetComponent<ParticleSystem>());
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
        }
    }
    void InitRandomBubblePool(int maxBubbles, GameObject bubblesParent)
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

            bubbles.Add(newBubble);
            bubbleObj.SetActive(false);
        }
    }

    int bubbleIndex = 0;
    void OnBubblePoppedHandler(Vector3 previousPos)
    {
        //TODO: fix object pool - sometimes click deactivates two bubbles
        if (bubbleIndex >= bubbles.Count)
            bubbleIndex = 0;

        int activeBubbles = FindObjectsOfType<Bubble>().Length;
        for (int i = bubbleIndex; i < bubbles.Count; i++)
        {
            BubbleData bubble = bubbles[i];

            if (activeBubbles >= amountAllowedAtOnce) break;

            //TODO: fix VFX playing on start
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
    void ResetBubblePosition(BubbleData bubble)
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
    SpawnPoint ChooseRandomSpawnPoint()
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
}
