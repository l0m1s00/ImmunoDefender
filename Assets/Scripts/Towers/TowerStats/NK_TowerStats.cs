using System.Collections.Generic;
using UnityEngine;

public class NK_TowerStats : TargetingTowerStats
{
    public CellSpawnerStats spawnerStats;

    public NK_TowerStats(NK_TowerData data) : base(data)
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
            "Lymphocytes belonging to the innate immune system.\nTheir main role is to identify and destroy infected or cancerous cells."
        });

        return stats;
    }
}
