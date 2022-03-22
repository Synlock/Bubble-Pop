using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    List<BubbleData> bubbles;
    [SerializeField] float timeToSpawn = 10f;
    float timer = 0f;

    [SerializeField] GameObject powerUpPrefab;
    [SerializeField] float bubbleSize = 2f;
    [SerializeField] float bubbleSpeed = 2f;
    [SerializeField] float timeModifier = 10f;
    [SerializeField] int healthModifier = 1;

    void Start()
    {
        timer = timeToSpawn;
        bubbles = BubbleController.Instance.GetBubbles();
    }

    void Update()
    {
        PowerUpSpawner();
    }

    void PowerUpSpawner()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject powerUp = Instantiate(powerUpPrefab);
            PowerUpTypeController(bubbles, powerUp.GetComponent<PowerUp>().type);
            timer = timeToSpawn;
        }
    }
    public void PowerUpTypeController(List<BubbleData> bubbles, PowerUpType powerUpType)
    {
        foreach (BubbleData bubble in bubbles)
        {
            switch (powerUpType)
            {
                case PowerUpType.BubbleSizeUp:
                    bubble.GetGameObject().transform.localScale *= bubbleSize;
                    break;

                case PowerUpType.BubbleSizeDown:
                    bubble.GetGameObject().transform.localScale /= bubbleSize;
                    break;

                case PowerUpType.SlowBubble:
                    bubble.SetSpeed(bubble.GetSpeed() / bubbleSpeed);
                    break;

                case PowerUpType.FastBubble:
                    bubble.SetSpeed(bubble.GetSpeed() * bubbleSpeed);
                    break;

                case PowerUpType.TimeLeftUp:
                    UiController.Instance.GetLevelTimeSlider().value += timeModifier; 
                    break;

                case PowerUpType.TimeLeftDown:
                    UiController.Instance.GetLevelTimeSlider().value -= timeModifier;
                    break;

                case PowerUpType.HealthDown:
                    LevelController.SetHitPoints(LevelController.GetHitPoints()-healthModifier);
                    break;

                case PowerUpType.HealthUp:
                    LevelController.SetHitPoints(LevelController.GetHitPoints()+healthModifier);
                    break;
            }
        }
    }
}