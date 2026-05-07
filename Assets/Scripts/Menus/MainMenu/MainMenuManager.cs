using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    //public static MainMenuManager Instance;

    //[Header("Components")]
    //[SerializeField] private Button closeMenuButton;
    //[SerializeField] private Button selectDifficultyButton;
    

    ////EVENTOS
    //public static event Action OnCloseMenu;
    //public static event Action OnLoadGameplay;

    //private void Awake()
    //{

    //    closeMenuButton.onClick.AddListener(OnCloseMenuButtonClicked);
    //}

    //private void Start()
    //{
    //    mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
    //}

    //private void OnStartButtonClicked()
    //{
    //    if (isDifficultySelected) OnLoadGameplay?.Invoke();
    //}

    //private void OnCloseMenuButtonClicked()
    //{
    //    isDifficultySelected = false;
    //    OnCloseMenu?.Invoke();
    //    mainMenuUIRef.DisplayDifficultyPanel(false);
    //    mainMenuUIRef.DisplaySelectDifficultyButton(selectDifficultyButton.gameObject);
    //    mainMenuUIRef.DisplaySelectDifficultyButton(exitButton.gameObject);
    //    mainMenuUIRef.DisplaySelectDifficultyButton(settingsButton.gameObject);
    //}

    //private void OnSelectDifficultyButtonClicked()
    //{
    //    mainMenuUIRef.DisplayDifficultyPanel(true);
    //    mainMenuUIRef.HideSelectDifficultyButton(selectDifficultyButton.gameObject);
    //    mainMenuUIRef.HideSelectDifficultyButton(exitButton.gameObject);
    //    mainMenuUIRef.HideSelectDifficultyButton(settingsButton.gameObject);
    //}


    //public void SetNewSelectedDifficulty(DifficultySettingsData newSettingsData)
    //{
    //    mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
    //}

    //public void DeselectDifficulty()
    //{
    //    isDifficultySelected = false;
    //    mainMenuUIRef.SetButtonColorAlpha(startGameButton.gameObject.GetComponent<Image>(), isDifficultySelected);
    //}
}
