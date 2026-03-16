using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FungiEnemy : Enemy
{
    [Header("Toxic Waste Settings")]
    [SerializeField] private ToxicWasteData wasteData;
    private Transform wasteContainer;

    private int minToxicWasteAmountOnDeath = 4;
    private int maxToxicWasteAmountOnDeath = 8;
    private float dropWasteTimer = 0f;
    private float dropWasteCooldown;
    private Vector3 previousPosition;

    //EVENTOS
    public static event Action<float> OnIncreaseToxicBar;
    public static event Action<ToxicWaste> OnToxicWasteGenerated;

    private void Start()
    {
        CalculateNextWasteDropTime();
        dropWasteTimer = dropWasteCooldown;
        wasteContainer = GameObject.Find("InGameWaste").transform;
    }

    void Update()
    {
        previousPosition = transform.position;

        Move();
        DropWaste();
    }

    private void DropWaste()
    {
        dropWasteTimer -= Time.deltaTime;

        if (dropWasteTimer <= 0f)
        {
            ShotToxicWaste();
            CalculateNextWasteDropTime();
            dropWasteTimer = dropWasteCooldown;
        }
    }

    private void CalculateNextWasteDropTime()
    {
        //currentDropWasteTime = Random.Range(dropWasteMinCooldown, dropWasteMaxCooldown);
        dropWasteCooldown = Random.Range(4f, 10f);
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
        //tomar una direccion aleatoria alrededor del punto de muerte del fungi
        int toxicWasteAmount = Random.Range(minToxicWasteAmountOnDeath, maxToxicWasteAmountOnDeath + 1);

        for (int i = 0; i < toxicWasteAmount; i++)
        {
            Vector3 direction = (Vector3)Random.insideUnitCircle.normalized;

            CreateToxicWaste(direction);
        }
    }

    private void CreateToxicWaste(Vector3 direction)
    {
        GameObject newWaste = PoolManager.Instance.SpawnWaste();
        newWaste.transform.position = this.transform.position;
        newWaste.GetComponent<ToxicWaste>().Initialize(wasteData, direction);
        newWaste.transform.SetParent(wasteContainer);
        OnIncreaseToxicBar?.Invoke(wasteData.toxicAmount);
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
        CalculateNextWasteDropTime();
    }
}
