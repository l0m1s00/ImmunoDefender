using System.Collections.Generic;
using UnityEngine;

public class CellSpawnerStats
{
    public ImmuneCellData soldierData;
    public CellSpawnerData spawnerData;
    protected int baseMaxSoldiers;
    public int maxSoldiers;
    public float spawnSoldierCooldown;
    public int availableSoldiers;
    public int totalSoldiers;

    public CellSpawnerStats(CellSpawnerData data)
    {
        if (data == null) return;

        soldierData = data.soldierDataSO;
        baseMaxSoldiers = data.nSoldiers;
        maxSoldiers = baseMaxSoldiers;
        spawnSoldierCooldown = data.spawnSoldierCooldown;
        totalSoldiers = baseMaxSoldiers;
        availableSoldiers = baseMaxSoldiers;
        spawnerData = data;
    }

    public List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat
        {
            statName = "Available Cells",
            currentStatValue = availableSoldiers.ToString()
        });

        stats.Add(new EntitiyStat
        {
            statName = "Deployed Cells",
            currentStatValue = (totalSoldiers - availableSoldiers).ToString()
        });

        return stats;
    }
}
