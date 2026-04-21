using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TargetingTower : Tower, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optimization")]
    [SerializeField][Range(0.1f, 1f)] protected float scanInterval;

    //Enemy Target
    protected Enemy currentTarget;
    private float scanTimer = 0f;

    //Stats
    protected TargetingTowerStats targetingStats => (TargetingTowerStats)basicStats;

    public override void Initialize(TowerData data)
    {
        base.Initialize(data);

        if (data is TargetingTowerData targetingData)
        {
            basicStats = new TargetingTowerStats(targetingData);
        }

        towerUIRef.InitializeVisuals(targetingStats.range);
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

        //el rango de deteccion de la torre debe comenzar desde el limite de la casilla donde se construye
        //NO desde el centro de la propia torre
        float realDetectionRange = targetingStats.range + 0.5f;

        foreach (Enemy enemy in EnemyManager.Instance.GetEnemiesInBattlefield())
        {
            //enemigo a comparar desaparece por cualquier causa
            if (!IsValidTarget(enemy) || enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distanceToEnemy = Vector2.Distance(this.transform.position, enemy.transform.position);

            if (distanceToEnemy <= realDetectionRange && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    protected virtual bool IsValidTarget(Enemy enemy)
    {
        return true;
    }

    protected void HandleCurrentTarget()
    {
        if (currentTarget != null)
        {
            float realDetectionRange = targetingStats.range + 0.5f;

            //el enemigo fijado ha desaparecido, por muerte o salida de rango
            if (!currentTarget.gameObject.activeInHierarchy || Vector2.Distance(transform.position, currentTarget.transform.position) > realDetectionRange)
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
        return targetingStats.range + 0.5f; //rango real de la torre, desde el borde de la casilla, no desde el centro del objeto
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        towerUIRef.ShowVisualRange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        towerUIRef.HideVisualRange();
    }

    public override void Reset()
    {
        base.Reset();

        scanInterval = 0f;
        targetingStats.range = 0f;
        currentTarget = null;
        scanTimer = 0f;
        towerUIRef.Reset();
    }
}
