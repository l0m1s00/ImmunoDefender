using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool restart; //debug

    private GridGraph gridGraph;
    private PathGenerator pathGenerator;
    private List<Cell> pathCells;
    private HashSet<Cell> towerCells;
    private int pathSize;

    [SerializeField] private GridCellObject[] pathCellObjects;
    [SerializeField] private GridCellObject[] levelCellObjects;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int minPathLength;
    [Range(0, 100)] [SerializeField] private int groundPercentage;

    private void Awake()
    {
        towerCells = new HashSet<Cell>();
    }

    private void Start()
    {
        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath();
        }

        ShowPathConnections();
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
        pathGenerator.GenerateBranches(gridGraph);
        pathSize = pathCells.Count;
    }

    private void RestartMapGeneration()
    {
        restart = false;
        Transform pathTileContainer = this.transform.GetChild(0);
        for (int i = 0; i < pathTileContainer.childCount; i++)
        {
            Destroy(pathTileContainer.GetChild(i).gameObject);
        }
        Transform groundTileContainer = this.transform.GetChild(1);
        for (int i = 0; i < groundTileContainer.childCount; i++)
        {
            Destroy(groundTileContainer.GetChild(i).gameObject);
        }
        Transform towerHolderTileContainer = this.transform.GetChild(2);
        for (int i = 0; i < towerHolderTileContainer.childCount; i++)
        {
            Destroy(towerHolderTileContainer.GetChild(i).gameObject);
        }

        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath() ;
        }

        ShowPathConnections();
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

        foreach (Cell cell in towerCells)
        {
            Debug.Log((cell.x, cell.y));
        }

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
                    roll = Random.Range(0, 100);

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
        Transform groundTileContainer = this.transform.GetChild(1);
        Transform towerHolderTileContainer = this.transform.GetChild(2);
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

                    if (currentCell.isObstacle) tile.transform.SetParent(groundTileContainer);
                    else if (towerCells.Contains(currentCell)) tile.transform.SetParent(towerHolderTileContainer);
                    else tile.transform.SetParent(groundTileContainer);

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
        Transform pathTileContainer = this.transform.GetChild(0);
        HashSet<Cell> visitedCells = new HashSet<Cell>();

        PlacePathTilesRec(gridGraph.entryPoint, pathTileContainer, visitedCells);

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
}
