using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private HashSet<Tower> towers = new HashSet<Tower>(); //torres construidas
    private HashSet<CellularFarmTower> cellularFarmTowers = new HashSet<CellularFarmTower>();
    private HashSet<CellularFarmTower> currentInvestigatedFarms = new HashSet<CellularFarmTower>();

    //EVENTOS
    public static event Action OnNewCellularFarmBuilt;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += AddTower;
        TowerUpgradeManager.OnTowerDemolish += WrapperDeleteTower;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= AddTower;
        TowerUpgradeManager.OnTowerDemolish -= WrapperDeleteTower;
    }

    private void Update()
    {
        
    }

    private void AddTower(Tower newTower) 
    {
        towers.Add(newTower);

        if (newTower is CellularFarmTower newCellularFarmTower)
        {
            cellularFarmTowers.Add(newCellularFarmTower);
            OnNewCellularFarmBuilt?.Invoke();
        }
    }

    private void WrapperDeleteTower(Tower towerToEliminate)
    {
        DeleteTower(towerToEliminate);
    }

    public void DeleteTower(Tower towerToEliminate, bool unblockGridCell = true)
    {
        towers.Remove(towerToEliminate);

        //eliminar una granja celular NO debe liberar la casilla, ya que se crea una estructura generadora de enemigos
        if (towerToEliminate is CellularFarmTower cellularFarmTowerToEliminate)
        {
            cellularFarmTowers.Remove(cellularFarmTowerToEliminate);
            currentInvestigatedFarms.Remove(cellularFarmTowerToEliminate);
        }

        if (unblockGridCell) GridManager.Instance.UnblockCell(Mathf.RoundToInt(towerToEliminate.transform.position.x), Mathf.RoundToInt(towerToEliminate.transform.position.y));

        PoolEntitiesManager.Instance.DespawnTower(towerToEliminate.GetTowerStats().baseData, towerToEliminate.gameObject);
    }

    public HashSet<Tower> GetBuiltTowers()
    {
        return towers;
    }

    public HashSet<CellularFarmTower> GetBuiltCellularFarmTowers()
    {
        return cellularFarmTowers;
    }

    public void AddInvestigatedFarm(CellularFarmTower farm)
    {
        currentInvestigatedFarms.Add(farm);
    }

    public void RemoveInvestigatedFarm(CellularFarmTower farm)
    {
        currentInvestigatedFarms.Remove(farm);
    }

    public bool IsFarmInvestigated(CellularFarmTower farm)
    {
        return currentInvestigatedFarms.Contains(farm);
    }
}
