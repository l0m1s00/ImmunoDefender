using System.Collections.Generic;
using UnityEngine;

public class VirusEnemyStats : EnemyStats
{
    public int infectionProbability;

    public VirusEnemyStats(VirusEnemyData data) : base(data)
    {
        infectionProbability = data.infectionProbability;
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat
        {
            statName = "Infection",
            currentStatValue = infectionProbability.ToString(),
            extraInformation = "Determine the potential for infecting towers"
        });

        return stats;
    }
}
