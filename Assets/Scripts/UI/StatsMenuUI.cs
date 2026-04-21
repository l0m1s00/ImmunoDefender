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

public class StatsMenuUI : BaseMenuPanelUI
{
    [SerializeField] private TextMeshProUGUI entityNameText;
    [SerializeField] private Transform statsContainer;
    [SerializeField] private GameObject statSlotPrefab;

    private void OnEnable()
    {
        SelectionManager.OnEntitySelectedForInfo += DisplayEntityStats;
        SelectionManager.OnEntityDeselected += HideEntityStats;

        //eventos de modificacion de estadisticas de entidades
    }

    private void OnDisable()
    {
        SelectionManager.OnEntitySelectedForInfo -= DisplayEntityStats;
        SelectionManager.OnEntityDeselected -= HideEntityStats;
    }

    private void UpdateStats(string entityName, List<EntitiyStat> stats)
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

    private void DisplayEntityStats(string entityName, List<EntitiyStat> stats)
    {
        UpdateStats(entityName, stats);
        Open();
    }

    private void HideEntityStats()
    {
        Close();
    }
}
