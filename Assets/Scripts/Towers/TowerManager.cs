using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private HashSet<Tower> towers = new HashSet<Tower>(); //torres construidas
    private HashSet<CellularFarmTower> cellularFarmTowers = new HashSet<CellularFarmTower>();

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
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= AddTower;
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

    public void DeleteTower(Tower towerToEliminate)
    {
        towers.Remove(towerToEliminate);

        if (towerToEliminate is CellularFarmTower cellularFarmTowerToEliminate) cellularFarmTowers.Remove(cellularFarmTowerToEliminate);
    }

    public HashSet<Tower> GetBuiltTowers()
    {
        return towers;
    }

    public HashSet<CellularFarmTower> GetBuiltCellularFarmTowers()
    {
        return cellularFarmTowers;
    }
}
