using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, Resume, GameOver, Victory }
    private GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        currentState = GameState.MainMenu;
    }

    private void OnEnable()
    {
        PauseMenu.OnPauseGame += PauseGame;
        PauseMenu.OnReturnToMainMenu += LoadMainMenu;
        MainMenuManager.OnLoadGameplay += LoadGameplay;
    }

    private void OnDisable()
    {
        PauseMenu.OnPauseGame -= PauseGame;
        PauseMenu.OnReturnToMainMenu -= LoadMainMenu;
        MainMenuManager.OnLoadGameplay -= LoadGameplay;
    }

    private void ChangeGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        //HandleStates();
    }

    private void HandleStates()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                MainMenuActions();
                break;
            case GameState.Playing:
                PlayingActions();
                break;
            case GameState.Paused:
                PausedActions();
                break;
            case GameState.GameOver:
                GameOverActions();
                break;
            case GameState.Victory:
                break;
        }
    }

    private void MainMenuActions()
    {
        LoadMainMenu();
    }

    private void PlayingActions()
    {
        LoadGameplay();
    }

    private void PausedActions()
    {
        PauseGame();
    }

    private void GameOverActions()
    {
        Time.timeScale = 0f;
    }

    private void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            ChangeGameState(GameState.Paused);
            Time.timeScale = 0f;
        }
        else
        {
            ChangeGameState(GameState.Playing);
            Time.timeScale = 1f;
        }
    }

    private void LoadGameplay()
    {
        ChangeGameState(GameState.Playing);
        Time.timeScale = 1f;
        GameSceneManager.Instance.LoadGameplay();
    }

    private void LoadMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
        Time.timeScale = 1f;
        GameSceneManager.Instance.LoadMainMenu();
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    //public void SetGameState(GameState newState)
    //{
    //    ChangeGameState(newState);
    //}
}
