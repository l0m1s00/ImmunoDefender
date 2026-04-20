using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageCellStats : AttackerCellStats
{
    protected float baseDeleteToxicWasteTime;
    public float deleteToxicWasteTime;

    public MacrophageCellStats(MacrophageImmuneCellData data) : base(data)
    {
        baseDeleteToxicWasteTime = data.deleteToxicWasteCooldown;
        deleteToxicWasteTime = baseDeleteToxicWasteTime;
    }

    public override List<EntitiyStat> GetStats(List<float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStats(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Clean Waste", 
            initialStatValue = baseDeleteToxicWasteTime.ToString() + " s", 
            currentStatValue = deleteToxicWasteTime.ToString() + " s"
        });

        return stats;
    }
}
