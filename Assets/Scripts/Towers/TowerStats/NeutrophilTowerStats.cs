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

    public List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Range", initialStatValue = baseRange.ToString(), currentStatValue = range.ToString() });
        stats.Add(new EntitiyStat { statName = "Available Soldiers", currentStatValue = availableSoldiers.ToString() });
        stats.Add(new EntitiyStat { statName = "Deployed Soldiers", currentStatValue = (totalSoldiers - availableSoldiers).ToString() });
        stats.Add(new EntitiyStat { statName = "Reload Soldier", initialStatValue = baseRefillCooldown.ToString(), currentStatValue = refillCooldown.ToString()});

        return stats;
    }
}
