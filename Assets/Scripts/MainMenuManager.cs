using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUIRef;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button selectDifficultyButton;
    [SerializeField] private Button smallDifficultyButton;
    [SerializeField] private Button normalDifficultyButton;
    [SerializeField] private Button largeDifficultyButton;
    [SerializeField] private DifficultySettingsData smallDifficultyData;
    [SerializeField] private DifficultySettingsData normalDifficultyData;
    [SerializeField] private DifficultySettingsData largeDifficultyData;
    [SerializeField] private DifficultySettingsData currentDifficultyData;
    private bool isDifficultySelected = false;

    private void Awake()
    {
        startGameButton.onClick.AddListener(OnStartButtonClicked);
        selectDifficultyButton.onClick.AddListener(OnSelectDifficultyButtonClicked);
        smallDifficultyButton.onClick.AddListener(OnSmallDifficultyButtonClicked);
        normalDifficultyButton.onClick.AddListener(OnNormalDifficultyButtonClicked);
        largeDifficultyButton.onClick.AddListener(OnLargeDifficultyButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        if (isDifficultySelected) GameSceneManager.Instance.LoadGameplay();
    }

    private void OnSelectDifficultyButtonClicked()
    {
        mainMenuUIRef.HideSelectDifficultyButton(selectDifficultyButton.gameObject);
        mainMenuUIRef.ShowDifficultyPanel();
    }

    private void OnNormalDifficultyButtonClicked()
    {
        ChangeDifficultyValuesSO(normalDifficultyData);
        mainMenuUIRef.ShowSelectedDifficultyOutline(normalDifficultyButton.gameObject);
        isDifficultySelected = true;
    }

    private void OnSmallDifficultyButtonClicked()
    {
        ChangeDifficultyValuesSO(smallDifficultyData);
        mainMenuUIRef.ShowSelectedDifficultyOutline(smallDifficultyButton.gameObject);
        isDifficultySelected = true;
    }

    private void OnLargeDifficultyButtonClicked()
    {
        ChangeDifficultyValuesSO(largeDifficultyData);
        mainMenuUIRef.ShowSelectedDifficultyOutline(largeDifficultyButton.gameObject);
        isDifficultySelected = true;
    }

    private void ChangeDifficultyValuesSO(DifficultySettingsData newSettingsData)
    {
        currentDifficultyData.width = newSettingsData.width;
        currentDifficultyData.height = newSettingsData.height;
        currentDifficultyData.groundPercentage = newSettingsData.groundPercentage;
        currentDifficultyData.minPathSize = newSettingsData.minPathSize;
        currentDifficultyData.initialCurrency = newSettingsData.initialCurrency;
    }
}
