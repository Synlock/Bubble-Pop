using UnityEngine;

public enum PowerUpType {BubbleSizeUp, BubbleSizeDown, SlowBubble, FastBubble, TimeLeftUp, TimeLeftDown, HealthDown, HealthUp }

public class PowerUpData
{
    [SerializeField] PowerUpType powerUpType;
    [SerializeField] BubbleData data;
    [SerializeField] float bubbleSize = 2f;
    [SerializeField] float bubbleSpeed = 2f;
    [SerializeField] float timeModifier = 10f;
    [SerializeField] int healthModifier = 1;

    public PowerUpData(PowerUpType powerUpType, BubbleData data,float bubbleSize, float bubbleSpeed, float timeModifier, int healthModifier)
    {
        this.powerUpType = powerUpType;
        this.data = data;
        this.bubbleSize = bubbleSize;
        this.bubbleSpeed = bubbleSpeed;
        this.timeModifier = timeModifier;
        this.healthModifier = healthModifier;

    }
}
