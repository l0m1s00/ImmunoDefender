using System.Collections.Generic;
using UnityEngine;

public class CellularFarmTower : Tower
{
    private CellSpawnerModule spawnerModule;
    private float generateCurrencyTimer;

    //Infection Settings
    private List<StemCell> healthyStemCells = new List<StemCell>();
    private HashSet<StemCell> infectedStemCells = new HashSet<StemCell>();
    private bool isTotallyInfected = false; //cuando todas las celulas madre estan infectadas la granja se corrompe
    private float corruptionTimer;

    //Stats
    protected CellularFarmTowerStats cellularFarmTowerStats => (CellularFarmTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is CellularFarmTowerData cellularFarmData)
        {
            basicStats = new CellularFarmTowerStats(cellularFarmData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, cellularFarmTowerStats.spawnerStats);
            generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
            corruptionTimer = cellularFarmTowerStats.infectionCooldown;
        }

        towerUIRef.InitializeVisuals(0f);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Playing) return;

        if (isTotallyInfected) HandleCorruption();
        else
        {
            HandleEconomyGeneration();
            spawnerModule.HandleSpawning(true);
        }
    }

    public void AddNewHealthyStemCell(StemCell newStemCell)
    {
        healthyStemCells.Add(newStemCell);
    }

    private void HandleEconomyGeneration()
    {
        if (generateCurrencyTimer > 0f) generateCurrencyTimer -= Time.deltaTime;
        else
        {
            GenerateGenetics();
            generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
        }
    }

    private void GenerateGenetics()
    {
        //solo las celulas madre sanas pueden generar recursos
        int totalGeneticsToGenerate = healthyStemCells.Count * cellularFarmTowerStats.generatedCurrencyAmountPerCell;

        if (totalGeneticsToGenerate > 0)
        {
            EconomyManager.Instance.AddCurrency(totalGeneticsToGenerate);
        }
    }

    public bool ResolveInfectionAttempt(int virusInfectionPower)
    {
        if (isTotallyInfected) return false;

        int infectionProbability = Mathf.Clamp(virusInfectionPower - cellularFarmTowerStats.resistanceToInfection,cellularFarmTowerStats.minGetInfectionProbability, 100);
        int roll = Random.Range(1, 101);
        bool success = roll < infectionProbability;

        if (success)
        {
            InfectHealthyStemCell();
        }

        return success;
    }

    public void InfectHealthyStemCell()
    {
        if (healthyStemCells.Count > 0)
        {
            int r = Random.Range(0, healthyStemCells.Count);
            StemCell newInfectedCell = healthyStemCells[r];
            newInfectedCell.GetInfection();
            healthyStemCells.Remove(newInfectedCell);
            infectedStemCells.Add(newInfectedCell);

            CheckCellularFarmCorruption();
        }
    }

    private void CheckCellularFarmCorruption()
    {
        if (healthyStemCells.Count == 0 && infectedStemCells.Count == cellularFarmTowerStats.totalStemCells)
        {
            isTotallyInfected = true;
        }
    }

    private void HandleCorruption()
    {
        corruptionTimer -= Time.deltaTime;
        if (corruptionTimer <= 0f) ExplodeCorruptedFarm();
    }

    private void ExplodeCorruptedFarm()
    {
        if (cellularFarmTowerStats.baseData is CellularFarmTowerData data)
        {
            Instantiate(data.infectionSpawnerObj, transform.position, Quaternion.identity); //CAMBIAR PARA QUE LO GESTIONE EL POOL
        }

        List<StemCell> allStemCells = new List<StemCell>();
        allStemCells.AddRange(healthyStemCells);
        allStemCells.AddRange(infectedStemCells);

        foreach (StemCell stemCell in allStemCells)
        {
            PoolManager.Instance.DespawnImmuneCell(stemCell.GetCellStats().data, stemCell.gameObject);
        }

        TowerManager.Instance.DeleteTower(this);
        PoolManager.Instance.DespawnTower(cellularFarmTowerStats.baseData, this.gameObject);
    }

    public override void Reset()
    {
        base.Reset();

        healthyStemCells.Clear();
        infectedStemCells.Clear();
        corruptionTimer = 0f;
        isTotallyInfected = false;
        generateCurrencyTimer = 0f;
    }
}
