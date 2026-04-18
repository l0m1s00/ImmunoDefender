using System.Collections.Generic;
using UnityEngine;

public class DifficultiesManager : MonoBehaviour
{
    public static DifficultiesManager Instance;

    [Header("Difficulty Types")]
    [SerializeField] private List<DifficultySettingsData> difficultyTypes;
    [Header("UI References")]
    [SerializeField] private Transform difficultyPanelsContainer;
    [SerializeField] private List<Transform> difficultyPanelHolders;
    [SerializeField] private GameObject difficultyPanelPrefab;

    private List<Difficulty> difficulties = new List<Difficulty>();

    private int currentSelectedDifficultyIndex = -1; //-1 nada seleccionado

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        MainMenuManager.OnCloseMenu += ResetDifficulty;
    }

    private void OnDisable()
    {
        MainMenuManager.OnCloseMenu -= ResetDifficulty;
    }

    private void Start()
    {
        CreateDifficulties();
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
        MainMenuManager.Instance.SetNewSelectedDifficulty(difficulties[currentSelectedDifficultyIndex].GetDifficultyData());
    }

    public void DeselectDifficulty()
    {
        if (currentSelectedDifficultyIndex != -1)
        {
            difficulties[currentSelectedDifficultyIndex].SetSelected(false);
            currentSelectedDifficultyIndex = -1;
            MainMenuManager.Instance.DeselectDifficulty();
        }
    }

    private void ResetDifficulty()
    {
        if (currentSelectedDifficultyIndex != -1)
        {
            currentSelectedDifficultyIndex = -1;
            MainMenuManager.Instance.DeselectDifficulty();
        }
    }
}
