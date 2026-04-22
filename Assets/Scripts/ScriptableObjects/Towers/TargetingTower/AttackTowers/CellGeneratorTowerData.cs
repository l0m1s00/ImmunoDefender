using UnityEngine;

public abstract class CellGeneratorTowerData : TargetingTowerData
{
    [Header("Cell Soldiers Stats")]
    public ImmuneCellData soldierDataSO;
    public int nSoldiers;
    [Header("Deploy Soldiers Settings")]
    public float spawnSoldierCooldown;
}
