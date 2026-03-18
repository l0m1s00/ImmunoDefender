using UnityEngine;

[CreateAssetMenu(fileName = "ToxicWasteData", menuName = "Immuno Defender/Negative Items/Toxic Waste")]
public class ToxicWasteData : ScriptableObject
{
    [Header("Spawn Impulse Settings")]
    public float toxicAmount;
    public float ejectionSpeed;
    public float friction;
    [Header("Rewards")]
    public int reward;
}
