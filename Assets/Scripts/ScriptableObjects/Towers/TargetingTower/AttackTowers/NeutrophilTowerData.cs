using UnityEngine;

[CreateAssetMenu(fileName = "NeutrophilTowerData", menuName = "Immuno Defender/Towers/Targeting Tower Data/Attacker Tower Data/Neutrophil Tower Data")]
public class NeutrophilTowerData : TargetingTowerData
{
    [Header("Unit Generator Module Data")]
    public CellSpawnerData spawnerData;

    [Header("Refill Soldiers Settings")]
    public float refillCooldown;
    public AudioEventData reloadSoldierSounds;

    [Header("Info Provider Texts")]
    public string reloadSoldierText;
    public string allSoldiersGeneratedText;
}
