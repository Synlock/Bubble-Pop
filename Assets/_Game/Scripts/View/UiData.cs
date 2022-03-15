using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
class UiData
{
    Slider levelSlider;
    TMP_Text scoreText;

    public UiData(Slider levelSlider, TMP_Text scoreText)
    {
        this.levelSlider = levelSlider;
        this.scoreText = scoreText;
    }
}
