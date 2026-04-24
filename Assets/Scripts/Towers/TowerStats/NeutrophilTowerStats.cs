using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTowerStats : TargetingTowerStats
{
    protected float baseRefillCooldown; //tiempo que tarda la torre en recuperar 1 soldaado
    public float refillCooldown;

    public CellSpawnerStats spawnerStats;

    public NeutrophilTowerStats(NeutrophilTowerData data) : base(data)
    {
        baseRefillCooldown = data.refillCooldown;
        refillCooldown = baseRefillCooldown;
        spawnerStats = new CellSpawnerStats(data.spawnerData);
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.AddRange(spawnerStats.GetStats());

        stats.Add(new EntitiyStat { 
            statName = "Reload Soldier", 
            initialStatValue = baseRefillCooldown.ToString("F2") + " s", 
            currentStatValue = refillCooldown.ToString("F2") + " s",
            extraInformation = "Time required to recover a soldier. The recruitment of new troops begins when a soldier dies"
        });

        return stats;
    }
}
