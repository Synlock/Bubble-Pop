using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync(TAGS.LEVEL_SCENE_NAME);
        SceneManager.LoadSceneAsync(TAGS.MAIN_MENU_SCENE_NAME, LoadSceneMode.Additive);
    }
}
