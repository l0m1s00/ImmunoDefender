using System.Collections.Generic;
using UnityEngine;

public class MacrophageTower : AttackTower
{
    protected override void Update()
    {
        base.Update();

        if (currentTarget == null)
        {
            if (attackStats.availableSoldiers > 0f && spawnTimer <= 0f)
            {
                if (CheckForWasteInRange())
                {
                    DeploySoldier();
                    spawnTimer = attackStats.spawnSoldierCooldown;
                }
            }
        }
    }

    private ToxicWaste GetClosestAvailableWaste()
    {
        ToxicWaste closestWaste = null;
        float shortestDistance = float.MaxValue;

        foreach (ToxicWaste waste in EnemyManager.Instance.GetToxicWasteInBattlefield())
        {
            if (waste == null || !waste.gameObject.activeInHierarchy || waste.GetIsTargeted()) continue;

            float distanceToWaste = Vector2.Distance(transform.position, waste.transform.position);

            if (distanceToWaste <= attackStats.range && distanceToWaste < shortestDistance)
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

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Range: ", statValue = attackStats.range.ToString() });
        stats.Add(new EntitiyStat { statName = "Total Soldiers: ", statValue = attackStats.maxSoldiers.ToString() });
        stats.Add(new EntitiyStat { statName = "Deployed Soldiers: ", statValue = (attackStats.maxSoldiers - attackStats.availableSoldiers).ToString() });

        return stats;
    }

    public override void Reset()
    {
        base.Reset();

        
    }
}
