using System.Collections.Generic;
using UnityEngine;

public class MacrophageTower : TargetingTower
{
    private CellSpawnerModule spawnerModule;
    protected MacrophageTowerStats macrophageStats => (MacrophageTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is MacrophageTowerData macrophageTowerData)
        {
            basicStats = new MacrophageTowerStats(macrophageTowerData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, macrophageStats.spawnerStats);
        }
    }

    protected override void Update()
    {
        base.Update();

        bool shouldSpawn = currentTarget != null || CheckForWasteInRange();
        spawnerModule.HandleSpawning(shouldSpawn);
    }

    private ToxicWaste GetClosestAvailableWaste()
    {
        ToxicWaste closestWaste = null;
        float shortestDistance = float.MaxValue;

        foreach (ToxicWaste waste in EnemyManager.Instance.GetToxicWasteInBattlefield())
        {
            if (waste == null || !waste.gameObject.activeInHierarchy || waste.GetIsTargeted()) continue;

            float distanceToWaste = Vector2.Distance(transform.position, waste.transform.position);

            if (distanceToWaste <= GetTowerRange() && distanceToWaste < shortestDistance)
            {
                shortestDistance = distanceToWaste;
                closestWaste = waste;
            }
        }

        return closestWaste;
    }

    private bool CheckForWasteInRange()
    {
        return GetClosestAvailableWaste() != null;
    }

    public ToxicWaste RequestWasteTarget()
    {
        ToxicWaste target = GetClosestAvailableWaste();

        if (target != null) target.SetIsTargeted(true);

        return target;
    }

    public override void Reset()
    {
        base.Reset();

        
    }
}
