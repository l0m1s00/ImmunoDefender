using UnityEngine;

[CreateAssetMenu(fileName = "MacrophageTowerData", menuName = "Immuno Defender/Towers/Targeting Tower Data/Attacker Tower Data/Macrophage Tower Data")]
public class MacrophageTowerData : TargetingTowerData
{
    [Header("Unit Generator Module Data")]
    public CellSpawnerData spawnerData;
}
