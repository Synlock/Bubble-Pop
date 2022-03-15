using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] GameObject defaultBubblePrefab;
    [SerializeField] GameObject[] bubblePrefabs = new GameObject[1];

    [Tooltip("Randomly or manually add to the list of bubbles - pool selects bubbles from here")]
    [SerializeField] List<BubbleData> bubbles = new List<BubbleData>();
    [SerializeField] GameObject bubblesParent;

    [Tooltip("Set max number of bubbles to enter the pool")]
    [SerializeField] int maxBubbles = 10;

    float maxSpeed = 100f;
    float timeBetweenSpawns = 2f;

    [Tooltip("Check TRUE if bubble pool will be chosen randomly")]
    [SerializeField] bool chooseRandomly = false;

    public List<BubbleData> GetBubbles() => bubbles;
    public GameObject GetBubblesParent() => bubblesParent;

    void Awake()
    {
        InitBubblePrefabs();

        CheckBubbleParentExists();

        if (chooseRandomly)
        {
            InstantiateRandomBubblePool(maxBubbles, bubblesParent);
        }
        else InstantiateCustomBubblePool(bubbles);
    }

    void InitBubblePrefabs()
    {
        if (bubblePrefabs.Length <= 1)
            bubblePrefabs[0] = defaultBubblePrefab;
    }

    void CheckBubbleParentExists()
    {
        if (bubblesParent == null)
        {
            bubblesParent = new GameObject();
            bubblesParent.transform.position = Vector3.zero;
            bubblesParent.name = "Bubbles Parent";
        }
    }

    void InstantiateCustomBubblePool(List<BubbleData> bubbles)
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
            ParticleSystem particles = bubbles[i].GetParticleSystem();
            SpawnPoint point = bubbles[i].GetSpawnPoint();
            float speed = bubbles[i].GetSpeed();
            float spawnTime = bubbles[i].GetTimeBetweenSpawns();
            int score = bubbles[i].GetScore();

            BubbleData data = new BubbleData(go, type, particles, point, speed, spawnTime, score);
            bubbles[i] = data;
        }
    }

    void InstantiateRandomBubblePool(int maxBubbles, GameObject bubblesParent)
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
                null, 
                ChooseRandomSpawnPoint(),
                ChooseRandomNumber(maxSpeed),
                ChooseRandomNumber(timeBetweenSpawns),
                (int)ChooseRandomNumber(100)
                );

            bubbles.Add(newBubble);
            bubbleObj.SetActive(false);
        }
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
        float speed = rand.Next((int)maxNumber+1);

        if (speed == 0) return 1;

        return speed;
    }
}
