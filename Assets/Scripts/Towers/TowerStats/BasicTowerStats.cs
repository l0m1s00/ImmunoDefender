using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BasicTowerStats
{
    public TowerData baseData; //todas las torres tienen esta informacion
    public Dictionary<TowerUpgradeData, int> currentUpgradesByLevel; //todas las torres tienen mejoras

    public BasicTowerStats(TowerData data)
    {
        baseData = data;
        currentUpgradesByLevel = new Dictionary<TowerUpgradeData, int>();

        if (baseData.towerUpgrades != null) //crear mejoras
        {
            foreach (TowerUpgradeData upgrade in baseData.towerUpgrades)
            {
                currentUpgradesByLevel.Add(upgrade, 0);
            }
        }
    }

    public virtual void ApplyUpgrade(TowerUpgradeData upgradeData, int upgradeLevel)
    {
        
    }

    public virtual List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();
        return stats;
    }
}
