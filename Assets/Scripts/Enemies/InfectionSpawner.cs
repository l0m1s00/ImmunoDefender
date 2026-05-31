using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class InfectionSpawner : MonoBehaviour
{
    [Header("Infection Swarm Settings")]
    [SerializeField] private VirionEnemyData virionData;
    [SerializeField] private int nVirion;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float initialDelay;

    private List<Cell> enemiesSubRoute = new List<Cell>();
    private Cell initialCell; //celda en la que se posicionan los enemigos del enjambre infeccioso

    //EVENTOS
    public static event Action<Enemy> OnEnemySpawn;

    private void Start()
    {
        FindInitialCell();

        if (initialCell != null)
        {
            CalculateSubRoute();
            StartCoroutine(SpawnEnemy());
        }
    }

    private void FindInitialCell() //prioriza la casilla mas cercana al final del camino
    {
        GridGraph graph = GridManager.Instance.GetCompleteGraph();
        if (graph == null) return;

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        Vector2Int[] directionPriorities = new Vector2Int[] //se posiciona al enemigo lo mas cercano al punto final del camino
        {
            new Vector2Int(1, 0),   //derecha
            new Vector2Int(0, -1),  //abajo
            new Vector2Int(0, 1),   //arriba
            new Vector2Int(-1, 0)  //izquierda
        };

        foreach (Vector2Int direction in directionPriorities)
        {
            Cell cell = graph.GetCell(x + direction.x, y + direction.y);

            if (cell != null && EnemySpawner.Instance.GetEnemyPath().Contains(cell))
            {
                initialCell = cell;
                break;
            }
        }
    }

    private void CalculateSubRoute()
    {
        List<Cell> completeEnemyPath = EnemySpawner.Instance.GetEnemyPath();
        //buscar la casilla de aparicion de los enemigos y tomar el camino desde ese punto hasta el final
        int ii = completeEnemyPath.IndexOf(initialCell);
        int fi = completeEnemyPath.Count - ii;
        enemiesSubRoute = completeEnemyPath.GetRange(ii, fi);
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < nVirion; i++)
        {
            GameObject virionObj = PoolEntitiesManager.Instance.SpawnEnemy(virionData);
            virionObj.transform.position = this.transform.position;

            if (virionObj.TryGetComponent<VirionEnemy>(out VirionEnemy virion))
            {
                virion.Initialize(enemiesSubRoute, virionData);
            }

            OnEnemySpawn?.Invoke(virion);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        //la casilla finalmente ha sido liberada
        GridManager.Instance.UnblockCell(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
        Destroy(gameObject); //tras soltar el enjambre el spawn se destruye
    }
}
