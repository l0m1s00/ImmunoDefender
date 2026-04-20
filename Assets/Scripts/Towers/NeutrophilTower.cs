using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTower : AttackTower
{
    private float refillTimer;

    protected NeutrophilTowerStats neutrophilTowerStats => (NeutrophilTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is NeutrophilTowerData neutrophilTowerData)
        {
            basicStats = new NeutrophilTowerStats(neutrophilTowerData);
        }

        refillTimer = neutrophilTowerStats.refillCooldown;
    }

    protected override void Update()
    {
        HandleSoldiersRefill();
        base.Update();
    }

    private void HandleSoldiersRefill()
    {
        if (neutrophilTowerStats.totalSoldiers < neutrophilTowerStats.maxSoldiers)
        {
            refillTimer -= Time.deltaTime;

            if (refillTimer <= 0f)
            {
                neutrophilTowerStats.totalSoldiers++;
                neutrophilTowerStats.availableSoldiers++;
                NotifyStatsChanged();
                refillTimer = neutrophilTowerStats.refillCooldown;
            }
        }
    }

    public void OnNeutrophilSoldierDead()
    {
        neutrophilTowerStats.totalSoldiers--;
        NotifyStatsChanged();
    }

    public override List<EntitiyStat> GetStats()
    {
        return neutrophilTowerStats.GetStats();
    }

    public override void Reset()
    {
        base.Reset();

        refillTimer = 0f;
        neutrophilTowerStats.refillCooldown = 0f;
    }
}
