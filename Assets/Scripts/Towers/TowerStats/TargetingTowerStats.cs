using UnityEngine;

public class TargetingTowerStats : BasicTowerStats
{
    public float range; //rango de la torre

    public TargetingTowerStats(TargetingTowerData data) : base(data)
    {
        range = data.range;
    }
}
