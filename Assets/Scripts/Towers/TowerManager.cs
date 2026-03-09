using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private List<GameObject> towers = new List<GameObject>();

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += AddTower;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= AddTower;
    }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    public void AddTower(GameObject newTower) 
    {
        towers.Add(newTower);
        //Debug.Log("Torres construidas: " + towers.Count);
    }

    public void DeleteTower(GameObject towerToEliminate)
    {
        towers.Remove(towerToEliminate);
    }
}
