using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class LevelData
{
    public int level = 1;

    public List<BubbleData> bubbles = new List<BubbleData>();
    public ParticleSystem[] vfxs = new ParticleSystem[1];
    public GameObject[] bubblePrefabs = new GameObject[1];

    public int maxBubbles = 10;
    public int amountAllowedAtOnce = 5;

    public bool chooseBubblesRandomly = false;
    //background variable here

    public LevelData()
    {

    }

    public LevelData(
        int level, List<BubbleData> bubbles, ParticleSystem[] vfxs, GameObject[] bubblePrefabs,
        int maxBubbles, int amountAllowedAtOnce, bool chooseBubblesRandomly
        )
    {
        this.level = level;
        this.bubbles = bubbles;
        this.vfxs = vfxs;
        this.bubblePrefabs = bubblePrefabs;
        this.maxBubbles = maxBubbles;
        this.amountAllowedAtOnce = amountAllowedAtOnce;
        this.chooseBubblesRandomly = chooseBubblesRandomly;
    }
}
