using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class AttackTower : TargetingTower
{
    [Header("Components")]
    [SerializeField] protected Transform soldiersSpawnPoint;
    [SerializeField] protected ParticleSystem spawnSoldierParticles;
    private Transform soldiersContainer;
    //Deploy Soldier Settings
    protected float spawnTimer = 0f;

    //Stats
    protected AttackTowerStats attackStats => (AttackTowerStats)basicStats;

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
            basicStats = new AttackTowerStats(attackerData);
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleSoldiersSpawn();
    }

    protected void HandleSoldiersSpawn()
    {
        spawnTimer -= Time.deltaTime;

        if (currentTarget != null && attackStats.availableSoldiers > 0 && spawnTimer <= 0f)
        {
            DeploySoldier();
            spawnTimer = attackStats.spawnSoldierCooldown;
        }
    }

    protected virtual void DeploySoldier()
    {
        attackStats.availableSoldiers--;

        GameObject newImmuneCellObj = PoolManager.Instance.SpawnImmuneCell(attackStats.soldierData);
        newImmuneCellObj.transform.position = soldiersSpawnPoint.position;
        newImmuneCellObj.transform.SetParent(soldiersContainer);
        newImmuneCellObj.GetComponent<AttackerCell>().Initialize(this, currentTarget, attackStats.soldierData);
        spawnSoldierParticles.Play();
    }

    public void SoldierReturn()
    {
        attackStats.availableSoldiers++;
    }

    public override void Reset()
    {
        base.Reset();

        attackStats.availableSoldiers = 0;
        attackStats.totalSoldiers = 0;
        attackStats.maxSoldiers = 0;
        attackStats.spawnSoldierCooldown = 0f;
    }

    private void OnDrawGizmosSelected() //DEBUG
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackStats.range);
    }
}
