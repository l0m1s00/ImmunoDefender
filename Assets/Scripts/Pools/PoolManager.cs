using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    //para optimizacion en la generacion de enemigos por oleada

    public static PoolManager Instance;

    [Header("Enemy Pool Settings")]
    [SerializeField] private List<EnemyData> enemyTypes;
    [SerializeField] private Transform enemyPoolContainer;
    [SerializeField] private int initialEnemyPoolSize;
    [Header("Tower Pool Settings")]
    [SerializeField] private List<TowerData> towerTypes;
    [SerializeField] private Transform towerPoolContainer;
    [SerializeField] private Transform ghostTowerPoolContainer;
    [SerializeField] private int initialTowersPoolSize;
    [Header("Immune Cell Pool Settings")]
    [SerializeField] private List<ImmuneCellData> immuneCellTypes;
    [SerializeField] private Transform immuneCellPoolContainer;
    [SerializeField] private int initialImmuneCellPoolSize;
    [Header("Consumable Items")]
    [SerializeField] private GameObject wastePrefab;
    [SerializeField] private Transform wastePoolContainer;
    [SerializeField] private int initialWastePoolSize;

    //pool de enemigos
    private Dictionary<EnemyData, ObjectPool> enemyPool = new Dictionary<EnemyData, ObjectPool>();
    //pool de torres
    private Dictionary<TowerData, ObjectPool> towerPool = new Dictionary<TowerData, ObjectPool>();
    //pool de fantasmas torre
    private Dictionary<TowerData, ObjectPool> ghostTowerPool = new Dictionary<TowerData, ObjectPool>();
    //pool de celulas
    private Dictionary<ImmuneCellData, ObjectPool> immuneCellPool = new Dictionary<ImmuneCellData, ObjectPool>();
    //pool de residuos
    private ObjectPool wastePool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        CreateEnemyPools();
        CreateTowerPools();
        CreateImmuneCellPool();
        CreateConsumableItemsPool();
    }

    //crear X cantidad de cada tipo de objeto y reservarlos
    private void CreateEnemyPools()
    {
        foreach (EnemyData enemyDataSO in enemyTypes)
        {
            ObjectPool newPool = new ObjectPool(enemyDataSO.enemyPrefab, initialEnemyPoolSize, enemyPoolContainer);
            enemyPool.Add(enemyDataSO, newPool);
        }
    }

    private void CreateTowerPools()
    {
        foreach (TowerData towerDataSO in towerTypes)
        {
            ObjectPool newTowerPool = new ObjectPool(towerDataSO.towerPrefab, initialTowersPoolSize, towerPoolContainer);
            towerPool.Add(towerDataSO, newTowerPool);
            ObjectPool newGhostTowerPool = new ObjectPool(towerDataSO.towerGhostPrefab, 1, ghostTowerPoolContainer);
            ghostTowerPool.Add(towerDataSO, newGhostTowerPool);
        }
    }

    private void CreateImmuneCellPool()
    {
        foreach (ImmuneCellData immuneCellDataSO in immuneCellTypes)
        {
            ObjectPool newPool = new ObjectPool(immuneCellDataSO.immuneCellPrefab, initialImmuneCellPoolSize, immuneCellPoolContainer);
            immuneCellPool.Add(immuneCellDataSO, newPool);
        }
    }

    private void CreateConsumableItemsPool()
    {
        //residuos
        ObjectPool newPool = new ObjectPool(wastePrefab, initialWastePoolSize, wastePoolContainer);
        wastePool = newPool;
    }

    //coger un objeto, "Instantiate"
    public GameObject SpawnEnemy(EnemyData enemyData)
    {
        return enemyPool[enemyData].Get();
    }

    public GameObject SpawnTower(TowerData towerData)
    {
        return towerPool[towerData].Get();
    }

    public GameObject SpawnGhostTower(TowerData towerData)
    {
        return ghostTowerPool[towerData].Get();
    }

    public GameObject SpawnImmuneCell(ImmuneCellData immuneCellData)
    {
        return immuneCellPool[immuneCellData].Get();
    }

    public GameObject SpawnWaste()
    {
        return wastePool.Get();
    }

    //devolver un enemigo, "Destroy"
    public void DespawnEnemy(EnemyData enemyData, GameObject enemyObj)
    {
        enemyPool[enemyData].Restore(enemyObj);
    }

    public void DespawnTower(TowerData towerData, GameObject towerObj)
    {
        towerPool[towerData].Restore(towerObj);
    }

    public void DespawnGhostTower(TowerData towerData, GameObject towerGhostObj)
    {
        ghostTowerPool[towerData].Restore(towerGhostObj);
    }

    public void DespawnImmuneCell(ImmuneCellData immuneCellData, GameObject immuneCellObj)
    {
        immuneCellPool[immuneCellData].Restore(immuneCellObj);
    }

    public void DespawnWaste(GameObject wasteObj)
    {
        wastePool.Restore(wasteObj);
    }
}
