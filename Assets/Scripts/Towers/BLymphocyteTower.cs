using System;
using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTower : TargetingTower
{
    private Enemy lastInvestigatedEnemy;
    protected BLymphocyteTowerStats bLymphocyteStats => (BLymphocyteTowerStats)basicStats;

    //EVENTOS
    public static event Action OnBuffGenerated;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

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
            bLymphocyteStats.damageBonuses[currentTarget.GetData()] += bLymphocyteStats.damageBonus;
            OnBuffGenerated?.Invoke();
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
        return bLymphocyteStats.GetStats();
    }

    public float GetDamageBonusAgainstEnemy(EnemyData enemyData)
    {
        return bLymphocyteStats.damageBonuses[enemyData];
    }

    public Dictionary<EnemyData, float> GetAllDamageBonuses()
    {
        return bLymphocyteStats.damageBonuses;
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
