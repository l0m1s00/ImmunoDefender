using System.Collections.Generic;
using UnityEngine;

//modulo de generacion de unidades celulares
public class CellSpawnerModule : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform soldiersSpawnPoint;
    [SerializeField] private ParticleSystem spawnSoldierParticles;

    //total de celulas del modulo de generacion
    HashSet<ImmuneCell> immuneCellsInMap = new HashSet<ImmuneCell>();

    private Transform soldiersContainer;
    public CellSpawnerStats spawnerStats;
    private Tower parentTower;
    private float spawnTimer;

    public void Initialize(Tower tower, CellSpawnerStats stats)
    {
        parentTower = tower;
        spawnerStats = stats;
        spawnTimer = spawnerStats.spawnSoldierCooldown;
        soldiersContainer = GameObject.Find("InGameCells").transform;
    }

    public void HandleSpawning(bool canSpawn)
    {
        if (spawnTimer > 0f)
        {
            spawnTimer -= Time.deltaTime;
        }
        else if (canSpawn && spawnerStats.availableSoldiers > 0)
        {
            DeploySoldier();
            spawnTimer = spawnerStats.spawnSoldierCooldown;
        }
    }

    private void DeploySoldier()
    {
        spawnerStats.availableSoldiers--;
        parentTower.NotifyStatsChanged();

        GameObject newCellObj = PoolEntitiesManager.Instance.SpawnImmuneCell(spawnerStats.soldierData);
        newCellObj.transform.position = soldiersSpawnPoint.position;
        newCellObj.transform.SetParent(soldiersContainer);

        Enemy target = (parentTower is TargetingTower targeting) ? targeting.RequestTarget() : null;

        if (newCellObj.TryGetComponent<ImmuneCell>(out ImmuneCell cell))
        {
            cell.Initialize(parentTower, target, spawnerStats.soldierData);
            immuneCellsInMap.Add(cell);
        }
            

        if (spawnSoldierParticles != null) spawnSoldierParticles.Play();

        AudioManager.Instance.PlaySFX(spawnerStats.spawnerData.spawnCellSounds, transform.position);
    }

    public void ReturnCell(ImmuneCell cell)
    {
        immuneCellsInMap.Remove(cell);
        spawnerStats.availableSoldiers++;
        parentTower.NotifyStatsChanged();
    }

    //eliminar las celulas en juego asociadas a una torre
    public void ResetInMapTowerCells()
    {
        foreach (ImmuneCell cell in immuneCellsInMap)
        {
            PoolEntitiesManager.Instance.DespawnImmuneCell(cell.GetStats().data, cell.gameObject);
        }

        immuneCellsInMap.Clear();
    }
}
