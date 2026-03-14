using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private List<Tower> towers = new List<Tower>(); //torres construidas

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
    }

    private void DeleteTower(Tower towerToEliminate)
    {
        towers.Remove(towerToEliminate);
    }
}
