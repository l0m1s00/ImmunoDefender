using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTower : TargetingTower
{
    private float investigationAmount;
    private float investigationInterval;
    private float maxInvestigationAmount = 100f;
    Dictionary<EnemyData, float> currentInvestigation;
    private Enemy lastInvestigatedEnemy;

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is BLymphocyteTowerData bLymphocyteTowerData)
        {
            investigationAmount = bLymphocyteTowerData.investigationAmount;
            investigationInterval = bLymphocyteTowerData.investigationInterval;
        }

        currentInvestigation = new Dictionary<EnemyData, float>();

        foreach (EnemyData enemyType in DataLibrary.Instance.GetEnemyTypes())
        {
            currentInvestigation.Add(enemyType, 0f);
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
            float currentAmount = currentInvestigation[currentTarget.GetData()];
            currentTarget.UpdateInvestigationBar(currentAmount);
        }

        lastInvestigatedEnemy = currentTarget;
    }

    private void InvestigateEnemy()
    {
        if (currentTarget == null) return;

        float amountPerSecond = investigationAmount / investigationInterval;
        currentInvestigation[currentTarget.GetData()] += amountPerSecond * Time.deltaTime;

        float amount = currentInvestigation[currentTarget.GetData()];

        if (amount >= maxInvestigationAmount)
        {
            currentInvestigation[currentTarget.GetData()] = 0f;

            //añadir bufos

            amount = 0f;
        }

        currentTarget.UpdateInvestigationBar(amount / maxInvestigationAmount);

    }

    protected override bool IsValidTarget(Enemy enemy)
    {
        return !enemy.IsBeingInvestigated();
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

        investigationAmount = 0f;
        investigationInterval = 0f;
        currentInvestigation = null;
    }
}
