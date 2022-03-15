using UnityEngine;

public enum BubbleType { Purple, Green, Blue, Orange, Yellow, Red }
public enum SpawnPoint { Top, Bottom, Left, Right }

[System.Serializable]
public class BubbleData
{
    //TODO:
    //1. change colors through editor
    //2. implement object pooling
    //3. make bubbles poppable
    //4. implement score feature
    [SerializeField] GameObject bubbleObj;
    [SerializeField] Transform bubbleTransform;
    [SerializeField] BubbleType bubbleType;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color color;
    [SerializeField] ParticleSystem particles;
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int score;

    #region Getters
    public GameObject GetGameObject() => bubbleObj;
    public Transform GetBubbleTransform() => bubbleTransform;
    public BubbleType GetBubbleType() => bubbleType;
    public MeshRenderer GetMeshRenderer() => meshRenderer;
    public Color GetColor() => color;
    public ParticleSystem GetParticleSystem() => particles;
    public SpawnPoint GetSpawnPoint() => spawnPoint;
    public float GetSpeed() => speed;
    public float GetTimeBetweenSpawns() => timeBetweenSpawns;
    public int GetScore() => score;
    #endregion

    #region Setters
    public void SetGameObject(GameObject newObj) => bubbleObj = newObj;
    public void SetBubbleTransform(Transform newTransform) => bubbleTransform = newTransform;
    public void SetBubbleType(BubbleType newType) => bubbleType = newType;
    public void SetMeshRenderer(MeshRenderer newRenderer) => meshRenderer = newRenderer;
    public void SetColor(Color newColor) => color = newColor;
    public void SetParticleSystem(ParticleSystem newParticleSystem) => particles = newParticleSystem;
    public void SetSpawnPoint(SpawnPoint newSpawnPoint) => spawnPoint = newSpawnPoint;
    public void SetSpeed(float newSpeed) => speed = newSpeed;
    public void SetTimeBetweenSpawns(float newTimeBetweenSpawns) => timeBetweenSpawns = newTimeBetweenSpawns;
    public void SetScore(int newScore) => score = newScore;
    #endregion

    public BubbleData(GameObject bubbleObj, BubbleType bubbleType, ParticleSystem particles,
        SpawnPoint spawnPoint, float speed = 2f, float timeBetweenSpawns = 2f, int score = 0)
    {
        meshRenderer = bubbleObj.GetComponent<MeshRenderer>();
        bubbleTransform = bubbleObj.GetComponent<Transform>();

        this.bubbleObj = bubbleObj;
        this.bubbleType = bubbleType;
        this.particles = particles;
        this.spawnPoint = spawnPoint;
        this.speed = speed;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.score = score;

        BubbleTypeHandler(bubbleType, meshRenderer,this);

        SpawnPointHandler(spawnPoint, bubbleTransform);
    }

    void BubbleTypeHandler(BubbleType bubbleType, MeshRenderer meshRenderer, BubbleData data)
    {
        switch (bubbleType)
        {
            case BubbleType.Purple:
                Color purpleHeartColor = new Color(0.5f, 0.14f, 0.75f, 1f); //Purple Heart #8025BE
                meshRenderer.material.color = purpleHeartColor; 
                meshRenderer.gameObject.name = $"Purple Bubble";
                data.SetScore(Random.Range(8,16));
                break;

            case BubbleType.Green:
                meshRenderer.material.color = Color.green;
                meshRenderer.gameObject.name = $"Green Bubble";
                data.SetScore(Random.Range(16,25));
                break;

            case BubbleType.Blue:
                meshRenderer.material.color = Color.blue;
                meshRenderer.gameObject.name = $"Blue Bubble";
                data.SetScore(Random.Range(25,37));
                break;

            case BubbleType.Orange:
                Color sunColor = new Color(1f, 0.39f, 0f, 1f); //Blaze Orange #FF6400
                meshRenderer.material.color = sunColor; 
                meshRenderer.gameObject.name = $"Orange Bubble";
                data.SetScore(Random.Range(37,49));
                break;

            case BubbleType.Yellow:
                meshRenderer.material.color = Color.yellow;
                meshRenderer.gameObject.name = $"Yellow Bubble";
                data.SetScore(Random.Range(49,61));
                break;

            case BubbleType.Red:
                meshRenderer.material.color = Color.red;
                meshRenderer.gameObject.name = $"Red Bubble";
                data.SetScore(Random.Range(61,71));
                break;
        }
    }
    void SpawnPointHandler(SpawnPoint spawnPoint, Transform bubbleTransform)
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