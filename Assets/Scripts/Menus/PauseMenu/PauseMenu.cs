using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;
    //private PauseMenuUI pauseMenuUIRef;

    //EVENTOS
    public static event Action OnPauseGame;
    public static event Action OnReturnToMainMenu;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
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
