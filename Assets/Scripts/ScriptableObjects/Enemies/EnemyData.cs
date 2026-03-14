using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("General Stats")]
    public float speed;
    public float health;
    public GameObject enemyPrefab;
}
