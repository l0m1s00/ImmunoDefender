using System.Collections.Generic;
using UnityEngine;

public class GlowBallPathIndicator : MonoBehaviour, IResettable
{
    [SerializeField] private float speed;
    private List<Cell> route;
    private int i = 0;

    public void Initialize(List<Cell> enemiesPath)
    {
        route = enemiesPath;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 targetPosition = new Vector3(route[i].x, route[i].y, 0f);
        transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, speed * Time.deltaTime);
    
        if (Vector3.Distance(this.transform.position, targetPosition) <= 0.2f)
        {
            if (i == route.Count - 1)
            {
                RemoveGlowBall();
                return;
            }

            i++;
        }
    }

    public void RemoveGlowBall()
    {
        PoolEntitiesManager.Instance.DespawnGlowBall(this.gameObject);
    }

    public void Reset()
    {
        i = 0;
    }
}
