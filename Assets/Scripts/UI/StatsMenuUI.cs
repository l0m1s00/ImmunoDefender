using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatsMenuUI : BaseMenuPanelUI
{
    [SerializeField] private TextMeshProUGUI entityNameText;
    [SerializeField] private Transform statsContainer;
    [SerializeField] private GameObject statSlotPrefab;

    private void OnEnable()
    {
        SelectionManager.OnEntitySelectedForInfo += DisplayEntityStats;
        SelectionManager.OnEntityDeselected += HideEntityStats;
    }

    private void OnDisable()
    {
        SelectionManager.OnEntitySelectedForInfo -= DisplayEntityStats;
        SelectionManager.OnEntityDeselected -= HideEntityStats;
    }

    private void DisplayEntityStats(string entityName, List<EntitiyStat> stats)
    {
        entityNameText.text = entityName;

        foreach (Transform childStatSlot in statsContainer) Destroy(childStatSlot.gameObject);

        foreach (EntitiyStat stat in stats)
        {
            GameObject newStatSlot = Instantiate(statSlotPrefab, statsContainer);
            newStatSlot.GetComponent<EntityStatsSlotUI>().Initialize(stat.statName, stat.statValue);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(statsContainer.GetComponent<RectTransform>());

        Open();
    }

    private void HideEntityStats()
    {
        Close();
    }
}
