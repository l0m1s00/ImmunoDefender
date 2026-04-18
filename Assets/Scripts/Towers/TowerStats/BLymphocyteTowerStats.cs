using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTowerStats : TargetingTowerStats
{
    public float investigationAmount;
    public float investigationInterval;
    public float maxInvestigationAmount;
    public Dictionary<EnemyData, float> currentInvestigation;

    public BLymphocyteTowerStats(BLymphocyteTowerData data) : base(data)
    {
        investigationAmount = data.investigationAmount;
        investigationInterval = data.investigationInterval;
        maxInvestigationAmount = 100f;
        currentInvestigation = new Dictionary<EnemyData, float>();

        foreach (EnemyData enemyType in DataLibrary.Instance.GetEnemyTypes())
        {
            currentInvestigation.Add(enemyType, 0f);
        }
    }
}
