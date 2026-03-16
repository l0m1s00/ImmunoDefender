using UnityEngine;

//distintos estados en los que puede estar una celula (soldado)
public enum State
{
    None,
    Chase,
    Attack,
    Return,
    Clean, //Exclusivo de los macrofagos
}

public abstract class ImmuneCell : MonoBehaviour, IResettable
{
    protected ImmuneCellData data;
    protected Tower headquarters; //torre que ha generado la celula
    protected Enemy currentTarget;

    protected float speed;

    protected virtual void Update()
    {
        
    }

    public virtual void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        headquarters = parentTower;
        currentTarget = target;
        speed = data.speed;
        this.data = data;
    }

    protected virtual void MoveTowardsTarget(GameObject target)
    {
        if (target == null) return;

        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public virtual void Reset()
    {
        headquarters = null;
        currentTarget = null;
    }
}
