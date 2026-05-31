using System.Collections.Generic;
using UnityEngine;

public class TargetingTowerStats : BasicTowerStats
{
    protected float baseRange; 
    public float range; //rango de la torre

    public TargetingTowerStats(TargetingTowerData data) : base(data)
    {
        baseRange = data.range;
        range = baseRange;
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.Add(new EntitiyStat { 
            statName = "Range", 
            initialStatValue = baseRange.ToString(), 
            currentStatValue = range.ToString() 
        });

        return stats;
    }
}
