using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FungiEnemy : Enemy
{
    //[Header("Toxic Waste Settings")]
    private Transform wasteContainer;
    private Vector3 previousPosition;

    //Stats
    protected FungiEnemyStats fungiStats => (FungiEnemyStats)basicStats;
    private float dropWasteTimer = 0f;

    //EVENTOS
    public static event Action<ToxicWaste> OnToxicWasteGenerated;

    private void Start()
    {
        wasteContainer = GameObject.Find("InGameWaste").transform;
    }

    public override void Initialize(List<Cell> route, EnemyData data)
    {
        base.Initialize(route, data);

        if (data is FungiEnemyData fungiData)
        {
            basicStats = new FungiEnemyStats(fungiData);
            dropWasteTimer = fungiStats.dropWasteCooldown;
        }
    }

    void Update()
    {
        previousPosition = transform.position;

        Move(true);
        DropWaste();
    }

    private void DropWaste()
    {
        dropWasteTimer -= Time.deltaTime;

        if (dropWasteTimer <= 0f)
        {
            ShotToxicWaste();
            fungiStats.CalculateNextWasteDropTime();
            dropWasteTimer = fungiStats.dropWasteCooldown;
        }
    }

    private void ShotToxicWaste()
    {
        //determinar la direccion opuesta al movimineto del fungi
        Vector3 currentMovementDirection = (transform.position - previousPosition).normalized;
        Vector3 oppositeDirection = -currentMovementDirection;
        float randomSpread = Random.Range(-3f, 3f);
        oppositeDirection = Quaternion.Euler(0f, 0f, randomSpread) * oppositeDirection;
        
        CreateToxicWaste(oppositeDirection);
    }

    private void SpawnToxicCloud()
    {
        for (int i = 0; i < fungiStats.toxicWasteAmountOnDeath; i++)
        {
            //tomar una direccion aleatoria alrededor del punto de muerte del fungi
            Vector3 direction = (Vector3)Random.insideUnitCircle.normalized;
            CreateToxicWaste(direction);
        }
    }

    private void CreateToxicWaste(Vector3 direction)
    {
        GameObject newWaste = PoolEntitiesManager.Instance.SpawnWaste();
        newWaste.transform.position = this.transform.position;
        newWaste.GetComponent<ToxicWaste>().Initialize(direction);
        newWaste.transform.SetParent(wasteContainer);
        OnToxicWasteGenerated?.Invoke(newWaste.GetComponent<ToxicWaste>());
    }

    protected override void RemoveEnemy()
    {
        SpawnToxicCloud();
        base.RemoveEnemy();
    }

    public override void Reset()
    {
        base.Reset();

        dropWasteTimer = 0f;
    }
}
