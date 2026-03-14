using UnityEngine;

[CreateAssetMenu(fileName = "NeutrophilTowerData", menuName = "Immuno Defender/Towers/Attacker Tower Data/Neutrophil Tower Data")]
public class NeutrophilTowerData : AttackerTowerData
{
    [Header("Refill Soldiers Settings")]
    public float refillCooldown;
}
