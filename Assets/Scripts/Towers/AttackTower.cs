using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTower : Tower
{
    [Header("Components")]
    [SerializeField] protected Transform soldiersSpawnPoint;
    [SerializeField] protected ParticleSystem spawnSoldierParticles;
    private Transform soldiersContainer;
    [Header("Optimization")]
    [SerializeField][Range(0.1f, 1f)] protected float scanInterval;
    //Stats
    protected ImmuneCellData soldierData;
    protected int availableSoldiers; //soldados disponibles para mandar
    protected int totalSoldiers; //cantidad de soldados de la torre, desplegados y dentro de la torre
    protected int maxSoldiers; //cantidad de celulas que puede enviar para combatir
    protected float range; //rango de la torre
    //Enemy Target
    protected Enemy currentTarget;
    private float scanTimer = 0f;
    //Deploy Soldier Settings
    protected float spawnSoldierCooldown;
    protected float spawnTimer = 0f;

    private void Awake()
    {
        soldiersContainer = GameObject.Find("InGameCells").transform;
    }

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        //pattern matching y polimorfismo:
        //aunque las cartas se creen empleando TowerDataSO, realmente lo que se pasa es el tipo de la torre, AttackerTowerDataSO
        //ese dataSO se enmascara como TowerDataSO pero internamente sigue siendo AttackerTowerDataSO
        //este if revela la verdadera identidad de TowerDataSO
        if (data is AttackerTowerData attackerData)
        {
            range = attackerData.range;
            maxSoldiers = attackerData.nSoldiers;
            soldierData = attackerData.soldierDataSO;
            spawnSoldierCooldown = attackerData.spawnSoldierCooldown;
            //actualizar rango visual de la torre
        }

        totalSoldiers = maxSoldiers;
        availableSoldiers = maxSoldiers;
    }

    protected virtual void Update()
    {
        HandleCurrentTarget();
        ScanForEnemies();
        HandleSoldiersSpawn();
    }

    protected void HandleSoldiersSpawn()
    {
        spawnTimer -= Time.deltaTime;

        if (currentTarget != null && availableSoldiers > 0 && spawnTimer <= 0f)
        {
            DeploySoldier();
            spawnTimer = spawnSoldierCooldown;
        }
    }

    protected virtual void DeploySoldier()
    {
        availableSoldiers--;

        GameObject newImmuneCellObj = PoolManager.Instance.SpawnImmuneCell(soldierData);
        newImmuneCellObj.transform.position = soldiersSpawnPoint.position;
        newImmuneCellObj.transform.SetParent(soldiersContainer);
        newImmuneCellObj.GetComponent<AttackerCell>().Initialize(this, currentTarget, soldierData);
        spawnSoldierParticles.Play();
    }

    protected void ScanForEnemies()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer < 0 && currentTarget == null)
        {
            scanTimer = scanInterval;

            //buscar objetivo
            currentTarget = GetClosestEnemyInRange();
        }
    }

    private Enemy GetClosestEnemyInRange()
    {
        Enemy closestEnemy = null;
        float shortestDistance = float.MaxValue;
        HashSet<Enemy> enemiesInBattlefield = EnemyManager.Instance.GetEnemiesInBattlefield();

        foreach (Enemy enemy in enemiesInBattlefield)
        {
            //enemigo a comparar desaparece por cualquier causa
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distanceToEnemy = Vector2.Distance(this.transform.position, enemy.transform.position);

            if (distanceToEnemy <= range && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    protected void HandleCurrentTarget()
    {
        if (currentTarget != null)
        {
            //el enemigo fijado ha desaparecido, por muerte o salida de rango
            if (!currentTarget.gameObject.activeInHierarchy || Vector2.Distance(transform.position, currentTarget.transform.position) > range)
            {
                currentTarget = null;
            }
        }
    }

    public void SoldierReturn()
    {
        availableSoldiers++;
    }

    public Enemy RequestTarget()
    {
        return currentTarget;
    }

    public float GetTowerRange()
    {
        return range;
    }

    public override void Reset()
    {
        base.Reset();

        availableSoldiers = 0;
        totalSoldiers = 0;
        maxSoldiers = 0;
        range = 0f;
        currentTarget = null;
        scanTimer = 0f;
        spawnSoldierCooldown = 0f;
    }

    private void OnDrawGizmosSelected() //DEBUG
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
