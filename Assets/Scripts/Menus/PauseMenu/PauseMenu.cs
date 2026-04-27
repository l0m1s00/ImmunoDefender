using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeGameButton;
    //[SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    //EVENTOS
    public static event Action OnPauseGame;
    public static event Action OnReturnToMainMenu;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        resumeGameButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnEnable()
    {
        Inputs.OnESCPerformed += OnPauseButtonClicked;
    }

    private void OnDisable()
    {
        Inputs.OnESCPerformed -= OnPauseButtonClicked;
    }

    private void OnMainMenuButtonClicked()
    {
        OnReturnToMainMenu?.Invoke();
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnPauseButtonClicked()
    {
        OnPauseGame?.Invoke();
    }
}
