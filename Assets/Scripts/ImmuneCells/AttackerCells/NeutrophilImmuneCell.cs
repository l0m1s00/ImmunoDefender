using System.Collections.Generic;
using UnityEngine;

public class NeutrophilImmuneCell : AttackerCell
{
    protected NeutrophilCellStats neutrophilCellStats => (NeutrophilCellStats)basicStats;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is NeutrophilImmuneCellData neutrophilData)
        {
            basicStats = new NeutrophilCellStats(neutrophilData);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        neutrophilCellStats.remainingAttacks --;
        NotifyStatsChanged();

        if (neutrophilCellStats.remainingAttacks <= 0)
        {
            if (headquarters is NeutrophilTower neutrophilHeadquarters) neutrophilHeadquarters.OnNeutrophilSoldierDead();
            PoolEntitiesManager.Instance.DespawnImmuneCell(neutrophilCellStats.data, this.gameObject);
            AudioManager.Instance.PlaySFX(neutrophilCellStats.neutrophilData.deadSounds, transform.position);
        }
    }

    //public override List<EntitiyStat> GetStats()
    //{
    //    return neutrophilCellStats.GetStats(CalculateCurrentDamageBuffs());
    //}

    public override void Reset()
    {
        base.Reset();

        neutrophilCellStats.remainingAttacks = 0;
    }
}
