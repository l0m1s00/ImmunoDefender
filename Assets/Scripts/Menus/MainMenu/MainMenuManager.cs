using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [Header("Components")]
    [SerializeField] private DifficultiesPanelUI mainMenuUIRef;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button closeMenuButton;
    [SerializeField] private Button selectDifficultyButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;
    [Header("Difficulty")]
    [SerializeField] private DifficultySettingsData currentDifficultyData;
    private bool isDifficultySelected = false;

    //EVENTOS
    public static event Action OnCloseMenu;
    public static event Action OnLoadGameplay;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        startGameButton.onClick.AddListener(OnStartButtonClicked);
        selectDifficultyButton.onClick.AddListener(OnSelectDifficultyButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        closeMenuButton.onClick.AddListener(OnCloseMenuButtonClicked);
    }

    private void Start()
    {
        mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
    }

    private void OnStartButtonClicked()
    {
        if (isDifficultySelected) OnLoadGameplay?.Invoke();
    }

    private void OnCloseMenuButtonClicked()
    {
        isDifficultySelected = false;
        OnCloseMenu?.Invoke();
        mainMenuUIRef.DisplayDifficultyPanel(false);
        mainMenuUIRef.DisplaySelectDifficultyButton(selectDifficultyButton.gameObject);
        mainMenuUIRef.DisplaySelectDifficultyButton(exitButton.gameObject);
        mainMenuUIRef.DisplaySelectDifficultyButton(settingsButton.gameObject);
    }

    private void OnSelectDifficultyButtonClicked()
    {
        mainMenuUIRef.DisplayDifficultyPanel(true);
        mainMenuUIRef.HideSelectDifficultyButton(selectDifficultyButton.gameObject);
        mainMenuUIRef.HideSelectDifficultyButton(exitButton.gameObject);
        mainMenuUIRef.HideSelectDifficultyButton(settingsButton.gameObject);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void ChangeDifficultyValuesSO(DifficultySettingsData newSettingsData)
    {
        currentDifficultyData.width = newSettingsData.width;
        currentDifficultyData.height = newSettingsData.height;
        currentDifficultyData.groundPercentage = newSettingsData.groundPercentage;
        currentDifficultyData.minPathSize = newSettingsData.minPathSize;
        currentDifficultyData.initialCurrency = newSettingsData.initialCurrency;
    }

    public void SetNewSelectedDifficulty(DifficultySettingsData newSettingsData)
    {
        isDifficultySelected = true;
        mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
        ChangeDifficultyValuesSO(newSettingsData);
    }

    public void DeselectDifficulty()
    {
        isDifficultySelected = false;
        mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
    }
}
