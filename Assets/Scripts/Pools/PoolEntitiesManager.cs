using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PoolEntitiesManager : MonoBehaviour
{
    //para optimizacion en la generacion de enemigos por oleada

    public static PoolEntitiesManager Instance;

    [Header("Enemy Pool Settings")]
    [SerializeField] private Transform enemiesPoolContainer;
    [SerializeField] private int initialEnemyPoolSize;
    private List<EnemyData> enemyTypes;
    [Header("Tower Pool Settings")]
    [SerializeField] private Transform towersPoolContainer;
    [SerializeField] private Transform ghostTowerPoolContainer;
    [SerializeField] private int initialTowersPoolSize;
    private List<TowerData> towerTypes;
    [Header("Immune Cell Pool Settings")]
    [SerializeField] private Transform immuneCellsPoolContainer;
    [SerializeField] private int initialImmuneCellPoolSize;
    private List<ImmuneCellData> immuneCellTypes;
    [Header("Consumable Items")]
    [SerializeField] private GameObject wastePrefab;
    [SerializeField] private Transform wastesPoolContainer;
    [SerializeField] private int initialWastePoolSize;
    [Header("Vital Organs Pool Settings")]
    [SerializeField] private Transform vitalOrgansPoolContainer;
    [SerializeField] private int initialVitalOrganPoolSize;
    private List<VitalOrganData> vitalOrganTypes;

    //pool de enemigos
    private Dictionary<EnemyData, ObjectPool> enemiesPool = new Dictionary<EnemyData, ObjectPool>();
    //pool de torres
    private Dictionary<TowerData, ObjectPool> towersPool = new Dictionary<TowerData, ObjectPool>();
    //pool de fantasmas torre
    private Dictionary<TowerData, ObjectPool> ghostTowersPool = new Dictionary<TowerData, ObjectPool>();
    //pool de celulas
    private Dictionary<ImmuneCellData, ObjectPool> immuneCellsPool = new Dictionary<ImmuneCellData, ObjectPool>();
    //pool de organos vitales
    private Dictionary<VitalOrganData, ObjectPool> vitalOrgansPool = new Dictionary<VitalOrganData, ObjectPool>();
    //pool de residuos
    private ObjectPool wastesPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        GetEntityTypes();

        CreateEnemiesPool();
        CreateTowersPool();
        CreateImmuneCellsPool();
        CreateConsumableItemsPool();
        CreateVitalOrgansPool();
    }

    private void GetEntityTypes()
    {
        enemyTypes = EntitiesLibrary.Instance.GetEnemyTypes();
        towerTypes = EntitiesLibrary.Instance.GetTowerTypes();
        immuneCellTypes = EntitiesLibrary.Instance.GetCellTypes();
        vitalOrganTypes = EntitiesLibrary.Instance.GetVitalOrgan();
    }

    //crear X cantidad de cada tipo de objeto y reservarlos
    private void CreateEnemiesPool()
    {
        foreach (EnemyData enemyDataSO in enemyTypes)
        {
            ObjectPool newPool = new ObjectPool(enemyDataSO.enemyPrefab, initialEnemyPoolSize, enemiesPoolContainer);
            enemiesPool.Add(enemyDataSO, newPool);
        }
    }

    private void CreateVitalOrgansPool()
    {
        foreach (VitalOrganData vitalOrganDataSO in vitalOrganTypes)
        {
            ObjectPool newPool = new ObjectPool(vitalOrganDataSO.vitalOrganPrefab, initialVitalOrganPoolSize, vitalOrgansPoolContainer);
            vitalOrgansPool.Add(vitalOrganDataSO, newPool);
        }
    }

    private void CreateTowersPool()
    {
        foreach (TowerData towerDataSO in towerTypes)
        {
            ObjectPool newTowerPool = new ObjectPool(towerDataSO.towerPrefab, initialTowersPoolSize, towersPoolContainer);
            towersPool.Add(towerDataSO, newTowerPool);
            ObjectPool newGhostTowerPool = new ObjectPool(towerDataSO.towerGhostPrefab, 1, ghostTowerPoolContainer);
            ghostTowersPool.Add(towerDataSO, newGhostTowerPool);
        }
    }

    private void CreateImmuneCellsPool()
    {
        foreach (ImmuneCellData immuneCellDataSO in immuneCellTypes)
        {
            ObjectPool newPool = new ObjectPool(immuneCellDataSO.immuneCellPrefab, initialImmuneCellPoolSize, immuneCellsPoolContainer);
            immuneCellsPool.Add(immuneCellDataSO, newPool);
        }
    }

    private void CreateConsumableItemsPool()
    {
        //residuos
        ObjectPool newPool = new ObjectPool(wastePrefab, initialWastePoolSize, wastesPoolContainer);
        wastesPool = newPool;
    }

    //coger una entidad, "Instantiate"
    public GameObject SpawnEnemy(EnemyData enemyData)
    {
        return enemiesPool[enemyData].Get();
    }

    public GameObject SpawnTower(TowerData towerData)
    {
        return towersPool[towerData].Get();
    }

    public GameObject SpawnGhostTower(TowerData towerData)
    {
        return ghostTowersPool[towerData].Get();
    }

    public GameObject SpawnImmuneCell(ImmuneCellData immuneCellData)
    {
        return immuneCellsPool[immuneCellData].Get();
    }

    public GameObject SpawnWaste()
    {
        return wastesPool.Get();
    }

    public GameObject SpawnVitalOrgan(VitalOrganData vitalOrganData)
    {
        return vitalOrgansPool[vitalOrganData].Get();
    }

    //devolver una entidad, "Destroy"
    public void DespawnEnemy(EnemyData enemyData, GameObject enemyObj)
    {
        enemiesPool[enemyData].Restore(enemyObj);
    }

    public void DespawnVitalOrgan(VitalOrganData vitalOrganData, GameObject vitalOrganObj)
    {
        vitalOrgansPool[vitalOrganData].Restore(vitalOrganObj);
    }

    public void DespawnTower(TowerData towerData, GameObject towerObj)
    {
        //no siempre que desaparece una torre la casilla del mapa debe liberarse, por lo que no informamos sobre la desaparicion de la torre
        towersPool[towerData].Restore(towerObj);
    }

    public void DespawnGhostTower(TowerData towerData, GameObject towerGhostObj)
    {
        ghostTowersPool[towerData].Restore(towerGhostObj);
    }

    public void DespawnImmuneCell(ImmuneCellData immuneCellData, GameObject immuneCellObj)
    {
        immuneCellsPool[immuneCellData].Restore(immuneCellObj);
    }

    public void DespawnWaste(GameObject wasteObj)
    {
        wastesPool.Restore(wasteObj);
    }
}
