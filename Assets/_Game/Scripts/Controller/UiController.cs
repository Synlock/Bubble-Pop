using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] float sliderTimeSpeed = 1f;
    [SerializeField] float sliderMaxTime = 30f;

    PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        InitSlider(slider);
        UpdateScoreText(scoreText, playerController);
    }

    void Update()
    {
        UpdateSlider(slider,sliderTimeSpeed);
        UpdateScoreText(scoreText, playerController);
    }
    void InitSlider(Slider slider)
    {
        slider.maxValue = sliderMaxTime;
    }
    void UpdateSlider(Slider slider, float speed)
    {
        slider.value = speed * Time.time;
    }

    void UpdateScoreText(TMP_Text scoreText, PlayerController playerController)
    {
        scoreText.text = playerController.GetScore().ToString();
    }
}
