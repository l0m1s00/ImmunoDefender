using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Immuno Defender/Enemies")]
public class EnemiesDataSO : ScriptableObject
{
    //STATS
    public float speed;
    public float health;
    public GameObject enemyPrefab;
}
