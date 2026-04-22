using UnityEngine;

[CreateAssetMenu(fileName = "NeutrophilTowerData", menuName = "Immuno Defender/Towers/Targeting Tower Data/Attacker Tower Data/Neutrophil Tower Data")]
public class NeutrophilTowerData : CellGeneratorTowerData
{
    [Header("Refill Soldiers Settings")]
    public float refillCooldown;
}
