using System.Collections.Generic;
using UnityEngine;

public class CellularFarmTowerStats : BasicTowerStats
{
    protected int baseGeneratedCurrencyAmountPerCell;
    public int generatedCurrencyAmountPerCell;
    protected float baseGeneratedCurrencyInterval;
    public float generatedCurrencyInterval;
    private int baseResistanceToInfection;
    public int resistanceToInfection;
    public int minGetInfectionProbability;
    public float infectionCooldown;

    public CellSpawnerStats spawnerStats;

    public CellularFarmTowerStats(CellularFarmTowerData data) : base(data)
    {
        baseGeneratedCurrencyAmountPerCell = data.generatedCurrencyAmountPerCell;
        generatedCurrencyAmountPerCell = baseGeneratedCurrencyAmountPerCell;
        baseGeneratedCurrencyInterval = data.generatedCurrencyInterval;
        generatedCurrencyInterval = baseGeneratedCurrencyInterval;
        baseResistanceToInfection = data.resistanceToInfection;
        resistanceToInfection = baseResistanceToInfection;
        minGetInfectionProbability = data.minGetInfectionProbability;
        infectionCooldown = data.farmInfectionCooldown;

        spawnerStats = new CellSpawnerStats(data.spawnerData);
    }

    public override void ApplyUpgrade(TowerUpgradeData upgradeData, int upgradeLevel)
    {
        base.ApplyUpgrade(upgradeData, upgradeLevel);

        if (upgradeData.upgradeType == UpgradeType.FarmResistance)
        {
            resistanceToInfection += (int)upgradeData.statIncrements[upgradeLevel];
        }
        else if (upgradeData.upgradeType == UpgradeType.FarmAmountGeneration)
        {
            generatedCurrencyAmountPerCell += (int)upgradeData.statIncrements[upgradeLevel];
        }
    }

    public override List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = base.GetStatsToDisplay();

        stats.Add(new EntitiyStat
        {
            statName = "Stem Cells",
            initialStatValue = spawnerStats.baseMaxSoldiers.ToString(),
            currentStatValue = spawnerStats.maxSoldiers.ToString(),
            extraInformation = ""
        });
        stats.Add(new EntitiyStat
        {
            statName = "Generated Genetics per cell",
            initialStatValue = (baseGeneratedCurrencyAmountPerCell / baseGeneratedCurrencyInterval).ToString() + " / s",
            currentStatValue = (generatedCurrencyAmountPerCell / generatedCurrencyInterval).ToString() + " / s",
            extraInformation = ""
        });
        stats.Add(new EntitiyStat
        {
            statName = "Total Generated Genetics",
            initialStatValue = ((baseGeneratedCurrencyAmountPerCell / baseGeneratedCurrencyInterval) * spawnerStats.baseMaxSoldiers).ToString() + " / s",
            currentStatValue = ((generatedCurrencyAmountPerCell / generatedCurrencyInterval) * spawnerStats.maxSoldiers).ToString() + " / s",
            extraInformation = "Total amount of Genetics generated per second"
        });
        stats.Add(new EntitiyStat
        {
            statName = "Resistance to infection",
            initialStatValue = baseResistanceToInfection.ToString(),
            currentStatValue = resistanceToInfection.ToString(),
            extraInformation = "To determine whether an infection is successful, this resistance is compared against the enemy’s infectious attack. There is always a minimal chance of infection (" + minGetInfectionProbability.ToString() + "%)"
        });

        return stats;
    }
}
