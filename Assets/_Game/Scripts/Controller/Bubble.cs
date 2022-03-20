using UnityEngine;

public class Bubble : MonoBehaviour
{
    BubbleController controller;
    BubbleData myData;
    Rigidbody rb;
    Vector2 moveDir;

    public BubbleData GetBubbleData() => myData;

    public void SetMoveDir(Vector2 newMoveDir) => moveDir = newMoveDir; 

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        controller = FindObjectOfType<BubbleController>();

        InitMoveDirection();
    }

    void InitMoveDirection()
    {
        if (transform == controller.GetBubblesParent().transform.GetChild(0))
        {
            myData = controller.GetBubbles()[0];
            moveDir = BubbleData.CalculateBubbleDirection(myData.GetBubbleTransform().position, myData.GetSpeed(), myData.GetSpawnPoint());
            return;
        }

        for (int i = 0; i < controller.GetBubbles().Count; i++)
        {
            myData = controller.GetBubbles()[i];
            moveDir = BubbleData.CalculateBubbleDirection(myData.GetBubbleTransform().position, myData.GetSpeed(), myData.GetSpawnPoint());

            if (transform == controller.GetBubblesParent().transform.GetChild(i)) break;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDir * Time.fixedDeltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            //reduce life here on hit
        }
    }
}