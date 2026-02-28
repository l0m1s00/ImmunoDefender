using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //STATS
    private float speed;

    private List<Cell> route;
    private int i = 0;

    protected virtual void Move()
    {
        Vector3 targetPos = new Vector3(route[i].x, route[i].y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) <= 0.001f)
        {
            if (i == route.Count-1) //alcanzado el final de la ruta
            {
                Destroy(this.gameObject);//CAMBIAR
            }

            i++;
        }
    }

    public void SetStats(List<Cell> newRoute, float newSpeed)
    {
        route = newRoute;
        speed = newSpeed;
    }
}
