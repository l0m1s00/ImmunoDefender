using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageCellStats : AttackerCellStats
{
    protected float baseDeleteToxicWasteTime;
    public float deleteToxicWasteTime;

    public MacrophageCellStats(MacrophageImmuneCellData data) : base(data)
    {
        baseDeleteToxicWasteTime = data.deleteToxicWasteCooldown;
        deleteToxicWasteTime = baseDeleteToxicWasteTime;
    }

    public override void ApplyUpgrade(UpgradeType upgradeType, float amount)
    {
        base.ApplyUpgrade(upgradeType, amount);
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Clean Waste", 
            initialStatValue = baseDeleteToxicWasteTime.ToString("F2") + " s", 
            currentStatValue = deleteToxicWasteTime.ToString("F2") + " s",
            extraInformation = "The time it takes to eliminate a toxic waste"
        });

        return stats;
    }
}
