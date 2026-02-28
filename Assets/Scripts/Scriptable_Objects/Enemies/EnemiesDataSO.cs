using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Immuno Defender/Enemies")]
public class EnemiesDataSO : ScriptableObject
{
    public float speed;
    public GameObject enemyPrefab;
}
