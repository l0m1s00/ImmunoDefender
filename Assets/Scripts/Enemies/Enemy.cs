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
    [SerializeField] private Transform visualsPivotTransform; //lo que realmente gira es la carpeta que contiene los huesos y los renderers, para no interferir con las transformaciones locales de los huesos
    [SerializeField][Range(0f, 1f)] private float rotationSmoothTime;
    private float rotationVelocity;

    //knockback settings
    private float knockbackForce = 0.1f;
    private float knockbackRecoveryTime = 0.5f;

    //Knockback
    private Vector3 knockbackOffset = Vector3.zero;
    private Vector3 knockbackVelocity = Vector3.zero;
    private Vector3 currentMoveDirection = Vector3.zero;

    //Wave
    private Vector3 ghostPosition;
    private float randomMovementOffset;

    //Suavizado de curvas para el movimiento ondulatorio
    private Vector3 currentPerpendicular;
    private Vector3 perpendicularVelocity;

    //STATS
    protected EnemyStats basicStats;
    private bool isBeingInvestigated = false;
    private List<Cell> route;

    private int i = 0;

    //EVENTOS
    public static event Action<Enemy> OnEnemyRemovedFromBattlefield;
    public event Action OnStatsChanged;
    public event Action<ISelectable> OnEntityDestroyed;
    public static event Action<float> OnAttackVitalOrgan;

    //preparar al enemigo para lanzarlo en la partida
    public virtual void Initialize(List<Cell> newRoute, EnemyData enemyData)
    {
        //stats
        basicStats = new EnemyStats(enemyData);
        enemyUIRef.UpdateHealthBar(1f);
        //movimiento
        route = newRoute;
        ghostPosition = transform.position;
        randomMovementOffset = UnityEngine.Random.Range(0f, 100f);
        currentPerpendicular = Vector3.up;
    }

    protected virtual void Move(bool mustRotate)
    {
        Vector3 targetPosition = new Vector3(route[i].x, route[i].y, 0f);
        currentMoveDirection = (targetPosition - ghostPosition).normalized;

        if (currentMoveDirection != Vector3.zero && mustRotate)
        {
            //radianes a grados
            //-90 grados porque los enemigos son dibujados en vertical pero realmente su diseño es de lado
            float targetAngle = (Mathf.Atan2(currentMoveDirection.y, currentMoveDirection.x) * Mathf.Rad2Deg) + 90f;
            //interpolacion entre el angulo actual y el objetivo
            float currentAngle = visualsPivotTransform.eulerAngles.z;
            float smoothAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, rotationSmoothTime);

            visualsPivotTransform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
        }

        //suavizar cambios de direccion para la onda
        Vector3 targetPerpendicular = new Vector3(-currentMoveDirection.y, currentMoveDirection.x, 0f);
        currentPerpendicular = Vector3.SmoothDamp(currentPerpendicular, targetPerpendicular, ref perpendicularVelocity, turnSmoothTime);
        //falsa posicion que sigue el enemigo
        ghostPosition = Vector3.MoveTowards(ghostPosition, targetPosition, basicStats.speed * Time.deltaTime);
        //movimiento ondulatorio para el desplazamiento
        float wave = Mathf.Sin((Time.time * waveSpeed) + randomMovementOffset) * waveAmplitude;

        //si ha recibido un impulso por un ataque de una celula nos recuperamos rapidamente
        knockbackOffset = Vector3.SmoothDamp(knockbackOffset, Vector3.zero, ref knockbackVelocity, knockbackRecoveryTime);

        transform.position = ghostPosition + (currentPerpendicular * wave) + knockbackOffset;

        if (Vector3.Distance(ghostPosition, targetPosition) <= 0.2f)
        {
            if (i == route.Count - 1)
            {
                OnAttackVitalOrgan?.Invoke(basicStats.damage);
                RemoveEnemy();
                return;
            }

            i++;
        }

        //AudioManager.Instance.PlaySFX(basicStats.data.movementSounds, transform.position);
    }

    protected virtual void RemoveEnemy()
    {
        OnEntityDestroyed?.Invoke(this);
        OnEnemyRemovedFromBattlefield?.Invoke(this);
        PoolEntitiesManager.Instance.DespawnEnemy(basicStats.data, this.gameObject);
    }

    public void TakeDamage(float damage, Vector3 sourcePosition)
    {
        if (basicStats.currentHealth <= 0f) return;

        //anulamos Z para evitar problemas con la camara
        Vector3 flatPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 flatSource = new Vector3(sourcePosition.x, sourcePosition.y, 0f);

        //determir direccion del impacto
        Vector3 hitDirection = (transform.position - sourcePosition).normalized;

        //si el atacante esta justo encima del enemigo, por defecto se empuja en la direccion opuesta al movimiento
        if (hitDirection.sqrMagnitude == 0)
        {
            hitDirection = -currentMoveDirection;
        }

        //aplicar impulso en la direccion contraria al movimiento
        knockbackOffset = hitDirection * knockbackForce;
        //evitar acumulacion de inercia de SmoothDamp
        knockbackVelocity = Vector3.zero;

        enemyUIRef.HitFlash();

        if (basicStats.currentHealth - damage <= 0f)
        {
            EconomyManager.Instance.AddCurrency(basicStats.data.rewardDrop);
            RemoveEnemy();
        }
        else
        {
            basicStats.currentHealth -= damage;
            enemyUIRef.UpdateHealthBar(basicStats.currentHealth / basicStats.maxHealth);
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
        return basicStats.data;
    }

    public bool IsBeingInvestigated()
    {
        return isBeingInvestigated;
    }

    public void SetIsBeingInvestigated(bool investigating)
    {
        isBeingInvestigated = investigating;
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
        return basicStats.enemyName;
    }

    public virtual List<EntitiyStat> GetEntityStatsToDisplay()
    {
        return basicStats.GetStatsToDisplay();
    }

    public virtual void Reset() //dejar al enemigo en estado neutro antes de ponerlo en espera para su siguiente uso
    {
        perpendicularVelocity = Vector3.zero;
        currentPerpendicular = Vector3.zero;
        ghostPosition = Vector3.zero;

        knockbackOffset = Vector3.zero;
        knockbackVelocity = Vector3.zero;
        currentMoveDirection = Vector3.zero;

        rotationVelocity = 0f;
        visualsPivotTransform.rotation = Quaternion.identity;

        i = 0;
        isBeingInvestigated = false;
        DeactivateInvestigationBar();

        enemyUIRef.ResetUI();
    }
}
