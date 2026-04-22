using System.Collections.Generic;
using UnityEngine;

public class StemCellStats : BasicCellStats
{
    public StemCellStats(StemCellData data) : base(data)
    {
        
    }

    //public override List<EntitiyStat> GetStats(Dictionary<EnemyData, float> damageBonuses)
    //{
    //    List<EntitiyStat> stats = base.GetStats(damageBonuses);

    //    stats.Add(new EntitiyStat
    //    {
    //        statName = "Remaining Attacks",
    //        initialStatValue = baseMaxAttacksCapacity.ToString(),
    //        currentStatValue = remainingAttacks.ToString(),
    //        extraInformation = "Number of attacks before death"
    //    });

    //    return stats;
    //}
}
