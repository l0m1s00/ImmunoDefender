using UnityEngine;

[CreateAssetMenu(fileName = "NK_TowerData", menuName = "Immuno Defender/Towers/Targeting Tower Data/Attacker Tower Data/NK Tower Data")]
public class NK_TowerData : TargetingTowerData
{
    [Header("Unit Generator Module Data")]
    public CellSpawnerData spawnerData;
}
