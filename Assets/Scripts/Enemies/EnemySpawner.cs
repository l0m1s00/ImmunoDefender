using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Spawner Settings")]
    [SerializeField] private Transform enemiesContainer;
    [SerializeField] private float delayBeforeWaveStarts;

    private List<Cell> enemyPath;
    private Vector2 spawnPoint;
    private Cell entryPoint;


    //EVENTOS
    public static event Action<Enemy> OnEnemySpawn;
    public static event Action OnWaveEnded;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GridManager.OnMapGenerationCompleted += InitializeSpawner;
        LevelManager.OnStartWaveCurrentWaveData += StartWave;
    }

    private void OnDisable()
    {
        GridManager.OnMapGenerationCompleted -= InitializeSpawner;
        LevelManager.OnStartWaveCurrentWaveData -= StartWave;
    }

    private void Update()
    {

    }

    private void InitializeSpawner()
    {
        entryPoint = GridManager.Instance.GetGraphEntryPoint();
        spawnPoint = new Vector2(entryPoint.x, entryPoint.y);
        CalculateEnemyPath(); //determinar camino por el grafo que los enemigos deben seguir
    }

    private void CalculatePathDFS(Cell currentCell, HashSet<Cell> visitedCells)
    {
        if (visitedCells.Contains(currentCell))
        {
            enemyPath.Add(currentCell);
            return;
        }

        visitedCells.Add(currentCell);
        enemyPath.Add(currentCell);

        foreach (Cell nextCell in currentCell.nextCells)
        {
            CalculatePathDFS(nextCell, visitedCells);
        }
    }

    private void CalculateEnemyPath()
    {
        enemyPath = new List<Cell>();  
        HashSet<Cell> visitedCells = new HashSet<Cell>();
        CalculatePathDFS(entryPoint, visitedCells);
    }

    private void StartWave(WaveConfig currentWave)
    {
        StartCoroutine(SpawnWave(currentWave)); //soltar enemigos
    }

    private IEnumerator SpawnWave(WaveConfig wave)
    {
        yield return new WaitForSeconds(delayBeforeWaveStarts); //tiempo de espera antes de empezar la oleada

        foreach (EnemyGroup enemyGroup in wave.enemyGroups)
        {
            for (int i = 0; i < enemyGroup.count; i++)
            {
                InstantiateEnemy(enemyGroup.enemyData);

                if (i < enemyGroup.count - 1)
                    yield return new WaitForSeconds(enemyGroup.timeBetweenEnemies);
            }

            if (enemyGroup.delayAfterGroup > 0f)
                yield return new WaitForSeconds(enemyGroup.delayAfterGroup);
        }

        while (EnemyManager.Instance.GetEnemiesInBattlefield().Count > 0) { yield return new WaitForSeconds(0.5f); } //2 comprobaciones por segundo 

        OnWaveEnded?.Invoke();
    }

    private void InstantiateEnemy(EnemyData enemyData)
    {
        GameObject newEnemyObj = PoolEntitiesManager.Instance.SpawnEnemy(enemyData);
        newEnemyObj.transform.position = spawnPoint;
        newEnemyObj.transform.SetParent(enemiesContainer);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
        newEnemy.Initialize(enemyPath, enemyData);
        OnEnemySpawn?.Invoke(newEnemy); //notificar que ha aparecido un enemigo
    }

    public List<Cell> GetEnemyPath()
    {
        return enemyPath;
    }
}
