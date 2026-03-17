using UnityEngine;

public abstract class AttackerTowerData : TargetingTowerData
{
    [Header("Combat Stats")]
    public ImmuneCellData soldierDataSO;
    public int nSoldiers;
    [Header("Deploy Soldiers Settings")]
    public float spawnSoldierCooldown;
}
