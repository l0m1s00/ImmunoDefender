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

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.AddRange(spawnerStats.GetStats());

        return stats;
    }
}
