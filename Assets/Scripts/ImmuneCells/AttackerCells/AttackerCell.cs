using UnityEngine;

public abstract class AttackerCell : ImmuneCell
{
    protected float damage;
    protected float attackRange;
    protected float attackCooldown;

    protected float attackTimer = 0f;
    protected bool isReturning = false;

    public override void Initialize(Tower parentTower, GameObject target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is AttackerImmuneCellData attackerData)
        {
            damage = attackerData.damage;
            attackRange = attackerData.attackRange;
            attackCooldown = attackerData.attackCooldown;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

        if (isReturning)
        {
            ReturnToHeadquarters();
        }
        else
        {
            HandleCombat();
        }
    }

    protected virtual void HandleCombat()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy || IsTargetOutOfTowerRange())
        {
            RequestNewTarget();

            if (currentTarget == null)
            {
                isReturning = true;
                return;
            }
        }

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);
    
        if (distanceToTarget > attackRange)
        {
            MoveTowardsTarget();
        }
        else if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    //comprueba si el enemigo sigue en los limites de la torre
    private bool IsTargetOutOfTowerRange()
    {
        if (headquarters is AttackTower attackTower && currentTarget != null)
        {
            float distanceToTower = Vector2.Distance(attackTower.transform.position, currentTarget.transform.position);
            
            if (distanceToTower > attackTower.GetTowerRange())
            {
                currentTarget = null;
                return true;
            }
        }

        return false;
    }

    protected virtual void ReturnToHeadquarters()
    {
        RequestNewTarget();
        if (currentTarget != null)
        {
            isReturning = false;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, headquarters.transform.position, speed * Time.deltaTime);
    
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

    protected virtual void Attack()
    {
        
    }

    //solicitar a la torre padre un nuevo objetivo
    public void RequestNewTarget()
    {
        if (headquarters is AttackTower attackTower)
        {
            Enemy newTarget = attackTower.RequestTarget();
            
            if (newTarget != null)
            {
                currentTarget = newTarget.gameObject;
            }
        }
    }
}
