using UnityEngine;

public enum BubbleType { Purple, Green, Blue, Orange, Yellow, Red }
public enum SpawnPoint { Top, Bottom, Left, Right }

[System.Serializable]
public class BubbleData
{
    //TODO:
    //3. make bubbles poppable - improve
    //4. implement score feature - improve
    [SerializeField] GameObject bubbleObj;
    [SerializeField] Transform bubbleTransform;
    [SerializeField] BubbleType bubbleType;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color color;
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int score;
    [SerializeField] int minScore;
    [SerializeField] int maxScore;
    [SerializeField] bool standardSpecs;

    #region Getters
    public GameObject GetGameObject() => bubbleObj;
    public Transform GetBubbleTransform() => bubbleTransform;
    public BubbleType GetBubbleType() => bubbleType;
    public MeshRenderer GetMeshRenderer() => meshRenderer;
    public Color GetColor() => color;
    public ParticleSystem GetParticleSystem() => particles;
    public AudioClip GetAudioClip() => audioClip;
    public AudioSource GetAudioSource() => audioSource;
    public SpawnPoint GetSpawnPoint() => spawnPoint;
    public float GetSpeed() => speed;
    public float GetTimeBetweenSpawns() => timeBetweenSpawns;
    public int GetScore() => score;
    public int GetMinScore() => minScore;
    public int GetMaxScore() => maxScore;
    public bool GetStandardSpecs() => standardSpecs;
    #endregion

    #region Setters
    public void SetGameObject(GameObject newObj) => bubbleObj = newObj;
    public void SetBubbleTransform(Transform newTransform) => bubbleTransform = newTransform;
    public void SetBubbleType(BubbleType newType) => bubbleType = newType;
    public void SetMeshRenderer(MeshRenderer newRenderer) => meshRenderer = newRenderer;
    public void SetColor(Color newColor) => color = newColor;
    public void SetParticleSystem(ParticleSystem newParticleSystem) => particles = newParticleSystem;
    public void SetAudioClip(AudioClip newAudioClip) => audioClip = newAudioClip; 
    public void SetAudioSource(AudioSource newAudioSource) => audioSource = newAudioSource;
    public void SetSpawnPoint(SpawnPoint newSpawnPoint) => spawnPoint = newSpawnPoint;
    public void SetSpeed(float newSpeed) => speed = newSpeed;
    public void SetTimeBetweenSpawns(float newTimeBetweenSpawns) => timeBetweenSpawns = newTimeBetweenSpawns;
    public void SetScore(int newScore) => score = newScore;
    public void SetMinScore(int newMinScore) => score = newMinScore;
    public void SetMaxScore(int newMaxScore) => score = newMaxScore;
    public void SetStandardSpecs(bool isStandard) => standardSpecs = isStandard;
    #endregion

    public BubbleData(
        GameObject bubbleObj, BubbleType bubbleType, Color color, ParticleSystem particles, AudioClip audioClip,
        SpawnPoint spawnPoint, int minScore, int maxScore, float speed = 20f, float timeBetweenSpawns = 2f, bool isStandard = true
        )
    {
        meshRenderer = bubbleObj.GetComponent<MeshRenderer>();
        bubbleTransform = bubbleObj.GetComponent<Transform>();
        audioSource = bubbleObj.GetComponent<AudioSource>();

        this.bubbleObj = bubbleObj;
        this.bubbleType = bubbleType;
        this.color = color;
        this.particles = particles;
        this.audioClip = audioClip;
        this.spawnPoint = spawnPoint;
        this.speed = speed;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.minScore = minScore;
        this.maxScore = maxScore;
        standardSpecs = isStandard;

        BubbleTypeHandler(bubbleType, meshRenderer, color, this, isStandard);

        SpawnPointHandler(spawnPoint, bubbleTransform);
    }

    public void BubbleTypeHandler(BubbleType bubbleType, MeshRenderer meshRenderer, Color color, BubbleData data, bool isStandard)
    {
        switch (bubbleType)
        {
            case BubbleType.Purple:
                if (isStandard)
                {
                    Color purpleHeartColor = new Color(0.5f, 0.14f, 0.75f, 1f); //Purple Heart #8025BE
                    ChooseTypeSpecs(meshRenderer, purpleHeartColor, "Purple Bubble", data, 8, 16);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Purple Bubble", data, data.minScore, data.maxScore);
                break;

            case BubbleType.Green:
                if (isStandard)
                {
                    ChooseTypeSpecs(meshRenderer, Color.green, "Green Bubble", data, 16, 25);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Green Bubble", data, data.minScore, data.maxScore);
                break;

            case BubbleType.Blue:
                if (isStandard)
                {
                    ChooseTypeSpecs(meshRenderer, Color.blue, "Blue Bubble", data, 25, 36);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Blue Bubble", data, data.minScore, data.maxScore);
                break;

            case BubbleType.Orange:
                if (isStandard)
                {
                    Color blazeOrangeColor = new Color(1f, 0.39f, 0f, 1f); //Blaze Orange #FF6400
                    ChooseTypeSpecs(meshRenderer, blazeOrangeColor, "Orange Bubble", data, 37, 49);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Orange Bubble", data, data.minScore, data.maxScore);
                break;

            case BubbleType.Yellow:
                if (isStandard)
                {
                    ChooseTypeSpecs(meshRenderer, Color.yellow, "Yellow Bubble", data, 49, 61);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Yellow Bubble", data, data.minScore, data.maxScore);
                break;

            case BubbleType.Red:
                if (isStandard)
                {
                    ChooseTypeSpecs(meshRenderer, Color.red, "Red Bubble", data, 61, 70);
                    return;
                }
                ChooseTypeSpecs(meshRenderer, color, "Red Bubble", data, data.minScore, data.maxScore);
                break;
        }
    }
    void ChooseTypeSpecs(MeshRenderer meshRenderer, Color color, string objName, BubbleData data, int minScore, int maxScore)
    {
        meshRenderer.material.color = color;
        meshRenderer.gameObject.name = objName;

        data.SetMinScore(minScore);
        data.SetMaxScore(maxScore);
        int score = ChooseRandomScore(minScore, maxScore);

        data.SetScore(score);
    }
    int ChooseRandomScore(int min, int max)
    {
        int newNum;
        newNum = Random.Range(min, max);
        return newNum;
    }

    public void SpawnPointHandler(SpawnPoint spawnPoint, Transform bubbleTransform)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(ScreenDimensions.GetScreenDimensions());

        switch (spawnPoint)
        {
            case SpawnPoint.Top:
                Vector3 topSpawnPoint = new Vector3(Random.Range(-10f, worldPos.x + 10f), worldPos.y + 12f, 0f);
                bubbleTransform.position = topSpawnPoint;
                break;

            case SpawnPoint.Bottom:
                Vector3 bottomSpawnPoint = new Vector3(Random.Range(-10f, worldPos.x + 10f), -10f, 0f);
                bubbleTransform.position = bottomSpawnPoint;
                break;

            case SpawnPoint.Left:
                Vector3 leftSpawnPoint = new Vector3(-10f, Random.Range(-10f, worldPos.y + 10f), 0f);
                bubbleTransform.position = leftSpawnPoint;
                break;

            case SpawnPoint.Right:
                Vector3 rightSpawnPoint = new Vector3(worldPos.x + 10f, Random.Range(-10f, worldPos.y + 10f), 0f);
                bubbleTransform.position = rightSpawnPoint;
                break;
        }
    }
    public static Vector2 CalculateBubbleDirection(Vector2 startPos, float speed, SpawnPoint spawnPoint)
    {
        Vector2 dirToMove = Vector2.zero;
        Vector2 cutScreenDimensions = Camera.main.ScreenToWorldPoint(ScreenDimensions.GetScreenDimensions(2, 2));
        float cutWidth = cutScreenDimensions.x;
        float cutHeight = cutScreenDimensions.y;
        //Top----------------------------------------------------------------------------------
        if (startPos.x < cutWidth && spawnPoint == SpawnPoint.Top)
        {
            dirToMove = MoveDir("SE", speed);
            return dirToMove;
        }
        else if (startPos.x > cutWidth && spawnPoint == SpawnPoint.Top)
        {
            dirToMove = MoveDir("SW", speed);
            return dirToMove;
        }
        //Bottom-------------------------------------------------------------------------------
        if (startPos.x < cutWidth && spawnPoint == SpawnPoint.Bottom)
        {
            dirToMove = MoveDir("NE", speed);
            return dirToMove;
        }
        else if (startPos.x > cutWidth && spawnPoint == SpawnPoint.Bottom)
        {
            dirToMove = MoveDir("NW", speed);
            return dirToMove;
        }
        //Left---------------------------------------------------------------------------------
        if (spawnPoint == SpawnPoint.Left && startPos.y < cutHeight)
        {
            dirToMove = MoveDir("NE", speed);
            return dirToMove;
        }
        else if (spawnPoint == SpawnPoint.Left && startPos.y > cutHeight)
        {
            dirToMove = MoveDir("SE", speed);
            return dirToMove;
        }
        //Right-------------------------------------------------------------------------------
        if (spawnPoint == SpawnPoint.Right && startPos.y < cutHeight)
        {
            dirToMove = MoveDir("NW", speed);
            return dirToMove;
        }
        else if (spawnPoint == SpawnPoint.Right && startPos.y > cutHeight)
        {
            dirToMove = MoveDir("SW", speed);
            return dirToMove;
        }
        return dirToMove;
    }
    static Vector2 MoveDir(string writeDir, float speed)
    {
        Vector2 dirToMove;
        switch (writeDir)
        {
            case "NW":
                dirToMove = new Vector2(-speed, speed);
                return dirToMove;

            case "NE":
                dirToMove = new Vector2(speed, speed);
                return dirToMove;

            case "SW":
                dirToMove = new Vector2(-speed, -speed);
                return dirToMove;

            case "SE":
                dirToMove = new Vector2(speed, -speed);
                return dirToMove;

        }
        return Vector2.zero;
    }
}