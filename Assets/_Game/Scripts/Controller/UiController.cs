using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] Slider levelTimeSlider;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] float sliderTimeSpeed = 1f;

    public Slider GetLevelTimeSlider() => levelTimeSlider;
    public TMP_Text GetScoreText() => scoreText;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        InitSlider(levelTimeSlider);
        UpdateScoreText(scoreText, playerController);
    }

    float time = 0;
    void Update()
    {
        if (BubbleController.GetGameStarted())
        {
            LevelController.OnWinLevel.Invoke(levelTimeSlider, LevelController.GetLevelTime());
            UpdateSlider(levelTimeSlider, sliderTimeSpeed);
            UpdateScoreText(scoreText, playerController);
        }
    }
    void InitSlider(Slider slider)
    {
        slider.maxValue = LevelController.GetLevelTime();
    }

    void UpdateSlider(Slider slider, float speed)
    {
        time = Time.deltaTime;
        slider.value += speed * time;
    }
    void UpdateScoreText(TMP_Text scoreText, PlayerController playerController)
    {
        scoreText.text = PlayerController.GetScore().ToString();
    }
}