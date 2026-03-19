using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, GameOver, Victory }
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

    private void ChangeGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        HandleStates();
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
        Time.timeScale = 1f;
    }

    private void PlayingActions()
    {
        Time.timeScale = 1f;
    }

    private void PausedActions()
    {
        Time.timeScale = 0f;
    }

    private void GameOverActions()
    {
        Time.timeScale = 0f;
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    public void SetGameState(GameState newState)
    {
        ChangeGameState(newState);
    }
}
