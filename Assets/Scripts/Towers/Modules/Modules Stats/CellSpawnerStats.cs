using System.Collections.Generic;
using UnityEngine;

public class CellSpawnerStats
{
    public ImmuneCellData soldierData;
    public CellSpawnerData spawnerData;
    public int baseMaxSoldiers;
    public int maxSoldiers;
    public float spawnSoldierCooldown;
    public int availableSoldiers;
    public int totalSoldiers;

    //total de celulas del modulo de generacion
    public HashSet<ImmuneCell> immuneCellsInBattlefield = new HashSet<ImmuneCell>();
    //diccionario para almacenar los beneficios que se les deben aplicar a las celulas por compra de mejoras
    public Dictionary<UpgradeType, float> currentCellBonuses = new Dictionary<UpgradeType, float>(); 

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

    public void ApplyUpgrade(TowerUpgradeData upgradeData, int upgradeLevel)
    {
        float amount = upgradeData.statIncrements[upgradeLevel];

        //mejoras del propio spawner
        if (upgradeData.upgradeType == UpgradeType.MaxSoldiers)
        {
            maxSoldiers += (int)amount;
            totalSoldiers += (int)amount;
            availableSoldiers += (int)amount;
        }
        else //mejoras que deben aplicarse directamente a las celulas, almacenar para las nuevas celulas creadas y propagar el beneficio a todas las activas
        {
            if (!currentCellBonuses.ContainsKey(upgradeData.upgradeType))
            {
                currentCellBonuses.Add(upgradeData.upgradeType, 0f);
            }

            currentCellBonuses[upgradeData.upgradeType] += amount;

            ApplyUpgradeToCellsInBattlefield(upgradeData.upgradeType, amount);
        }
    }

    public void ApplyUpgradeToCellsInBattlefield(UpgradeType upgradeType, float amount)
    {
        foreach (ImmuneCell cell in immuneCellsInBattlefield)
        {
            cell.ApplyUpgrade(upgradeType, amount);
        }
    }

    public List<EntitiyStat> GetStatsToDisplay()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat
        {
            statName = "Total Cells",
            initialStatValue = baseMaxSoldiers.ToString(),
            currentStatValue = maxSoldiers.ToString()
        });

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
