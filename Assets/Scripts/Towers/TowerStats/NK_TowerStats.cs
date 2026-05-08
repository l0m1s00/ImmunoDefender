using System.Collections.Generic;
using UnityEngine;

public class NK_TowerStats : TargetingTowerStats
{
    public CellSpawnerStats spawnerStats;

    public NK_TowerStats(NK_TowerData data) : base(data)
    {
        spawnerStats = new CellSpawnerStats(data.spawnerData);
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.AddRange(spawnerStats.GetStats());

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "PLACE HOLDER de la descripcion biologica de la celula NK alargada pues necesito comprobar que el tooltip funcione bien"
        });

        return stats;
    }
}
