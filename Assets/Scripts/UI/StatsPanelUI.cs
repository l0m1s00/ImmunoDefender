using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//MOVER
[System.Serializable]
public struct EntitiyStat
{
    public string statName;
    public string initialStatValue;
    public string currentStatValue;
    public string extraInformation;
}

public class StatsPanelUI : BasePanelUI
{
    [SerializeField] private TextMeshProUGUI entityNameText;
    [SerializeField] private Transform statsContainer;
    [SerializeField] private GameObject statSlotPrefab;

    private void OnEnable()
    {
        SelectionManager.OnRefreshSelectedEntityInfo += UpdateStatsUI;
        SelectionManager.OnEntityDeselected += HideEntityStats;
        SelectionManager.OnEntitySelected += DisplayEntityStats;
    }

    private void OnDisable()
    {
        SelectionManager.OnRefreshSelectedEntityInfo -= UpdateStatsUI;
        SelectionManager.OnEntityDeselected -= HideEntityStats;
        SelectionManager.OnEntitySelected -= DisplayEntityStats;
    }

    private void UpdateStatsUI(string entityName, List<EntitiyStat> stats)
    {
        entityNameText.text = entityName;

        foreach (Transform childStatSlot in statsContainer) Destroy(childStatSlot.gameObject);

        foreach (EntitiyStat stat in stats)
        {
            GameObject newStatSlot = Instantiate(statSlotPrefab, statsContainer);
            newStatSlot.GetComponent<EntityStatsSlotUI>().Initialize(stat.statName, stat.initialStatValue, stat.currentStatValue, stat.extraInformation);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(statsContainer.GetComponent<RectTransform>());
    }

    private void DisplayEntityStats(ISelectable Is)
    {
        Open();
    }

    private void HideEntityStats()
    {
        Close();
    }
}
