using System.Collections.Generic;
using UnityEngine;

public class FungiEnemyStats : EnemyStats
{
    private int minToxicWasteAmountOnDeath;
    private int maxToxicWasteAmountOnDeath;
    public int toxicWasteAmountOnDeath;
    private float minGenerateToxicWasteCooldown;
    private float maxGenerateToxicWasteCooldown;
    public float dropWasteCooldown;

    public FungiEnemyStats(FungiEnemyData data) : base(data)
    {
        minToxicWasteAmountOnDeath = data.minToxicWasteCloudAmount;
        maxToxicWasteAmountOnDeath = data.maxToxicWasteCloudAmount;
        CalculateCloudToxicWasteAmount();
        minGenerateToxicWasteCooldown = data.dropWasteMinCooldown;
        maxGenerateToxicWasteCooldown = data.dropWasteMaxCooldown;
        CalculateNextWasteDropTime();
    }

    public void CalculateNextWasteDropTime()
    {
        dropWasteCooldown = Random.Range(minGenerateToxicWasteCooldown, maxGenerateToxicWasteCooldown);
    }

    public void CalculateCloudToxicWasteAmount()
    {
        toxicWasteAmountOnDeath = Random.Range(minToxicWasteAmountOnDeath, maxToxicWasteAmountOnDeath);
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.Add(new EntitiyStat
        {
            statName = "Toxic Waste Cloud",
            currentStatValue = toxicWasteAmountOnDeath.ToString(),
            extraInformation = "Toxic wastes generated on death"
        });

        return stats;
    }
}
