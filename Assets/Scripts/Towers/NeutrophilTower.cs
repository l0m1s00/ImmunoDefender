using System.Collections.Generic;
using UnityEngine;

public class NeutrophilTower : TargetingTower
{
    private CellSpawnerModule spawnerModule;
    private float refillTimer;

    protected NeutrophilTowerStats neutrophilTowerStats => (NeutrophilTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is NeutrophilTowerData neutrophilTowerData)
        {
            basicStats = new NeutrophilTowerStats(neutrophilTowerData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, neutrophilTowerStats.spawnerStats);
            refillTimer = neutrophilTowerStats.refillCooldown;
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleSoldiersRefill();
        bool shouldSpawn = currentTarget != null;
        spawnerModule.HandleSpawning(shouldSpawn);
    }

    private void HandleSoldiersRefill()
    {
        if (neutrophilTowerStats.spawnerStats.totalSoldiers < neutrophilTowerStats.spawnerStats.maxSoldiers)
        {
            refillTimer -= Time.deltaTime;

            if (refillTimer <= 0f)
            {
                neutrophilTowerStats.spawnerStats.totalSoldiers++;
                neutrophilTowerStats.spawnerStats.availableSoldiers++;
                NotifyStatsChanged();
                refillTimer = neutrophilTowerStats.refillCooldown;
                AudioManager.Instance.PlaySFX(neutrophilTowerStats.data.reloadSoldierSounds, transform.position);
            }
        }
    }

    public void OnNeutrophilSoldierDead()
    {
        neutrophilTowerStats.spawnerStats.totalSoldiers--;
        NotifyStatsChanged();
    }

    public override void Reset()
    {
        base.Reset();

        refillTimer = 0f;
    }
}
