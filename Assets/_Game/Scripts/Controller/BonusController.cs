using System;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    public static BonusController Instance;
    BubbleController bubbleController;
    UiController uiController;

    public static Action OnStartBonus;
    public static bool isBonus;
    public static bool endBonus;

    [SerializeField] float timeScaleSpeed;

    [Tooltip("Level time divided by this number = bonus time")]
    public float bonusTime = 10f;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        OnStartBonus += OnStartBonusHandler;    
    }
    void Start()
    {
        bubbleController = FindObjectOfType<BubbleController>();
        uiController = FindObjectOfType<UiController>();
    }
    void Update()
    {
        Time.timeScale = timeScaleSpeed;
    }
    void OnStartBonusHandler()
    {
        foreach (BubbleData bubble in bubbleController.GetBubbles())
        {
            bubble.SetSpawnPoint(SpawnPoint.Middle);
            bubble.GetGameObject().GetComponent<Bubble>().SetMoveDir(Vector2.zero);
            bubble.GetGameObject().SetActive(true);

            if (isBonus) continue;
            BubbleController.ResetAllBubblePosition(bubbleController.GetBubbles());
        }
        isBonus = true;
    }
}
