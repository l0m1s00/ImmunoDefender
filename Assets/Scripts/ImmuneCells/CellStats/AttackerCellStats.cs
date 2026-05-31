using System.Collections.Generic;
using UnityEngine;

public class AttackerCellStats : BasicCellStats
{
    protected float baseDamage;
    public float damage;
    public float attackRange;
    protected float baseAttackCooldown;
    public float attackCooldown;

    public AttackerCellStats(AttackerImmuneCellData data) : base(data)
    {
        baseDamage = data.damage;
        damage = baseDamage;
        attackRange = data.attackRange;
        baseAttackCooldown = data.attackCooldown;
        attackCooldown = baseAttackCooldown;
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        string extraDamageBonuses = "";

        if (damageBonuses != null && damageBonuses.Count > 0)
        {
            List<string> convertedBonuses = new List<string>();

            foreach(KeyValuePair<EnemyData, float> key in damageBonuses)
            {
                convertedBonuses.Add(key.Key.enemyName + " (+" + key.Value.ToString("F2") + ")");
            }

            extraDamageBonuses = string.Join("\n", convertedBonuses);
        }

        stats.Add(new EntitiyStat { 
            statName = "Damage", 
            initialStatValue = baseDamage.ToString("F2"), 
            currentStatValue = damage.ToString("F2"), 
            extraInformation = extraDamageBonuses
        });

        

        return stats;
    }
}
