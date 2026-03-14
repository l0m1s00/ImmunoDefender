using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FungiEnemy : Enemy
{
    [Header("Toxic Waste Settings")]
    private Transform wasteContainer;

    private float dropWasteTimer = 0f;
    private float currentDropWasteTime = 0f;

    //EVENTOS
    public static event Action<float> OnToxicWasteGenerated;

    private void Start()
    {
        CalculateNextWasteDropTime();
        dropWasteTimer = Random.Range(1f, currentDropWasteTime);
        wasteContainer = GameObject.Find("InGameWaste").transform;
    }

    void Update()
    {
        Move();
        DropWaste();
    }

    private void DropWaste()
    {
        dropWasteTimer += Time.deltaTime;

        if (dropWasteTimer >= currentDropWasteTime)
        {
            dropWasteTimer = 0f;
            SpawnWaste();
            CalculateNextWasteDropTime();
        }
    }

    private void CalculateNextWasteDropTime()
    {
        //currentDropWasteTime = Random.Range(dropWasteMinCooldown, dropWasteMaxCooldown);
        currentDropWasteTime = Random.Range(2f, 10f);
    }

    private void SpawnWaste()
    {
        GameObject newWaste = PoolManager.Instance.SpawnWaste();
        newWaste.transform.position = this.transform.position;
        newWaste.transform.SetParent(wasteContainer);
        OnToxicWasteGenerated?.Invoke(1f);
    }

    public override void Reset()
    {
        base.Reset();

        dropWasteTimer = 0f;
        CalculateNextWasteDropTime();
    }
}
