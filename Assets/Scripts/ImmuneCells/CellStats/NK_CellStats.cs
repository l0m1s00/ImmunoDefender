using System.Collections.Generic;
using UnityEngine;

public class NK_CellStats : AttackerCellStats
{
    private float baseAnalysisAmount;
    public float analysisAmount;
    private float baseAnalysisInterval;
    public float analysisInterval;
    public float maxAnalysisAmount;

    public NK_CellStats(NK_ImmuneCellData data) : base(data)
    {
        baseAnalysisAmount = data.analysisAmount;
        analysisAmount = baseAnalysisAmount;
        baseAnalysisInterval = data.analysisInterval;
        analysisInterval = baseAnalysisInterval;
        maxAnalysisAmount = 100f;
    }

    public override void ApplyUpgrade(UpgradeType upgradeType, float amount)
    {
        base.ApplyUpgrade(upgradeType, amount);
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        stats.Add(new EntitiyStat
        {
            statName = "Analysis Amount",
            initialStatValue = baseAnalysisAmount.ToString(),
            currentStatValue = analysisAmount.ToString()
        });

        stats.Add(new EntitiyStat
        {
            statName = "Analysis Interval",
            initialStatValue = (baseAnalysisAmount / baseAnalysisInterval).ToString() + " / s",
            currentStatValue = (analysisAmount / analysisInterval).ToString() + " / s",
            extraInformation = "Determines the time taken to complete the scan of a potentially infected farm"
        });

        return stats;
    }
}
