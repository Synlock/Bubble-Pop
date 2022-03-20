using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button nextLvlBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button leaderboardBtn;

    Action OnNextLevelPressed;
    Action OnSettingsPressed;
    Action OnLeaderboardsPressed;

    void Awake()
    {
        OnNextLevelPressed += OnNextLevelPressedHandler;
        OnSettingsPressed += OnSettingsPressedHandler;
        OnLeaderboardsPressed += OnLeaderboardsPressedHandler;
    }
    void Start()
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
    }
    //TODO: pull current level from player data
    void OnNextLevelPressedHandler()
    {
        //temp
        LevelController.OnNextLevel.Invoke(LevelController.GetCurrentLevel() - 1);
        LevelController.SetCurrentLevel(LevelController.GetCurrentLevel() + 1);

        BubbleController.OnInitBubbleController.Invoke();
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    void OnSettingsPressedHandler()
    {

    }
    void OnLeaderboardsPressedHandler()
    {

    }
}