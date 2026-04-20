using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IResettable, ISelectable
{
    [Header("Components")]
    [SerializeField] private EnemyUI enemyUIRef;

    [Header("Movement Settings")]
    [SerializeField] private float waveAmplitude;
    [SerializeField] private float waveSpeed;
    [SerializeField] private float turnSmoothTime;

    //Wave
    private Vector3 ghostPosition;
    private float randomMovementOffset;

    //Suavizado de curvas
    private Vector3 currentPerpendicular;
    private Vector3 perpendicularVelocity;

    //STATS
    private EnemyData data;
    private float speed;
    private float maxHealth;
    private float currentHealth;
    private float damage;
    private bool isBeingInvestigated = false;

    private List<Cell> route;
    private int i = 0;

    //EVENTOS
    public static event Action<Enemy> OnEnemyRemovedFromBattlefield;
    public event Action OnStatsChanged;
    public event Action<ISelectable> OnEntityDestroyed;

    protected virtual void Move()
    {
        Vector3 targetPosition = new Vector3(route[i].x, route[i].y, 0f);
        Vector3 targetDirection = (targetPosition - ghostPosition).normalized;
        Vector3 targetPerpendicular = new Vector3(-targetDirection.y, targetDirection.x, 0f);

        //suavizar cambios de direccion
        currentPerpendicular = Vector3.SmoothDamp(currentPerpendicular, targetPerpendicular, ref perpendicularVelocity, turnSmoothTime);
        //falsa posicion que sigue el enemigo
        ghostPosition = Vector3.MoveTowards(ghostPosition, targetPosition, speed * Time.deltaTime);
        //movimiento ondulatorio para el desplazamiento
        float wave = Mathf.Sin((Time.time * waveSpeed) + randomMovementOffset) * waveAmplitude;
        
        transform.position = ghostPosition + (currentPerpendicular * wave);

        if (Vector3.Distance(ghostPosition, targetPosition) <= 0.2f)
        {
            if (i == route.Count - 1)
            {
                RemoveEnemy();
                return;
            }

            i++;
        }
    }

    //preparar al enemigo para lanzarlo en la partida
    public void InitializeEnemy(List<Cell> newRoute, EnemyData enemyData)
    {
        //stats
        data = enemyData;
        damage = data.damage;
        speed = data.speed;
        maxHealth = data.health;
        currentHealth = maxHealth;
        enemyUIRef.UpdateHealthBar(1f);
        //movimiento
        route = newRoute;
        ghostPosition = transform.position;
        randomMovementOffset = UnityEngine.Random.Range(0f, 100f);
        currentPerpendicular = Vector3.up;
    }

    protected virtual void RemoveEnemy()
    {
        OnEntityDestroyed?.Invoke(this);
        OnEnemyRemovedFromBattlefield?.Invoke(this);
        PoolManager.Instance.DespawnEnemy(data, this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f) return;

        if (currentHealth - damage <= 0f)
        {
            EconomyManager.Instance.AddCurrency(data.rewardDrop);
            RemoveEnemy();
        }
        else
        {
            currentHealth -= damage;
            enemyUIRef.UpdateHealthBar(currentHealth / maxHealth);
            OnStatsChanged?.Invoke();
        }
    }

    public void UpdateInvestigationBar(float amount)
    {
        enemyUIRef.UpdateInvestigationFill(amount);
    }

    public void DeactivateInvestigationBar()
    {
        enemyUIRef.DeactivateInvestigationFill();
    }

    public EnemyData GetData()
    {
        return data;
    }

    public bool IsBeingInvestigated()
    {
        return isBeingInvestigated;
    }

    public void SetIsBeingInvestigated(bool investigating)
    {
        isBeingInvestigated = investigating;
    }

    public virtual void Reset() //dejar al enemigo en estado neutro antes de ponerlo en espera para su siguiente uso
    {
        perpendicularVelocity = Vector3.zero;
        currentPerpendicular = Vector3.zero;
        ghostPosition = Vector3.zero;
        i = 0;
        isBeingInvestigated = false;
        DeactivateInvestigationBar();
    }

    public virtual void OnSelect()
    {
        enemyUIRef.SelectEnemy();
    }

    public virtual void OnDeselect()
    {
        enemyUIRef.DeselectEnemy();
    }

    public virtual string GetEntityName()
    {
        return data.enemyName;
    }

    public virtual List<EntitiyStat> GetStats()
    {
        List<EntitiyStat> stats = new List<EntitiyStat>();

        stats.Add(new EntitiyStat { statName = "Health", currentStatValue = currentHealth.ToString() });
        stats.Add(new EntitiyStat { statName = "Speed", currentStatValue = speed.ToString() });
        stats.Add(new EntitiyStat { statName = "Damage", currentStatValue = damage.ToString() });

        return stats;
    }
}
