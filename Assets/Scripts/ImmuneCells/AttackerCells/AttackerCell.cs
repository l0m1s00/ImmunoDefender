using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackerCell : ImmuneCell
{
    protected AttackerCellStats attackerStats => (AttackerCellStats)basicStats;
    protected float attackTimer = 0f;
    protected State currentState = State.None;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is AttackerImmuneCellData attackerData)
        {
            basicStats = new AttackerCellStats(attackerData);
            currentState = State.Chase;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

        HandleActionsFSM();
        ActionsFSM();
    }

    protected virtual void ActionsFSM()
    {
        switch (currentState)
        {
            case State.Chase:
                ChaseEnemyActions();
                break;
            case State.Attack:
                AttackActions();
                break;
            case State.Return:
                ReturnActions();
                break;
            default:
                break;
        }
    }

    protected virtual void HandleActionsFSM()
    {
        switch (currentState)
        {
            case State.Chase:
                HandleChaseState();
                break;
            case State.Attack:
                HandleAttackState();
                break;
            case State.Return:
                HandleReturnState();
                break;
            default:
                break;
        }
    }

    protected void MoveTowardsTarget(GameObject target)
    {
        if (target == null) return;

        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, basicStats.speed * Time.deltaTime);
    }

    private void ChaseEnemyActions()
    {
        MoveTowardsTarget(currentTarget.gameObject);
    }

    private void HandleChaseState()
    {
        if (!IsCurrentTargetValid())
        {
            currentTarget = null;
            RequestNewTarget();

            if (currentTarget == null)
            {
                currentState = State.Return;
                return;
            }
        }

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget <= attackerStats.attackRange)
        {
            currentState = State.Attack;
            return;
        }
    }

    private void AttackActions()
    {
        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackerStats.attackCooldown;
        }
    }

    protected virtual void Attack()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy) return;

        float totalDamage = attackerStats.damage + CalculateCurrentDamageBuffsAgainstEnemy();
        currentTarget.TakeDamage(totalDamage, transform.position);
    }

    private float CalculateCurrentDamageBuffsAgainstEnemy()
    {
        float damageBonus = 0f;

        foreach (Tower tower in TowerManager.Instance.GetBuiltTowers()) //revisar las torres cercanas para buscar alguna que proporcione buffos
        {
            if (tower is BLymphocyteTower bLymphocyteTower)
            {
                float distanceToTower = Vector2.Distance(headquarters.transform.position, bLymphocyteTower.transform.position);

                if (distanceToTower <= bLymphocyteTower.GetTowerRange())
                {
                    damageBonus += bLymphocyteTower.GetDamageBonusAgainstEnemy(currentTarget.GetData());
                }
            }
        }

        return damageBonus;
    }

    private Dictionary<EnemyData, float> CalculateDamageBuffsAgainstAllEnemies()
    {
        //tomamos todos los bufos de daño de todas las torres linfocitosB al alcance y recordamos por cada grupo de bufos el enemigo al que afectan
        Dictionary<EnemyData, float> totalBonuses = new Dictionary<EnemyData, float>();

        foreach (EnemyData enemyType in DataLibrary.Instance.GetEnemyTypes())
        {
            totalBonuses.Add(enemyType, 0f);
        }

        foreach (Tower tower in TowerManager.Instance.GetBuiltTowers())
        {
            if (tower is BLymphocyteTower bLymphocyteTower)
            {
                float distanceToTower = Vector2.Distance(headquarters.transform.position, bLymphocyteTower.transform.position);
                //float sqrDistance = (headquarters.transform.position - bLymphocyteTower.transform.position).sqrMagnitude;
                //float towerSqrRange = bLymphocyteTower.GetTowerRange() * bLymphocyteTower.GetTowerRange();

                if (distanceToTower <= bLymphocyteTower.GetTowerRange())
                {
                    foreach (KeyValuePair<EnemyData, float> enemyKey in bLymphocyteTower.GetAllDamageBonuses())
                    {
                        //if (totalBonuses.ContainsKey(enemyKey.Key)) totalBonuses[enemyKey.Key] += enemyKey.Value;
                        //else totalBonuses[enemyKey.Key] = enemyKey.Value;
                        totalBonuses[enemyKey.Key] += enemyKey.Value;
                    }
                }
            }
        }

        return totalBonuses;
    }

    public override List<EntitiyStat> GetStats()
    {
        Dictionary<EnemyData, float> damageBonuses = CalculateDamageBuffsAgainstAllEnemies();
        return attackerStats.GetStats(damageBonuses);
    }

    private void HandleAttackState()
    {
        if (!IsCurrentTargetValid())
        {
            currentTarget = null;
            RequestNewTarget();

            if (currentTarget == null)
            {
                currentState = State.Return;
                return;
            }

            currentState = State.Chase;
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget > attackerStats.attackRange)
        {
            currentState = State.Chase;
        }
    }

    private void ReturnActions()
    {
        ReturnToHeadquarters();
    }

    protected virtual void ReturnToHeadquarters()
    {
        MoveTowardsTarget(headquarters.gameObject);
    }

    private void HandleReturnState()
    {
        RequestNewTarget();

        if (currentTarget != null)
        {
            currentState = State.Chase;
            return;
        }
        if (Vector2.Distance(transform.position, headquarters.transform.position) < 0.1f)
        {
            EnterTower();
        }
    }

    private void EnterTower()
    {
        CellSpawnerModule spawnerModule = headquarters.GetComponent<CellSpawnerModule>();
        spawnerModule.ReturnCell();
        NotifyEntityDestruction();
        PoolEntitiesManager.Instance.DespawnImmuneCell(attackerStats.data, this.gameObject);
    }

    //comprueba si el enemigo sigue en los limites de la torre
    private bool IsTargetOutOfTowerRange()
    {
        if (headquarters is TargetingTower targetingTower && currentTarget != null)
        {
            float distanceToTower = Vector2.Distance(targetingTower.transform.position, currentTarget.transform.position);
            
            if (distanceToTower > targetingTower.GetTowerRange())
            {
                return true;
            }
        }

        return false;
    }

    protected bool IsCurrentTargetValid()
    {
        if (currentTarget == null) return false;
        if (!currentTarget.gameObject.activeInHierarchy) return false;
        if (IsTargetOutOfTowerRange()) return false;

        return true;
    }


    //solicitar a la torre padre un nuevo objetivo
    public void RequestNewTarget()
    {
        if (headquarters is TargetingTower targetingTower)
        {
            Enemy newTarget = targetingTower.RequestTarget();
            
            if (newTarget != null)
            {
                currentTarget = newTarget;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        attackerStats.damage = 0f;
        attackerStats.attackRange = 0f;
        attackerStats.attackCooldown = 0f;
        attackTimer = 0f;
        currentState = State.None;
    }
}
