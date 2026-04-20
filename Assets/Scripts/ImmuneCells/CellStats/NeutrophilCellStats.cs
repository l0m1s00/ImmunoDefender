using System.Collections.Generic;
using UnityEngine;

public class NeutrophilCellStats : AttackerCellStats
{
    protected float baseMaxAttacksCapacity;
    public float attacksCapacity;
    public float remainingAttacks;

    public NeutrophilCellStats(NeutrophilImmuneCellData data) : base(data)
    {
        baseMaxAttacksCapacity = data.maxAttacksCapacity;
        attacksCapacity = baseMaxAttacksCapacity;
        remainingAttacks = attacksCapacity;
    }

    public override List<EntitiyStat> GetStats(List<float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStats(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Remaining Attacks", 
            initialStatValue = baseMaxAttacksCapacity.ToString(), 
            currentStatValue = remainingAttacks.ToString() 
        });

        return stats;
    }
}
