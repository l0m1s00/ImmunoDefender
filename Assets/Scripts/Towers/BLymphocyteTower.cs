using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTower : TargetingTower
{
    private float investigationAmount;
    private float investigationInterval;
    private float investigationTimer;
    Dictionary<Enemy, float> currentInvestigation = new Dictionary<Enemy, float>();

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is BLymphocyteTowerData bLymphocyteTowerData)
        {
            investigationAmount = bLymphocyteTowerData.investigationAmount;
            investigationInterval = bLymphocyteTowerData.investigationInterval;
        }

        investigationTimer = investigationInterval;
    }
}
