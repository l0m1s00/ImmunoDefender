using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTower : AttackTower
{
    private float refillTimer;

    protected NeutrophilTowerStats neutrophilStats => (NeutrophilTowerStats)basicStats;

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is NeutrophilTowerData neutrophilTowerData)
        {
            basicStats = new NeutrophilTowerStats(neutrophilTowerData);
        }

        refillTimer = neutrophilStats.refillCooldown;
    }

    protected override void Update()
    {
        HandleSoldiersRefill();
        base.Update();
    }

    private void HandleSoldiersRefill()
    {
        if (neutrophilStats.totalSoldiers < neutrophilStats.maxSoldiers)
        {
            refillTimer -= Time.deltaTime;

            if (refillTimer <= 0f)
            {
                neutrophilStats.totalSoldiers++;
                neutrophilStats.availableSoldiers++;
                refillTimer = neutrophilStats.refillCooldown;
            }
        }
    }

    public void OnNeutrophilSoldierDead()
    {
        neutrophilStats.totalSoldiers--;
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Range: ", statValue = neutrophilStats.range.ToString() });
        stats.Add(new EntitiyStat { statName = "Total Soldiers: ", statValue = neutrophilStats.maxSoldiers.ToString() });
        stats.Add(new EntitiyStat { statName = "Deployed Soldiers: ", statValue = (neutrophilStats.maxSoldiers - neutrophilStats.availableSoldiers).ToString() });
        stats.Add(new EntitiyStat { statName = "Reload Soldier (s): ", statValue = neutrophilStats.refillCooldown.ToString() });

        return stats;
    }

    public override void Reset()
    {
        base.Reset();

        refillTimer = 0f;
        neutrophilStats.refillCooldown = 0f;
    }
}
