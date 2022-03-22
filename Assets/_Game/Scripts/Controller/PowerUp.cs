using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    BubbleData data;
    public PowerUpType type;

    [SerializeField] float speed = 50f;
    
    Rigidbody rb;
    Vector2 moveDir;

    private void Awake()
    {
        type = ChooseRandomPowerUpType();
        data = new BubbleData(BubbleController.ChooseRandomSpawnPoint(), gameObject, transform);
        data.SetSpeed(speed);

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        moveDir = BubbleData.CalculateBubbleDirection(data.GetBubbleTransform().position, data.GetSpeed(), data.GetSpawnPoint());
    }
    void FixedUpdate()
    {
        rb.velocity = moveDir * Time.fixedDeltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(TAGS.WALLS_LAYER_NAME))
        {
            Destroy(gameObject);
        }
    }
    PowerUpType ChooseRandomPowerUpType()
    {
        Array powerUpTypes = Enum.GetValues(typeof(PowerUpType));

        System.Random rand = new System.Random();
        PowerUpType type = (PowerUpType)powerUpTypes.GetValue(rand.Next(powerUpTypes.Length));

        return type;
    }
}
