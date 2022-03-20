using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    PlayerController playerController;
    BubbleController bubbleController;

    [SerializeField] Slider levelTimeSlider;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] float sliderTimeSpeed = 1f;
    [SerializeField] float levelTime = 30f;

    bool levelEnded = false;

    public static Action OnEndLevel;

    void Awake()
    {
        OnEndLevel += OnEndLevelHandler;

        playerController = FindObjectOfType<PlayerController>();
        bubbleController = FindObjectOfType<BubbleController>();
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
            OnEndLevel.Invoke();
            UpdateSlider(levelTimeSlider, sliderTimeSpeed);
            UpdateScoreText(scoreText, playerController);
        }
    }
    void InitSlider(Slider slider)
    {
        slider.maxValue = levelTime;
    }

    void UpdateSlider(Slider slider, float speed)
    {
        time = Time.deltaTime;
        slider.value += speed * time;
    }
    void UpdateScoreText(TMP_Text scoreText, PlayerController playerController)
    {
        scoreText.text = playerController.GetScore().ToString();
    }

    void OnEndLevelHandler()
    {
        if (levelTimeSlider.value >= levelTime)
        {
            levelEnded = true;
            bubbleController.ResetBubbleController();

            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);

            if (levelEnded)
            {
                levelTimeSlider.value = 0f;
                playerController.SetScore(0);
                levelEnded = false;
                return;
            }
        }
    }
}