using System.Collections.Generic;

public class NeutrophilCellStats : AttackerCellStats
{
    protected int baseMaxAttacksCapacity;
    public int attacksCapacity;
    public int remainingAttacks;

    public NeutrophilCellStats(NeutrophilImmuneCellData data) : base(data)
    {
        baseMaxAttacksCapacity = data.maxAttacksCapacity;
        attacksCapacity = baseMaxAttacksCapacity;
        remainingAttacks = attacksCapacity;
    }

    public override List<EntitiyStat> GetStats(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStats(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Remaining Attacks", 
            initialStatValue = baseMaxAttacksCapacity.ToString(), 
            currentStatValue = remainingAttacks.ToString(),
            extraInformation = "Number of attacks before death"
        });

        return stats;
    }
}
