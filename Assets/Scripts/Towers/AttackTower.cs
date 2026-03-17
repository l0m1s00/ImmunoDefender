using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTower : TargetingTower
{
    [Header("Components")]
    [SerializeField] protected Transform soldiersSpawnPoint;
    [SerializeField] protected ParticleSystem spawnSoldierParticles;
    private Transform soldiersContainer;
    //Stats
    protected ImmuneCellData soldierData;
    protected int availableSoldiers; //soldados disponibles para mandar
    protected int totalSoldiers; //cantidad de soldados de la torre, desplegados y dentro de la torre
    protected int maxSoldiers; //cantidad de celulas que puede enviar para combatir
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

    protected override void Update()
    {
        base.Update();
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

    public void SoldierReturn()
    {
        availableSoldiers++;
    }

    public override void Reset()
    {
        base.Reset();

        availableSoldiers = 0;
        totalSoldiers = 0;
        maxSoldiers = 0;
        spawnSoldierCooldown = 0f;
    }

    private void OnDrawGizmosSelected() //DEBUG
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
