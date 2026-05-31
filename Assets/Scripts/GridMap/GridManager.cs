using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private GridGraph gridGraph;
    private PathGenerator pathGenerator;
    private List<Cell> pathCells;
    private HashSet<Cell> towerCells = new HashSet<Cell>();
    private HashSet<GameObject> towerHolderCells = new HashSet<GameObject>();
    private int pathSize;

    [Header("Cell Containers")]
    [SerializeField] private GameObject towerHolderTileContainer;
    [SerializeField] private GameObject pathTileContainer;
    [SerializeField] private GameObject groundTileContainer;
    [Header("Cell Objects")]
    [SerializeField] private GridCellDataSO[] pathCellObjectsData;
    [SerializeField] private GridCellDataSO[] levelCellObjectsData;
    [SerializeField] private GridCellDataSO[] towerHolderCellObjectsData;
    //Map Settings
    private int height;
    private int width;
    private int minPathLength;
    private int groundPercentage;

    //EVENTOS
    public static event Action OnMapGenerationCompleted;
    public static event Action OnCellUnblocked;
    public static event Action<int, int> OnMapGenerationRequiredSize;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += BlockCellByTower;
        StartGameButtons.OnResetGameMap += RestartMapGeneration;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= BlockCellByTower;
        StartGameButtons.OnResetGameMap -= RestartMapGeneration;
    }

    private void Start()
    {
        InitializeGrid();

        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath();
        }

        //ShowPathConnections();
        StartCoroutine(GenerateMap());
    }

    private void InitializeGrid()
    {
        DifficultySettingsData difficultyData = DataLibrary.Instance.GetSelectedDifficultyData();
        width = difficultyData.width;
        height = difficultyData.height;
        groundPercentage = difficultyData.groundPercentage;
        minPathLength = difficultyData.minPathSize;
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

        towerCells.Clear();
        towerHolderCells.Clear();
        pathCells.Clear();

        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath();
        }

        //ShowPathConnections();
        StartCoroutine(GenerateMap());
    } 

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
        yield return StartCoroutine(PlaceScenaryTiles());
        yield return StartCoroutine(PlacePathTiles());
        yield return StartCoroutine(PlaceTowerHoldersTiles());

        OnMapGenerationCompleted?.Invoke();
        OnMapGenerationRequiredSize?.Invoke(width, height);
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
        GameObject groundTile = levelCellObjectsData[0].cellPrefab;
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
                        randomIndex = Random.Range(1, levelCellObjectsData.Length);
                        tile = levelCellObjectsData[randomIndex].cellPrefab;
                    }
                    else if (towerCells.Contains(currentCell))
                    {
                        continue;
                    }
                    else
                        tile = groundTile;

                    pos = new Vector3(x, y, 0);
                    tile = Instantiate(tile, pos, Quaternion.identity);

                    //if (towerCells.Contains(currentCell)) tile.transform.SetParent(towerHolderTileContainerTransform);
                    //else tile.transform.SetParent(groundTileContainerTransform);

                    tile.transform.SetParent(groundTileContainerTransform);

                    //yield return new WaitForSeconds(0.005f); //espera
                }
            }
        }

        yield return null;
    }

    private IEnumerator PlaceTowerHoldersTiles()
    {
        foreach (Cell currentTowerHolderCell in towerCells)
        {
            int neighboursValue = pathGenerator.GetCellNeighbourValue(currentTowerHolderCell.x, currentTowerHolderCell.y, gridGraph);
            GameObject tilePrefab = towerHolderCellObjectsData[neighboursValue].cellPrefab;
            GameObject tile = Instantiate(tilePrefab, new Vector3(currentTowerHolderCell.x, currentTowerHolderCell.y, 0f), Quaternion.identity);
            towerHolderCells.Add(tile);
            tile.transform.Rotate(0f, 0f, towerHolderCellObjectsData[neighboursValue].zRotation);
            tile.transform.SetParent(towerHolderTileContainer.transform);
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
        GameObject tilePrefab = pathCellObjectsData[neighboursValue].cellPrefab;
        GameObject tile = Instantiate(tilePrefab, new Vector3(currentCell.x, currentCell.y, 0f), Quaternion.identity);
        tile.transform.Rotate(0f, 0f, pathCellObjectsData[neighboursValue].zRotation);
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

    public void BlockCellByTower(Tower tower)
    {
        int x = (int)tower.gameObject.transform.position.x;
        int y = (int)tower.gameObject.transform.position.y;
        Cell cell = gridGraph.GetCell(x, y);
        cell.isTowerInCell = true;
        //Debug.Log("Celda bloqueada por torre: " + (x, y));
    }

    public void UnblockCell(int x, int y)
    {
        Cell cell = gridGraph.GetCell(x, y);
        cell.isTowerInCell = false;
        OnCellUnblocked?.Invoke();
    }

    public float GetMapHeight()
    {
        return height;
    }

    public float GetMapWidht()
    {
        return width;
    }

    public HashSet<GameObject> GetTowerHolderCellsObjects()
    {
        return towerHolderCells;
    }

    public Cell GetGraphEntryPoint()
    {
        return gridGraph.entryPoint;
    }

    public GridGraph GetCompleteGraph()
    {
        return gridGraph;
    }
}
