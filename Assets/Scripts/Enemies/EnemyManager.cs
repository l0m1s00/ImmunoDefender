using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private HashSet<Enemy> enemiesInBattlefield = new HashSet<Enemy>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawn += AddEnemy;
        Enemy.OnEnemyRemovedFromBattlefield += RemoveEnemy;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawn -= AddEnemy;
        Enemy.OnEnemyRemovedFromBattlefield -= RemoveEnemy;
    }

    private void AddEnemy(Enemy newEnemyInBattlefield)
    {
        enemiesInBattlefield.Add(newEnemyInBattlefield);
    }

    private void RemoveEnemy(Enemy defeatedEnemy)
    {
        enemiesInBattlefield.Remove(defeatedEnemy);
    }

    public HashSet<Enemy> GetEnemiesInBattlefield()
    {
        return enemiesInBattlefield;
    }
}
