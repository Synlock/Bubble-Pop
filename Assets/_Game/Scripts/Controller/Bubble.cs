using UnityEngine;

public class Bubble : MonoBehaviour
{
    BubbleController bubbleController;
    UiController uiController;
    BubbleData myData;
    Rigidbody rb;
    Vector2 moveDir;

    public BubbleData GetBubbleData() => myData;

    public void SetMoveDir(Vector2 newMoveDir) => moveDir = newMoveDir; 

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        bubbleController = FindObjectOfType<BubbleController>();
        uiController = FindObjectOfType<UiController>();

        InitMoveDirection();
    }

    void InitMoveDirection()
    {
        if (transform == bubbleController.GetBubblesParent().transform.GetChild(0))
        {
            myData = bubbleController.GetBubbles()[0];
            moveDir = BubbleData.CalculateBubbleDirection(myData.GetBubbleTransform().position, myData.GetSpeed(), myData.GetSpawnPoint());
            return;
        }

        for (int i = 0; i < bubbleController.GetBubbles().Count; i++)
        {
            myData = bubbleController.GetBubbles()[i];
            moveDir = BubbleData.CalculateBubbleDirection(myData.GetBubbleTransform().position, myData.GetSpeed(), myData.GetSpawnPoint());

            if (transform == bubbleController.GetBubblesParent().transform.GetChild(i)) break;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDir * Time.fixedDeltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(TAGS.WALLS_LAYER_NAME))
        {
            LevelController.OnLoseLevel.Invoke(uiController.GetLevelTimeSlider());
            LevelController.OnBubbleHitWall.Invoke();
            BubbleController.OnBubblePopped.Invoke(gameObject.transform.position);
            gameObject.SetActive(false);
        }
    }
}