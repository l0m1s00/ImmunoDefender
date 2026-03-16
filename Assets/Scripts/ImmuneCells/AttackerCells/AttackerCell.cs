using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackerCell : ImmuneCell
{
    protected float damage;
    protected float attackRange;
    protected float attackCooldown;

    protected float attackTimer = 0f;

    protected State currentState = State.None;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is AttackerImmuneCellData attackerData)
        {
            damage = attackerData.damage;
            attackRange = attackerData.attackRange;
            attackCooldown = attackerData.attackCooldown;
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

        if (distanceToTarget <= attackRange)
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
            attackTimer = attackCooldown;
        }
    }

    protected virtual void Attack()
    {
        currentTarget.TakeDamage(damage);
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

        if (distanceToTarget > attackRange)
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
        if (headquarters is AttackTower attackTowerHome) attackTowerHome.SoldierReturn();
        Destroy(gameObject);
    }

    //comprueba si el enemigo sigue en los limites de la torre
    private bool IsTargetOutOfTowerRange()
    {
        if (headquarters is AttackTower attackTower && currentTarget != null)
        {
            float distanceToTower = Vector2.Distance(attackTower.transform.position, currentTarget.transform.position);
            
            if (distanceToTower > attackTower.GetTowerRange())
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
        if (headquarters is AttackTower attackTower)
        {
            Enemy newTarget = attackTower.RequestTarget();
            
            if (newTarget != null)
            {
                currentTarget = newTarget;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        damage = 0f;
        attackRange = 0f;
        attackCooldown = 0f;
        attackTimer = 0f;
        currentState = State.None;
    }
}
