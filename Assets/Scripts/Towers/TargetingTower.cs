using System.Collections.Generic;
using UnityEngine;

public class TargetingTower : Tower
{
    [Header("Optimization")]
    [SerializeField][Range(0.1f, 1f)] protected float scanInterval;

    protected float range; //rango de la torre
    //Enemy Target
    protected Enemy currentTarget;
    private float scanTimer = 0f;

    public override void InitializeTower(TowerData data)
    {
        base.InitializeTower(data);

        if (data is TargetingTowerData targetingData)
        {
            range = targetingData.range;
        }
    }

    protected virtual void Update()
    {
        HandleCurrentTarget();
        ScanForEnemies();
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

        scanInterval = 0f;
        range = 0f;
        currentTarget = null;
        scanTimer = 0f;
    }
}
