using UnityEngine;

public class NeutrophilImmuneCell : AttackerCell
{
    private float currentDamageCapacity; //cantidad de danyo que puede infligir antes de morir

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is NeutrophilImmuneCellData neutrophilData)
        {
            currentDamageCapacity = neutrophilData.maxDamageCapacity;
        }
    }

    protected override void Attack()
    {
        base.Attack();

        currentDamageCapacity -= damage;

        if (currentDamageCapacity <= 0f)
        {
            if (headquarters is NeutrophilTower neutrophilHeadquarters) neutrophilHeadquarters.OnNeutrophilSoldierDead();
            PoolManager.Instance.DespawnImmuneCell(data, this.gameObject);
        }
    }

    public override void Reset()
    {
        base.Reset();

        currentDamageCapacity = 0f;
    }
}
