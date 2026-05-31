using System.Collections.Generic;
using UnityEngine;

public class BLymphocyteTowerStats : TargetingTowerStats
{
    private float baseInvestigationAmount;
    public float investigationAmount;
    private float baseInvestigationInterval;
    public float investigationInterval;
    public float maxInvestigationAmount;
    private float baseDamageBonus;
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

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

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

        stats.Add(new EntitiyStat
        {
            statName = "Biological Description",
            initialStatValue = "",
            currentStatValue = "",
            extraInformation =
            "Part of the adaptive immune system.\nThey are the body's intelligence units. These units analyse the exact structure of the different strains of pathogens.\nOnce they have understood it, they produce antibodies that mark the enemy, providing offensive capabilities to nearby immune cells when facing that specific type of enemy"
        });

        return stats;
    }
}
