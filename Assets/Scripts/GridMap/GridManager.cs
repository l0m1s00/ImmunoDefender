using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public bool restart; //debug

    public static GridManager Instance;

    private GridGraph gridGraph;
    private PathGenerator pathGenerator;
    private List<Cell> pathCells;
    private HashSet<Cell> towerCells = new HashSet<Cell>();
    private int pathSize;

    [Header("Cell Containers")]
    [SerializeField] private GameObject towerHolderTileContainer;
    [SerializeField] private GameObject pathTileContainer;
    [SerializeField] private GameObject groundTileContainer;
    [Header("Cell Objects")]
    [SerializeField] private GridCellDataSO[] pathCellObjects;
    [SerializeField] private GridCellDataSO[] levelCellObjects;
    [Header("Map Settings")]
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int minPathLength;
    [Range(0, 100)] [SerializeField] private int groundPercentage;

    //EVENTOS
    public static event Action OnMapGenerationCompleted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += BlockCellByTower;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= BlockCellByTower;
    }

    private void Start()
    {
        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath();
        }

        //ShowPathConnections();
        StartCoroutine(GenerateMap());
    }

    private void Update()
    {
        if (restart) //debug
        {
            RestartMapGeneration();
        }
    }

    private void NewPath()
    {
        gridGraph = new GridGraph(width, height);
        pathGenerator = new PathGenerator();
        pathCells = pathGenerator.GeneratePath(gridGraph);
        pathGenerator.GenerateCrossroads(gridGraph);
        //pathGenerator.GenerateBranches(gridGraph);
        pathSize = pathCells.Count;
    }

    private void RestartMapGeneration()
    {
        restart = false;
        Transform pathTileContainerTransform = pathTileContainer.transform;
        for (int i = 0; i < pathTileContainerTransform.childCount; i++)
        {
            Destroy(pathTileContainerTransform.GetChild(i).gameObject);
        }
        Transform groundTileContainerTransform = groundTileContainer.transform;
        for (int i = 0; i < groundTileContainerTransform.childCount; i++)
        {
            Destroy(groundTileContainerTransform.GetChild(i).gameObject);
        }
        Transform towerHolderTileContainerTransform = towerHolderTileContainer.transform;
        for (int i = 0; i < towerHolderTileContainerTransform.childCount; i++)
        {
            Destroy(towerHolderTileContainerTransform.GetChild(i).gameObject);
        }

        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath();
        }

        //ShowPathConnections();
        StartCoroutine(GenerateMap());
    } //debug

    private void GetTowerCells()
    {
        foreach (Cell pathCell in pathCells)
        {
            //no permitir poner torres alrededor del inicio y el final del camino
            if (pathCell.x == pathGenerator.startPosX && pathCell.y == pathGenerator.startPosY) continue;
            if (pathCell.x == pathGenerator.finalPosX && pathCell.y == pathGenerator.finalPosY) continue;

            IsTowerHolderCell(pathCell.x + 1, pathCell.y); //derecha
            IsTowerHolderCell(pathCell.x - 1, pathCell.y); //izquierda
            IsTowerHolderCell(pathCell.x, pathCell.y + 1); //arriba
            IsTowerHolderCell(pathCell.x, pathCell.y - 1); //abajo
        }
    }

    private void IsTowerHolderCell(int x, int y)
    {
        Cell cell = gridGraph.GetCell(x, y);
        if (cell == null) return; //no existe
        if (pathCells.Contains(cell)) return; //es camino
        if (cell.isObstacle) return; //es obstaculo
        //HashSet no permite duplicados
        towerCells.Add(cell);
    }

    private IEnumerator GenerateMap()
    {
        GenerateObstacles();
        GetTowerCells();
        StartCoroutine(PlaceScenaryTiles());
        StartCoroutine(PlacePathTiles());
        OnMapGenerationCompleted?.Invoke();
        yield return null;
    }

    private void GenerateObstacles()
    {
        Cell cell;
        int roll;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cell = gridGraph.GetCell(x, y);

                if (pathGenerator.IsCellFree(cell))
                {
                    roll = UnityEngine.Random.Range(0, 100);

                    if (roll >= groundPercentage)
                        cell.isObstacle = true;
                    else
                        cell.isObstacle = false;
                }
            }
        }
    }

    private IEnumerator PlaceScenaryTiles()
    {
        Transform groundTileContainerTransform = groundTileContainer.transform;
        Transform towerHolderTileContainerTransform = towerHolderTileContainer.transform;
        GameObject groundTile = levelCellObjects[0].cellPrefab;
        GameObject tile;
        Cell currentCell;
        Vector3 pos;
        int randomIndex;

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                currentCell = gridGraph.GetCell(x, y);

                if (pathGenerator.IsCellFree(gridGraph.GetCell(x, y)))
                {
                    if (currentCell.isObstacle)
                    {
                        randomIndex = Random.Range(2, levelCellObjects.Length);
                        tile = levelCellObjects[randomIndex].cellPrefab;
                    }
                    else if (towerCells.Contains(currentCell))
                    {
                        tile = levelCellObjects[1].cellPrefab;
                    }
                    else
                        tile = groundTile;

                    pos = new Vector3(x, y, 0);
                    tile = Instantiate(tile, pos, Quaternion.identity);

                    if (towerCells.Contains(currentCell)) tile.transform.SetParent(towerHolderTileContainerTransform);
                    else tile.transform.SetParent(groundTileContainerTransform);

                    //yield return new WaitForSeconds(0.005f); //espera
                }
            }
        }

        yield return null;
    }

    //renderizado DFS para el dibujado de tiles del camino
    //HashSet<Cell> --> visitedCells.Contains(currentCell) == O(1)
    private void PlacePathTilesRec(Cell currentCell, Transform pathTileContainer, HashSet<Cell> visitedCells)
    {
        if (visitedCells.Contains(currentCell)) return;

        visitedCells.Add(currentCell);

        int neighboursValue = pathGenerator.GetCellNeighbourValue(currentCell.x, currentCell.y, gridGraph);
        GameObject tilePrefab = pathCellObjects[neighboursValue].cellPrefab;
        GameObject tile = Instantiate(tilePrefab, new Vector3(currentCell.x, currentCell.y, 0f), Quaternion.identity);
        tile.transform.Rotate(0f, 0f, pathCellObjects[neighboursValue].zRotation);
        tile.transform.SetParent(pathTileContainer);

        foreach (Cell nextCell in currentCell.nextCells)
        {
            PlacePathTilesRec(nextCell, pathTileContainer, visitedCells);
        }
    }

    private IEnumerator PlacePathTiles()
    {
        Transform pathTileContainerTransform = pathTileContainer.transform;
        HashSet<Cell> visitedCells = new HashSet<Cell>();

        PlacePathTilesRec(gridGraph.entryPoint, pathTileContainerTransform, visitedCells);

        yield return null;
    }

    private void ShowPathConnectionsRec(Cell currentCell, HashSet<Cell> visitedCells) //debug
    {
        if (visitedCells.Contains(currentCell)) return;

        visitedCells.Add(currentCell); 
        
        string mess = "Act: " + System.Convert.ToString((currentCell.x, currentCell.y)) + " | ";

        foreach (Cell nextCell in currentCell.nextCells)
        {
            mess += "Sig: " + System.Convert.ToString((nextCell.x, nextCell.y));
            Debug.Log(mess);
            ShowPathConnectionsRec(nextCell, visitedCells);
        }
    }

    private void ShowPathConnections()
    {
        HashSet<Cell> visitedCells = new HashSet<Cell>();
        ShowPathConnectionsRec(gridGraph.entryPoint, visitedCells);
    }

    public bool IsBuildableCell(int x, int y)
    {
        Cell cell = gridGraph.GetCell(x, y);

        if (towerCells.Contains(cell) && !cell.isTowerInCell) return true;
        return false;
    }

    public void BlockCellByTower(GameObject tower)
    {
        int x = (int)tower.transform.position.x;
        int y = (int)tower.transform.position.y;
        Cell cell = gridGraph.GetCell(x, y);
        cell.isTowerInCell = true;
        Debug.Log("Celda bloqueada por torre: " + (x, y));
    }

    public void UnblockCell(int x, int y)
    {
        Cell cell = gridGraph.GetCell(x, y);
        cell.isTowerInCell = false;
    }

    public float GetMapHeight()
    {
        return height;
    }

    public float GetMapWidht()
    {
        return width;
    }

    public Cell GetGraphEntryPoint()
    {
        return gridGraph.entryPoint;
    }
}
