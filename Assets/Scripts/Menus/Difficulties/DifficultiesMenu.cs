using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultiesMenu : BaseMenu
{
    public static DifficultiesMenu Instance;

    [Header("Difficulty Types")]
    [SerializeField] private List<DifficultySettingsData> difficultyTypes;
    [SerializeField] private DifficultySettingsData currentDifficultyData;
    [Header("UI References")]
    [SerializeField] private Transform difficultyPanelsContainer;
    [SerializeField] private List<Transform> difficultyPanelHolders;
    [SerializeField] private GameObject difficultyPanelPrefab;
    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [Header("Audios")]
    [SerializeField] private AudioEventData startGameplayEventData;

    private DifficultiesMenuUI difficultyMenuUI_Ref;
    private List<Difficulty> difficulties = new List<Difficulty>();
    private int currentSelectedDifficultyIndex = -1; //-1 nada seleccionado

    //EVENTOS
    public static event Action OnLoadGameplay;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        FormsMenu.OnDisplayDifficulties += Open;
    }

    private void OnDisable()
    {
        FormsMenu.OnDisplayDifficulties -= Open;
    }

    protected override void Start()
    {
        base.Start();
        CreateDifficulties();
        startGameButton.onClick.AddListener(StartGame);
        difficultyMenuUI_Ref = (DifficultiesMenuUI)baseMenuUI_Ref;
    }

    protected override void Open()
    {
        base.Open();
    }

    protected override void Close()
    {
        base.Close();
    }

    private void StartGame()
    {
        if (currentSelectedDifficultyIndex != -1)
        {
            AudioManager.Instance.PlaySFX(startGameplayEventData, this.transform.position, 0f);
            OnLoadGameplay?.Invoke();
        }
    }

    private void CreateDifficulties()
    {
        GameObject newDifficulty;
        Difficulty difficultyLogic;

        for (int i = 0; i < difficultyTypes.Count; i++)
        {
            newDifficulty = Instantiate(difficultyPanelPrefab, difficultyPanelsContainer);
            newDifficulty.transform.position = difficultyPanelHolders[i].position;
            difficultyLogic = newDifficulty.GetComponent<Difficulty>();
            difficultyLogic.Initialize(difficultyTypes[i], i);
            difficulties.Add(difficultyLogic);
        }
    }

    private void ChangeDifficultyValuesSO(DifficultySettingsData newSettingsData)
    {
        currentDifficultyData.width = newSettingsData.width;
        currentDifficultyData.height = newSettingsData.height;
        currentDifficultyData.groundPercentage = newSettingsData.groundPercentage;
        currentDifficultyData.minPathSize = newSettingsData.minPathSize;
        currentDifficultyData.initialCurrency = newSettingsData.initialCurrency;
    }

    public void SelectDifficulty(int index)
    {
        if (currentSelectedDifficultyIndex == index)
        {
            DeselectDifficulty();
            return;
        }

        if (currentSelectedDifficultyIndex != -1)
        {
            difficulties[currentSelectedDifficultyIndex].SetSelected(false);
        }

        currentSelectedDifficultyIndex = index;
        difficulties[currentSelectedDifficultyIndex].SetSelected(true);
        ChangeDifficultyValuesSO(difficulties[currentSelectedDifficultyIndex].GetDifficultyData());
    }

    public void DeselectDifficulty()
    {
        if (currentSelectedDifficultyIndex != -1)
        {
            difficulties[currentSelectedDifficultyIndex].SetSelected(false);
            currentSelectedDifficultyIndex = -1;
            //MainMenuManager.Instance.DeselectDifficulty();
        }
    }

    private void ResetDifficulty()
    {
        if (currentSelectedDifficultyIndex != -1)
        {
            currentSelectedDifficultyIndex = -1;
            //MainMenuManager.Instance.DeselectDifficulty();
        }
    }
}
