using UnityEngine;

public class NeutrophilTower : AttackTower
{ 
    //Refill Soldiers Settings
    private float refillTimer;
    private float refillCooldown;

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is NeutrophilTowerData neutrophilTowerData)
        {
            refillCooldown = neutrophilTowerData.refillCooldown;
        }

        refillTimer = refillCooldown;
    }

    protected override void Update()
    {
        HandleSoldiersRefill();
        base.Update();
    }

    private void HandleSoldiersRefill()
    {
        if (totalSoldiers < maxSoldiers)
        {
            refillTimer -= Time.deltaTime;

            if (refillTimer <= 0f)
            {
                totalSoldiers++;
                availableSoldiers++;
                refillTimer = refillCooldown;
            }
        }
    }

    public void OnNeutrophilSoldierDead()
    {
        totalSoldiers--;
    }

    public override void Reset()
    {
        base.Reset();

        refillTimer = 0f;
        refillCooldown = 0f;
    }
}
