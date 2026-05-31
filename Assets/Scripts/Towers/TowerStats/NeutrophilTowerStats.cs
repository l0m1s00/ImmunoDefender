using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTowerStats : TargetingTowerStats
{
    protected float baseRefillCooldown; //tiempo que tarda la torre en recuperar 1 soldaado
    public float refillCooldown;
    public NeutrophilTowerData data;
    public CellSpawnerStats spawnerStats;

    public NeutrophilTowerStats(NeutrophilTowerData data) : base(data)
    {
        baseRefillCooldown = data.refillCooldown;
        refillCooldown = baseRefillCooldown;
        spawnerStats = new CellSpawnerStats(data.spawnerData);
        this.data = data;
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.AddRange(spawnerStats.GetStatsToDisplay());

        stats.Add(new EntitiyStat { 
            statName = "Reload Soldier", 
            initialStatValue = baseRefillCooldown.ToString("F2") + " s", 
            currentStatValue = refillCooldown.ToString("F2") + " s",
            extraInformation = "Time required to recover a soldier. The recruitment of new troops begins when a soldier dies"
        });

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "Part of the innate immune system.\nThey are the first defenders to arrive at the scene of the infection.\nVery numerous and fast. However, they are quickly depleted. They have a very short lifespan and usually die after their first battle"
        });

        return stats;
    }
}
