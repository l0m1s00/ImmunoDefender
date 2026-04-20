using UnityEngine;

public class TargetingTowerStats : BasicTowerStats
{
    protected float baseRange; 
    public float range; //rango de la torre

    public TargetingTowerStats(TargetingTowerData data) : base(data)
    {
        baseRange = data.range;
        range = baseRange;
    }
}
