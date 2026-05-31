using System.Collections.Generic;
using UnityEngine;

public class VirusEnemyStats : EnemyStats
{
    public int infectionProbability;

    public VirusEnemyStats(VirusEnemyData data) : base(data)
    {
        infectionProbability = data.infectionProbability;
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.Add(new EntitiyStat
        {
            statName = "Infection",
            currentStatValue = infectionProbability.ToString(),
            extraInformation = "Determine the potential for infecting towers"
        });

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "Infectious agents that are unable to survive or reproduce on their own.\nThey are very small and fast, using their speed to overwhelm the immune system.\nHave the ability to hijack healthy cells and use them as factories to produce new viruses"
        });

    return stats;
    }
}
