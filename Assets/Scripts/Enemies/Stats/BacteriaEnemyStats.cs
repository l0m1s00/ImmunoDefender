using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class BacteriaEnemyStats : EnemyStats
{
    public BacteriaEnemyStats(BacteriaEnemyData data) : base(data) { }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "They are extraordinarily adaptable organisms found in almost any environment.\nAlthough some species are pathogenic, the great majority are beneficial and essential for vital processes such as digestion or fermentation"
        });

        return stats;
    }
}
