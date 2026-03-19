using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    private const string MAIN_MENU_SCENE = "MainMenu";
    private const string IN_GAME_SCENE = "Gameplay";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync(MAIN_MENU_SCENE, GameManager.GameState.MainMenu));
    }

    public void LoadGameplay()
    {
        StartCoroutine(LoadSceneAsync(IN_GAME_SCENE, GameManager.GameState.Playing));
    }

    //public void RestartCurrentScene()
    //{

    //}

    private IEnumerator LoadSceneAsync(string sceneName, GameManager.GameState newGameState)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        GameManager.Instance.SetGameState(newGameState);
    }
}
