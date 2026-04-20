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

    public override List<EntitiyStat> GetStats(List<float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStats(damageBonuses);

        string extraDamageBonuses = "\n";

        for (int i = 0; i < damageBonuses.Count; i++)
        {
            extraDamageBonuses += "(+" + damageBonuses[i].ToString();

            if (i == damageBonuses.Count - 1) extraDamageBonuses += ")";
            else extraDamageBonuses += ") ";
        }

        stats.Add(new EntitiyStat { 
            statName = "Damage", 
            initialStatValue = baseDamage.ToString(), 
            currentStatValue = damage.ToString(), 
            extraStatValue = extraDamageBonuses
        });

        return stats;
    }
}
