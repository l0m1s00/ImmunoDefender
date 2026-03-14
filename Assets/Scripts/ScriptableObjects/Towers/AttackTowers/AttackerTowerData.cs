using UnityEngine;

public class AttackerTowerData : TowerData
{
    [Header("Combat Stats")]
    public ImmuneCellData soldierDataSO;
    public int nSoldiers;
    public float range;
    [Header("Deploy Soldiers Settings")]
    public float spawnSoldierCooldown;
}
