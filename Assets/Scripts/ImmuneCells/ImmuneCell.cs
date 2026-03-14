using UnityEngine;

public abstract class ImmuneCell : MonoBehaviour
{
    protected Tower headquarters; //torre que ha generado la celula
    protected GameObject currentTarget;

    protected float speed;

    protected virtual void Update()
    {

    }

    public virtual void Initialize(Tower parentTower, GameObject target, ImmuneCellData data)
    {
        headquarters = parentTower;
        currentTarget = target;
        speed = data.speed;
    }

    protected virtual void MoveTowardsTarget()
    {
        if (currentTarget == null) return;

        this.transform.position = Vector2.MoveTowards(this.transform.position, currentTarget.transform.position, speed * Time.deltaTime);
    }
}
