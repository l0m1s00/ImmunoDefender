using System.Collections.Generic;
using UnityEngine;

public class CellularFarmTower : Tower
{
    private float generateCurrencyTimer;

    //Stats
    protected CellularFarmTowerStats cellularFarmTowerStats => (CellularFarmTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is CellularFarmTowerData cellularFarmData)
        {
            basicStats = new CellularFarmTowerStats(cellularFarmData);
        }

        generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
        towerUIRef.InitializeVisuals(0f);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Playing) return;

        if (generateCurrencyTimer > 0f) generateCurrencyTimer -= Time.deltaTime;
        else
        {
            GenerateGenetics();
            generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
        }
    }

    private void GenerateGenetics()
    {
        int totalGeneticsToGenerate = cellularFarmTowerStats.totalStemCells * cellularFarmTowerStats.generatedCurrencyAmountPerCell;

        if (totalGeneticsToGenerate > 0)
        {
            EconomyManager.Instance.AddCurrency(totalGeneticsToGenerate);
        }
    }

    public override List<EntitiyStat> GetStats()
    {
        return cellularFarmTowerStats.GetStats();
    }

    public override void Reset()
    {
        base.Reset();

        generateCurrencyTimer = 0f;
    }
}
