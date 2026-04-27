using System.Collections.Generic;
using UnityEngine;

public class VirionEnemy : Enemy
{
    protected VirionEnemyStats virionStats => (VirionEnemyStats)basicStats;

    public override void Initialize(List<Cell> route, EnemyData data)
    {
        base.Initialize(route, data);

        if (data is VirionEnemyData virionData)
        {
            basicStats = new VirionEnemyStats(virionData);
        }
    }

    private void Update()
    {
        Move();
    }

    public override void Reset()
    {
        base.Reset();
    }
}
