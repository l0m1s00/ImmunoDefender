using UnityEngine;

[CreateAssetMenu(fileName = "VirusEnemyData", menuName = "Immuno Defender/Enemies/Virus Enemy Data")]
public class VirusEnemyData : EnemyData
{
    [Header("Infection Settings")]
    [Range(1, 100)] public int infectionProbability;
}
