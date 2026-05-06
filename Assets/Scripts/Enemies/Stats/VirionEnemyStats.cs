using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class VirionEnemyStats : EnemyStats
{
    public VirionEnemyStats(VirionEnemyData data) : base(data) { }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "When a virus replicates, these copies are released into the environment in the form of virions, which contain the parent virus’s DNA"
        });
        
        return stats;
    }
}
