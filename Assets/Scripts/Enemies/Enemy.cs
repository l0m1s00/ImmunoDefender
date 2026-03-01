using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    private float speed;

    private List<Cell> route;
    private int i = 0;

    protected virtual void Move()
    {
        Vector3 targetPosition = new Vector3(route[i].x, route[i].y, 0f);
        Vector3 targetDirection = (targetPosition - ghostPosition).normalized;
        Vector3 targetPerpendicular = new Vector3(-targetDirection.y, targetDirection.x, 0f);

        currentPerpendicular = Vector3.SmoothDamp(currentPerpendicular, targetPerpendicular, ref perpendicularVelocity, turnSmoothTime);
        
        ghostPosition = Vector3.MoveTowards(ghostPosition, targetPosition, speed * Time.deltaTime);
        float wave = Mathf.Sin((Time.time * waveSpeed) + randomMovementOffset) * waveAmplitude;
        transform.position = ghostPosition + (targetPerpendicular * wave);

        if (Vector3.Distance(ghostPosition, targetPosition) <= 0.1f)
        {
            if (i == route.Count - 1)
            {
                Destroy(this.gameObject);
                return;
            }

            i++;
        }
    }

    public void InitializeEnemy(List<Cell> newRoute, float newSpeed)
    {
        route = newRoute;
        speed = newSpeed;
        ghostPosition = transform.position;
        randomMovementOffset = Random.Range(0f, 100f);
        currentPerpendicular = Vector3.up;
    }
}
