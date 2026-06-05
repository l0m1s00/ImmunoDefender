using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CellularFarmTower : Tower, IStateInfoProvider
{
    private CellSpawnerModule spawnerModule;
    private float generateCurrencyTimer;
    private bool canGenerate = false;

    //Informacion para el provider
    private string currentStateText;
    private float possibleInfectionTextTimer = 0f;
    private int maxCorruptionTime = 0; //tiempo maximo posible ante una posible infeccion

    //Infection Settings
    private List<StemCell> healthyStemCells = new List<StemCell>();
    private HashSet<StemCell> infectedStemCells = new HashSet<StemCell>();
    private bool isTotallyInfected = false; //cuando todas las celulas madre estan infectadas la granja se corrompe
    private float corruptionTimer;

    //EVENTOS
    public event Action<string> OnStateInfoChanged;

    private CellularFarmTowerData towerData;
    //Stats
    protected CellularFarmTowerStats cellularFarmTowerStats => (CellularFarmTowerStats)basicStats;

    private void OnEnable()
    {
        LevelManager.OnStartWave += StartGeneticsGeneration;
        EnemySpawner.OnWaveEnded += StopGeneticsGeneration;
    }

    private void OnDisable()
    {
        LevelManager.OnStartWave -= StartGeneticsGeneration;
        EnemySpawner.OnWaveEnded -= StopGeneticsGeneration;
    }

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is CellularFarmTowerData cellularFarmData)
        {
            basicStats = new CellularFarmTowerStats(cellularFarmData);
            spawnerModule = GetComponent<CellSpawnerModule>();
            spawnerModule.Initialize(this, cellularFarmTowerStats.spawnerStats);
            spawnerModule.OnNewCellGenerated += NotifyStatsChanged;
            generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
            corruptionTimer = cellularFarmTowerStats.infectionCooldown;
            towerData = cellularFarmData;
        }

        canGenerate = LevelManager.Instance.HasWaveStarted();

        currentStateText = towerData.noInfection;
        towerUIRef.InitializeVisualRange(0f);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Playing) return;

        if (isTotallyInfected) HandleCorruption();
        else
        {
            if (canGenerate) HandleEconomyGeneration();
            spawnerModule.HandleSpawning(true);
        }

        if (possibleInfectionTextTimer > 0f)
        {
            possibleInfectionTextTimer -= Time.deltaTime;
        }
        else
        {
            if (currentStateText != towerData.noInfection) currentStateText = towerData.noInfection;
        }
    }

    private void StartGeneticsGeneration()
    {
        canGenerate = true;
    }

    private void StopGeneticsGeneration()
    {
        canGenerate = false;
        generateCurrencyTimer = cellularFarmTowerStats.generatedCurrencyInterval;
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
            EconomyManager.Instance.AddImmediateCurrency(totalGeneticsToGenerate);
        }
    }

    public bool ResolveInfectionAttempt(int virusInfectionPower) //virus intenta infectar la granja
    {
        if (isTotallyInfected) return false;

        ProviderTextSettings();

        //realmente es una probabilidad, probabilidad de infectar - resistencia (probabilidad negativa) de la granja
        int infectionProbability = Mathf.Clamp(virusInfectionPower - cellularFarmTowerStats.resistanceToInfection,cellularFarmTowerStats.minGetInfectionProbability, 100);
        int roll = UnityEngine.Random.Range(1, 101); //si sale el valor se infecta
        bool success = roll < infectionProbability;

        if (success)
        {
            InfectHealthyStemCell();
        }

        return success;
    }

    private void ProviderTextSettings() //la informacion extra "posible infeccion" debido a la deteccion de un virus hace poco, el ultimo, se mantiene un determinado tiempo
    {
        float infectionTime = 0f;

        //tomar el tiempo que tarda una celula madre en infectar a otra, culquiera
        if (healthyStemCells.Count > 0)
        {
            infectionTime = healthyStemCells[0].GetCellStats().getInfectionCooldown;
        }
        else
        {
            foreach (StemCell cell in infectedStemCells)
            {
                infectionTime = cell.GetCellStats().getInfectionCooldown;
                break;
            }
        }

        maxCorruptionTime = Mathf.CeilToInt(Mathf.Log(cellularFarmTowerStats.spawnerStats.maxSoldiers, 2f)) * Mathf.CeilToInt(infectionTime);
        possibleInfectionTextTimer = maxCorruptionTime;
        
        if (currentStateText != towerData.possibleInfection) currentStateText = towerData.possibleInfection;

        OnStateInfoChanged?.Invoke(currentStateText);
    }

    public void InfectHealthyStemCell()
    {
        if (healthyStemCells.Count > 0)
        {
            int r = UnityEngine.Random.Range(0, healthyStemCells.Count);
            StemCell newInfectedCell = healthyStemCells[r];
            newInfectedCell.GetInfection();
            healthyStemCells.Remove(newInfectedCell);
            infectedStemCells.Add(newInfectedCell);

            CheckCellularFarmCorruption();
        }
    }

    public bool CureInfectedStemCells() //la celula NK cura a las celulas madre infectadas
    {
        currentStateText = towerData.noInfection;
        OnStateInfoChanged?.Invoke(currentStateText);

        if (infectedStemCells.Count > 0)
        {
            foreach (StemCell cell in infectedStemCells)
            {
                cell.CureInfection();
                healthyStemCells.Add(cell);
            }

            isTotallyInfected = false;
            corruptionTimer = cellularFarmTowerStats.infectionCooldown;
            infectedStemCells.Clear();

            return true;
        }

        return false;
    }

    private void CheckCellularFarmCorruption()
    {
        if (healthyStemCells.Count == 0 && infectedStemCells.Count == cellularFarmTowerStats.spawnerStats.maxSoldiers)
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

        TowerManager.Instance.DeleteTower(this, false);
    }

    public string GetCurrentStateInfo()
    {
        return currentStateText;
    }

    public override void Reset()
    {
        spawnerModule.OnNewCellGenerated -= NotifyStatsChanged;
        spawnerModule.ResetInMapTowerCells();

        base.Reset();

        healthyStemCells.Clear();
        infectedStemCells.Clear();
        corruptionTimer = 0f;
        isTotallyInfected = false;
        generateCurrencyTimer = 0f;
        canGenerate = false;

        currentStateText = towerData.noInfection;
        possibleInfectionTextTimer = 0f;
        maxCorruptionTime = 0;
    }
}
