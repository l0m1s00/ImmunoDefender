using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTower : TargetingTower
{
    private Enemy lastInvestigatedEnemy;
    protected BLymphocyteTowerStats bLymphocyteStats => (BLymphocyteTowerStats)basicStats;

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is BLymphocyteTowerData bLymphocyteTowerData)
        {
            basicStats = new BLymphocyteTowerStats(bLymphocyteTowerData);
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleInvestigation();
        InvestigateEnemy();
    }

    private void HandleInvestigation()
    {
        if (currentTarget != lastInvestigatedEnemy)
        {
            if (lastInvestigatedEnemy != null)
            {
                lastInvestigatedEnemy.DeactivateInvestigationBar();
                lastInvestigatedEnemy.SetIsBeingInvestigated(false);
            }
        }

        if (currentTarget != null)
        {
            currentTarget.SetIsBeingInvestigated(true);
            float currentAmount = bLymphocyteStats.currentInvestigation[currentTarget.GetData()];
            currentTarget.UpdateInvestigationBar(currentAmount);
        }

        lastInvestigatedEnemy = currentTarget;
    }

    private void InvestigateEnemy()
    {
        if (currentTarget == null) return;

        float amountPerSecond = bLymphocyteStats.investigationAmount / bLymphocyteStats.investigationInterval;
        bLymphocyteStats.currentInvestigation[currentTarget.GetData()] += amountPerSecond * Time.deltaTime;

        float amount = bLymphocyteStats.currentInvestigation[currentTarget.GetData()];

        if (amount >= bLymphocyteStats.maxInvestigationAmount)
        {
            bLymphocyteStats.currentInvestigation[currentTarget.GetData()] = 0f;

            //añadir bufos

            amount = 0f;
        }

        currentTarget.UpdateInvestigationBar(amount / bLymphocyteStats.maxInvestigationAmount);

    }

    protected override bool IsValidTarget(Enemy enemy)
    {
        return !enemy.IsBeingInvestigated();
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Range: ", statValue = bLymphocyteStats.range.ToString() });
        stats.Add(new EntitiyStat { statName = "Investigation Interval (s): ", statValue = bLymphocyteStats.investigationInterval.ToString() });
        stats.Add(new EntitiyStat { statName = "Investigation Amount: ", statValue = bLymphocyteStats.investigationAmount.ToString() });

        return stats;
    }

    public override void Reset()
    {
        base.Reset();

        if (lastInvestigatedEnemy != null)
        {
            lastInvestigatedEnemy.DeactivateInvestigationBar();
            lastInvestigatedEnemy.SetIsBeingInvestigated(false);
            lastInvestigatedEnemy = null;
        }

        bLymphocyteStats.investigationAmount = 0f;
        bLymphocyteStats.investigationInterval = 0f;
        bLymphocyteStats.currentInvestigation = null;
    }
}
