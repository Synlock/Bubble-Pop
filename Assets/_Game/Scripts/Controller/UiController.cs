using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiController : MonoBehaviour
{
    public static UiController Instance;
    PlayerController playerController;

    [SerializeField] Slider levelTimeSlider;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] float sliderTimeSpeed = 1f;

    public Slider GetLevelTimeSlider() => levelTimeSlider;
    public TMP_Text GetScoreText() => scoreText;

    public static Action OnLoadUI;
    public static Action OnDisableUI;

    void Awake()
    {
        Instance = this;
        playerController = FindObjectOfType<PlayerController>();

        OnLoadUI += OnLoadUIHandler;
        OnDisableUI += OnDisableUIHandler;
    }

    float time = 0;
    void Start()
    {
        OnDisableUI.Invoke();
    }
    void Update()
    {
        if (BubbleController.GetGameStarted())
        {
            if (BonusController.isBonus)
                UpdateBonusSlider(levelTimeSlider, sliderTimeSpeed);
            else
                UpdateSlider(levelTimeSlider, sliderTimeSpeed);

            UpdateScoreText(scoreText);

            LevelController.OnWinLevel.Invoke(levelTimeSlider, LevelController.GetLevelTime());
        }
    }
    void InitSlider(Slider slider)
    {
        slider.gameObject.SetActive(true);
        slider.maxValue = LevelController.GetLevelTime();
    }

    void UpdateSlider(Slider slider, float speed)
    {
        time = Time.deltaTime;
        slider.value += speed * time;
    }
    void UpdateBonusSlider(Slider slider, float speed)
    {
        time = Time.deltaTime;
        slider.value -= speed * time;
    }
    void UpdateScoreText(TMP_Text scoreText)
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = PlayerController.GetScore().ToString();
    }

    void OnLoadUIHandler()
    {
        InitSlider(levelTimeSlider);
        UpdateScoreText(scoreText);
    }
    void OnDisableUIHandler()
    {
        levelTimeSlider.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }
}