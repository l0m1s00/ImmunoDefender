using System.Collections.Generic;
using UnityEngine;

public class NK_CellStats : AttackerCellStats
{
    public float baseAnalysisTime;
    public float analysisTime;

    public NK_CellStats(NK_ImmuneCellData data) : base(data)
    {
        baseAnalysisTime = data.analysisTime;
        analysisTime = baseAnalysisTime;
    }

    public override void ApplyUpgrade(UpgradeType upgradeType, float amount)
    {
        base.ApplyUpgrade(upgradeType, amount);

        if (upgradeType == UpgradeType.FarmAnalysis)
        {
            analysisTime -= amount;
        }
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        stats.Add(new EntitiyStat
        {
            statName = "Analysis Interval",
            initialStatValue = baseAnalysisTime.ToString("F2") + " / s",
            currentStatValue = analysisTime.ToString("F2") + " / s",
            extraInformation = "Determines the time taken to complete the scan of a potentially infected farm"
        });

        return stats;
    }
}
