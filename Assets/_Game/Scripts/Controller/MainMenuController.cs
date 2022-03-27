using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    PlayerData playerData = new PlayerData();

    [SerializeField] Button nextLvlBtn;

    [SerializeField] Button settingsBtn;
    [SerializeField] Button vibrationBtn;
    [SerializeField] Button soundBtn;

    [SerializeField] Button leaderboardBtn;

    [SerializeField] Button inventoryBtn;

    [SerializeField] Image settingsPanel;
    [SerializeField] Image inventoryPanel;

    [SerializeField] Image coinsImage;
    [SerializeField] TMP_Text coinsText;

    Action OnNextLevelPressed;
    Action OnSettingsPressed;
    Action OnVibrationPressed;
    Action OnSoundPressed;
    Action OnLeaderboardsPressed;
    Action OnInventoryPressed;

    bool inventoryOpen = false;
    bool settingsOpen = false;
    bool vibrateOn = true;
    bool soundOn = true;

    void Awake()
    {
        OnNextLevelPressed += OnNextLevelPressedHandler;
        OnSettingsPressed += OnSettingsPressedHandler;
        OnVibrationPressed += OnVibrationPressedHandler;
        OnSoundPressed += OnSoundPressedHandler;
        OnLeaderboardsPressed += OnLeaderboardsPressedHandler;
        OnInventoryPressed += OnInventoryPressedHandler;
    }
    void Start()
    {
        PlayerData.LoadPlayerData(playerData);

        InitMainMenuButtons();
        InitCoinsText();
    }

    void InitMainMenuButtons()
    {
        if (nextLvlBtn != null)
            nextLvlBtn.onClick.AddListener(() =>
            {
                OnNextLevelPressed.Invoke();
            });

        if (settingsBtn != null)
            settingsBtn.onClick.AddListener(() =>
            {
                OnSettingsPressed.Invoke();
            });
        if (vibrationBtn != null)
            vibrationBtn.onClick.AddListener(() =>
            {
                OnVibrationPressed.Invoke();
            });
        if (soundBtn != null)
            soundBtn.onClick.AddListener(() =>
            {
                OnSoundPressed.Invoke();
            });

        if (leaderboardBtn != null)
            leaderboardBtn.onClick.AddListener(() =>
            {
                OnLeaderboardsPressed.Invoke();
            });

        if (inventoryBtn != null)
            inventoryBtn.onClick.AddListener(() =>
            {
                OnInventoryPressed.Invoke();
            });
    }
    void InitCoinsText()
    {
        coinsText.text = playerData.coins.ToString();
    }

    void OnNextLevelPressedHandler()
    {
        int currentLevel = LevelController.GetPlayerData().GetLevel();
        LevelController.OnNextLevel.Invoke(currentLevel);

        BubbleController.OnInitBubbleController.Invoke();
        UiController.OnLoadUI.Invoke();
        SceneManager.UnloadSceneAsync(TAGS.MAIN_MENU_SCENE_NAME);
    }

    void OnSettingsPressedHandler()
    {
        settingsOpen = !settingsOpen;

        if (settingsOpen)
            settingsPanel.gameObject.SetActive(true);
        else settingsPanel.gameObject.SetActive(false);
    }
    void OnVibrationPressedHandler()
    {
        vibrateOn = !vibrateOn;
        
        if (vibrateOn)
        {
            vibrationBtn.image.color = Color.green;
            //insert vibrate on here
        }
        else
        {
            vibrationBtn.image.color = Color.red;
            //insert vibrate off here
        }
    }
    void OnSoundPressedHandler()
    {
        soundOn = !soundOn;

        if (soundOn)
        {
            soundBtn.image.color = Color.green;
            //insert sound on here
        }
        else
        {
            soundBtn.image.color = Color.red;
            //insert sound off here
        }
    }
    void OnLeaderboardsPressedHandler()
    {

    }

    void OnInventoryPressedHandler()
    {
        inventoryOpen = !inventoryOpen;

        if (inventoryOpen)
            inventoryPanel.gameObject.SetActive(true);
        else inventoryPanel.gameObject.SetActive(false);
    }
}