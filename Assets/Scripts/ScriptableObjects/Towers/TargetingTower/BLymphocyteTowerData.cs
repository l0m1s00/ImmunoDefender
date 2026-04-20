using UnityEngine;

[CreateAssetMenu(fileName = "BLymphocyteTowerData", menuName = "Immuno Defender/Towers/Targeting Tower Data/BLymphocyte Tower Data")]
public class BLymphocyteTowerData : TargetingTowerData
{
    [Header("Investigate Enemy Settings")]
    public float investigationAmount;
    public float investigationInterval;
    public float damageBonus;
}
