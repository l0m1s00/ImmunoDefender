using UnityEngine;

public class FungiEnemy : Enemy
{
    [Header("Toxic Waste")]
    [SerializeField] private GameObject wastePrefab;
    [SerializeField] private float dropWasteMinCooldown;
    [SerializeField] private float dropWasteMaxCooldown;
    private Transform wasteContainer;

    private float dropWasteTimer = 0f;
    private float currentDropWasteTime = 0f;

    private void Start()
    {
        CalculateNextWasteDropTime();
        dropWasteTimer = Random.Range(1f, currentDropWasteTime);
        wasteContainer = GameObject.Find("WASTE_CONTAINER").transform;
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
        currentDropWasteTime = Random.Range(dropWasteMinCooldown, dropWasteMaxCooldown);
    }

    private void SpawnWaste()
    {
        Instantiate(wastePrefab, transform.position, Quaternion.identity, wasteContainer);
    }
}
