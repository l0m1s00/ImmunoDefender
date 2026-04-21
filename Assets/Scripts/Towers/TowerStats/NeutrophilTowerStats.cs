using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTowerStats : AttackTowerStats
{
    protected float baseRefillCooldown; //tiempo que tarda la torre en recuperar 1 soldaado
    public float refillCooldown;

    public NeutrophilTowerStats(NeutrophilTowerData data) : base(data)
    {
        baseRefillCooldown = data.refillCooldown;
        refillCooldown = baseRefillCooldown;
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat { 
            statName = "Reload Soldier", 
            initialStatValue = baseRefillCooldown.ToString() + " s", 
            currentStatValue = refillCooldown.ToString() + " s",
            extraInformation = "Time required to recover a soldier. The recruitment of new troops begins when a soldier dies"
        });

        return stats;
    }
}
