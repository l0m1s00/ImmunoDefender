using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageTowerStats : TargetingTowerStats
{
    public CellSpawnerStats spawnerStats;

    public MacrophageTowerStats(MacrophageTowerData data) : base(data)
    {
        spawnerStats = new CellSpawnerStats(data.spawnerData);
    }

    public override void ApplyUpgrade(TowerUpgradeData upgradeData, int upgradeLevel)
    {
        base.ApplyUpgrade(upgradeData, upgradeLevel);
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.AddRange(spawnerStats.GetStatsToDisplay());

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "Part of the innate immune system.\nThey are the big eaters, heavy and very resistant units that patrol the tissues to keep the body clean.\nThey swallow up toxic waste and pathogens to prevent the area from becoming contaminated"
        });

        return stats;
    }
}
