using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


///////////////////////////////  ENEMIES WAVE SYSTEM ///////////////////////////////////////////////
[System.Serializable]
public struct EnemyGroup
{
    public EnemiesDataSO enemyData; //tipo de enemigo del grupo
    public int count; //numero de enemigos del grupo
    public float timeBetweenEnemies; //delay entre enemigos del mismo grupo
    public float delayAfterGroup; //descanso al terminar de mandar al grupo
}

[System.Serializable]
public struct WaveConfig
{
    public List<EnemyGroup> enemyGroups; //gruppos de enemigos que componen una oleada
}
///////////////////////////////////////////////////////////////////////////////////////////////////

public class EnemySpawner : MonoBehaviour
{
    //DEBUG
    [SerializeField] private bool nextWave = false;

    [Header("Spawner Settings")]
    [SerializeField] private Transform enemiesContainer;
    
    [Header("Wave Configuration")]
    [SerializeField] private List<WaveConfig> waves;
    [SerializeField] private float delayBeforeWaveStarts;

    private List<Cell> enemyPath;
    private Vector2 spawnPoint;
    private int currentWave = 0;
    private Cell entryPoint;

    //EVENTOS
    public static event Action<string> OnStartWave;
    public static event Action<Enemy> OnEnemySpawn;

    private void OnEnable()
    {
        GridManager.OnMapGenerationCompleted += InitializeSpawner;
    }

    private void OnDisable()
    {
        GridManager.OnMapGenerationCompleted -= InitializeSpawner;
    }

    private void Update()
    {
        if (nextWave) StartWave();
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

    private void StartWave()
    {
        OnStartWave?.Invoke((currentWave+1).ToString()); //avisar del inicio de la oleada
        StartCoroutine(SpawnWave(waves[currentWave])); //soltar enemigos
    }

    private IEnumerator SpawnWave(WaveConfig wave)
    {
        nextWave = false; //DEBUG

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

        currentWave++;
    }

    private void InstantiateEnemy(EnemiesDataSO enemyData)
    {
        GameObject newEnemyObj = PoolManager.Instance.SpawnEnemy(enemyData);
        newEnemyObj.transform.position = spawnPoint;
        newEnemyObj.transform.SetParent(enemiesContainer);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
        newEnemy.InitializeEnemy(enemyPath, enemyData);
        OnEnemySpawn?.Invoke(newEnemy); //notificar que ha aparecido un enemigo
    }
}
