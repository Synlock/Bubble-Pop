using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button nextLvlBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button leaderboardBtn;
    [SerializeField] Button inventoryBtn;

    [SerializeField] Image inventoryPanel;

    [SerializeField] Image coinsImage;
    [SerializeField] TMP_Text coinsText;

    Action OnNextLevelPressed;
    Action OnSettingsPressed;
    Action OnLeaderboardsPressed;
    Action OnInventoryPressed;

    void Awake()
    {
        OnNextLevelPressed += OnNextLevelPressedHandler;
        OnSettingsPressed += OnSettingsPressedHandler;
        OnLeaderboardsPressed += OnLeaderboardsPressedHandler;
        OnInventoryPressed += OnInventoryPressedHandler;
    }
    void Start()
    {
        InitMainMenuButtons();
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

    void OnNextLevelPressedHandler()
    {
        int currentLevel = LevelController.GetPlayerData().GetLevel();
        LevelController.OnNextLevel.Invoke(currentLevel);

        BubbleController.OnInitBubbleController.Invoke();
        UiController.OnLoadUI.Invoke();
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    void OnSettingsPressedHandler()
    {

    }
    void OnLeaderboardsPressedHandler()
    {

    }

    bool inventoryOpen = false;
    void OnInventoryPressedHandler()
    {
        inventoryOpen = !inventoryOpen;

        if (inventoryOpen)
            inventoryPanel.gameObject.SetActive(true);
        else inventoryPanel.gameObject.SetActive(false);

        
    }
}