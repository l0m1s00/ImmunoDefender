using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageTowerStats : AttackTowerStats
{
    public MacrophageTowerStats(MacrophageTowerData data) : base(data) { }

    public List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Range", initialStatValue = baseRange.ToString(), currentStatValue = range.ToString() });
        stats.Add(new EntitiyStat { statName = "Available Soldiers", currentStatValue = availableSoldiers.ToString() });
        stats.Add(new EntitiyStat { statName = "Deployed Soldiers", currentStatValue = (baseMaxSoldiers - availableSoldiers).ToString() });

        return stats;
    }
}
