using System.Collections.Generic;

public class NeutrophilCellStats : AttackerCellStats
{
    protected int baseMaxAttacksCapacity;
    public int attacksCapacity;
    public int remainingAttacks;
    public NeutrophilImmuneCellData neutrophilData;

    public NeutrophilCellStats(NeutrophilImmuneCellData data) : base(data)
    {
        baseMaxAttacksCapacity = data.maxAttacksCapacity;
        attacksCapacity = baseMaxAttacksCapacity;
        remainingAttacks = attacksCapacity;
        neutrophilData = data;
    }

    public override List<EntitiyStat> GetStatsToDisplay(Dictionary<EnemyData, float> damageBonuses)
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay(damageBonuses);

        stats.Add(new EntitiyStat { 
            statName = "Remaining Attacks", 
            initialStatValue = baseMaxAttacksCapacity.ToString(), 
            currentStatValue = remainingAttacks.ToString(),
            extraInformation = "Number of attacks before death"
        });

        return stats;
    }
}
