using System.Collections.Generic;
using UnityEngine;

public class VirionEnemy : Enemy
{
    [SerializeField] private float spinSpeed = 360f; //MOVER AL SO DE DATOS
    [SerializeField] private SpriteRenderer spriteRenderer; //MOVER
    protected VirionEnemyStats virionStats => (VirionEnemyStats)basicStats;

    public override void Initialize(List<Cell> route, EnemyData data)
    {
        base.Initialize(route, data);

        if (data is VirionEnemyData virionData)
        {
            basicStats = new VirionEnemyStats(virionData);
        }

        float randomStartAngle = Random.Range(0f, 360f);
        spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, randomStartAngle);
    }

    private void Update()
    {
        Move(false);
        spriteRenderer.transform.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
    }

    public override void Reset()
    {
        base.Reset();
    }
}
