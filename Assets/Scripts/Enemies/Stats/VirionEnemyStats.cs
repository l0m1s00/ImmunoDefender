using System.Collections.Generic;

public class VirionEnemyStats : EnemyStats
{
    public VirionEnemyStats(VirionEnemyData data) : base(data) { }

    public override List<EntitiyStat> GetStats()
    {
        return base.GetStats();
    }
}
