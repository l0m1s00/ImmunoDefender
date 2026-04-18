using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("General Stats")]
    public string enemyName;
    public float damage;
    public float speed;
    public float health;
    public int rewardDrop;
    public GameObject enemyPrefab;
}
