using System.Collections.Generic;
using UnityEngine;

public class CellularFarmTowerStats : BasicTowerStats
{
    protected int baseGeneratedCurrencyAmountPerCell;
    public int generatedCurrencyAmountPerCell;
    protected float baseGeneratedCurrencyInterval;
    public float generatedCurrencyInterval;
    protected int baseStemCells;
    public int totalStemCells;

    public CellSpawnerStats spawnerStats;

    public CellularFarmTowerStats(CellularFarmTowerData data) : base(data)
    {
        baseGeneratedCurrencyAmountPerCell = data.generatedCurrencyAmountPerCell;
        generatedCurrencyAmountPerCell = baseGeneratedCurrencyAmountPerCell;
        baseGeneratedCurrencyInterval = data.generatedCurrencyInterval;
        generatedCurrencyInterval = baseGeneratedCurrencyInterval;

        spawnerStats = new CellSpawnerStats(data.spawnerData);

        //baseStemCells = data.nStemCells;
        baseStemCells = spawnerStats.totalSoldiers;
        totalStemCells = baseStemCells;
    }

    public override List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = base.GetStats();

        stats.Add(new EntitiyStat
        {
            statName = "Stem Cells",
            initialStatValue = baseStemCells.ToString(),
            currentStatValue = totalStemCells.ToString(),
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
            initialStatValue = ((baseGeneratedCurrencyAmountPerCell * baseStemCells) / baseGeneratedCurrencyInterval).ToString() + " / s",
            currentStatValue = ((generatedCurrencyAmountPerCell * totalStemCells) / generatedCurrencyInterval).ToString() + " / s",
            extraInformation = "Total amount of Genetics generated per second"
        });

        return stats;
    }
}
