using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, Resume, GameOver }
    private GameState currentState;

    //EVENTOS
    public static event Action OnSceneTransition;
    public static event Action OnEndGame;

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
        EndGameManager.OnReturnToMainMenu += LoadMainMenu;
        EndGameManager.OnRestartGame += LoadGameplay;
        DifficultiesMenu.OnLoadGameplay += DoSceneTransition;
        SceneTransition.OnTransitionCompleted += LoadGameplay;
        LevelManager.OnEndGame += EndGame;
    }

    private void OnDisable()
    {
        PauseMenu.OnPauseGame -= PauseGame;
        PauseMenu.OnReturnToMainMenu -= LoadMainMenu;
        EndGameManager.OnReturnToMainMenu -= LoadMainMenu;
        EndGameManager.OnRestartGame -= LoadGameplay;
        DifficultiesMenu.OnLoadGameplay -= DoSceneTransition;
        SceneTransition.OnTransitionCompleted -= LoadGameplay;
        LevelManager.OnEndGame -= EndGame;
    }

    private void ChangeGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    private void PauseGame(bool isPaused)
    {
        if (isPaused)
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

    private void DoSceneTransition()
    {
        OnSceneTransition?.Invoke();
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

    public void EndGame()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        OnEndGame?.Invoke();
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }
}
