using System.Collections.Generic;
using UnityEngine;

public class NK_CellStats : AttackerCellStats
{
    public NK_CellStats(NK_ImmuneCellData data) : base(data)
    {

    }

    public override List<EntitiyStat> GetStats(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStats(damageBonuses);

        //stats.Add(new EntitiyStat
        //{
        //    statName = "Clean Waste",
        //    initialStatValue = baseDeleteToxicWasteTime.ToString("F2") + " s",
        //    currentStatValue = deleteToxicWasteTime.ToString("F2") + " s",
        //    extraInformation = "The time it takes to eliminate a toxic waste"
        //});

        return stats;
    }
}
