using UnityEngine;

public class MacrophageTower : AttackTower
{
    protected override void Update()
    {
        base.Update();

        if (currentTarget == null)
        {
            if (availableSoldiers > 0f && spawnTimer <= 0f)
            {
                if (CheckForWasteInRange())
                {
                    DeploySoldier();
                    spawnTimer = spawnSoldierCooldown;
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

            if (distanceToWaste <= range && distanceToWaste < shortestDistance)
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
