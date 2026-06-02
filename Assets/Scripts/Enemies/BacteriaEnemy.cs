using System.Collections.Generic;
using UnityEngine;

public class BacteriaEnemy : Enemy
{
    //Stats
    protected BacteriaEnemyStats bacteriaStats => (BacteriaEnemyStats)basicStats;

    public override void Initialize(List<Cell> route, EnemyData data)
    {
        base.Initialize(route, data);

        if (data is BacteriaEnemyData bacteriaData)
        {
            basicStats = new BacteriaEnemyStats(bacteriaData);
        }
    }

    void Update()
    {
        Move(false);
    }
}
