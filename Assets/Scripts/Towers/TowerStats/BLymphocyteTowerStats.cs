using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTowerStats : TargetingTowerStats
{
    protected float baseInvestigationAmount;
    public float investigationAmount;
    protected float baseInvestigationInterval;
    public float investigationInterval;
    public float maxInvestigationAmount;
    protected float baseDamageBonus;
    public float damageBonus;
    public Dictionary<EnemyData, float> currentInvestigation;
    public Dictionary<EnemyData, float> damageBonuses;

    public BLymphocyteTowerStats(BLymphocyteTowerData data) : base(data)
    {
        baseInvestigationAmount = data.investigationAmount;
        investigationAmount = baseInvestigationAmount;
        baseInvestigationInterval = data.investigationInterval;
        investigationInterval = baseInvestigationInterval;
        maxInvestigationAmount = 100f;
        baseDamageBonus = data.damageBonus;
        damageBonus = baseDamageBonus;
        currentInvestigation = new Dictionary<EnemyData, float>();
        damageBonuses = new Dictionary<EnemyData, float>();

        foreach (EnemyData enemyType in DataLibrary.Instance.GetEnemyTypes())
        {
            currentInvestigation.Add(enemyType, 0f);
            damageBonuses.Add(enemyType, 0f);
        }
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat { 
            statName = "Investigation Amount", 
            initialStatValue = baseInvestigationAmount.ToString(), 
            currentStatValue = investigationAmount.ToString()
        });

        stats.Add(new EntitiyStat { 
            statName = "Investigation Interval", 
            initialStatValue = (baseInvestigationAmount / baseInvestigationInterval).ToString() + " / s", 
            currentStatValue = (investigationAmount / investigationInterval).ToString() + " / s",
            extraInformation = "Amount of research per second"
        });

        return stats;
    }
}
