using UnityEngine;

public abstract class Tower : MonoBehaviour, IResettable
{
    protected TowerData baseData; //todas las torres tienen esta informacion

    public virtual void InitializeTower(TowerData data)
    {
        baseData = data;
    }

    public virtual void Reset() { }
}
