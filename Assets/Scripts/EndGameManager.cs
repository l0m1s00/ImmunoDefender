using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EndGameManagerUI endGameUI_ref;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    //EVENTOS
    public static event Action OnReturnToMainMenu;
    public static event Action OnRestartGame;

    private void Awake()
    {
        restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnEnable()
    {
        GameManager.OnEndGame += DisplayGameOverScreen;
    }

    private void OnDisable()
    {
        GameManager.OnEndGame -= DisplayGameOverScreen;
    }

    private void OnMainMenuButtonClicked()
    {
        OnReturnToMainMenu?.Invoke();
    }

    private void OnRestartGameButtonClicked()
    {
        OnRestartGame?.Invoke();
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void DisplayGameOverScreen()
    {
        endGameUI_ref.DisplayEndGameScreen();
    }
}
