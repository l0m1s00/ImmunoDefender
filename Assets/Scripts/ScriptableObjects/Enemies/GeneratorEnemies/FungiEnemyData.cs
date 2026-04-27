using UnityEngine;

[CreateAssetMenu(fileName = "FungiEnemyData", menuName = "Immuno Defender/Enemies/Generator Enemy/Fungi Enemy Data")]
public class FungiEnemyData : GeneratorEnemyData
{
    [Header("Toxic Waste Settings")]
    public int minToxicWasteCloudAmount;
    public int maxToxicWasteCloudAmount;
}
