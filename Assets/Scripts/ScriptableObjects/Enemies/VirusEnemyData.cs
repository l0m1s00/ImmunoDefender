using UnityEngine;

[CreateAssetMenu(fileName = "VirusEnemyData", menuName = "Immuno Defender/Enemies/Virus Enemy Data")]
public class VirusEnemyData : EnemyData
{
    [Header("Infection Settings")]
    public int infectionProbability;
}
