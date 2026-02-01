using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool restart; //debug

    private GridGraph gridGraph;
    private PathGenerator pathGenerator;
    private List<Cell> pathCells;
    private int pathSize;

    [SerializeField] private GridCellObject[] pathCellObjects;
    [SerializeField] private GridCellObject[] levelCellObjects;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int minPathLength;
    [SerializeField] private int groundPercentage;

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

        NewPath();

        while (pathSize < minPathLength)
        {
            NewPath() ;
        }

        ShowPathConnections();
        StartCoroutine(GenerateMap());
    } //debug

    private IEnumerator GenerateMap()
    {
        StartCoroutine(PlaceScenaryTiles());
        StartCoroutine(PlacePathTiles());

        yield return null;
    }

    private IEnumerator PlaceScenaryTiles()
    {
        Transform groundTileContainer = this.transform.GetChild(1);
        GameObject groundTile = levelCellObjects[0].cellPrefab;

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (pathGenerator.IsCellFree(gridGraph.GetCell(x, y)))
                {
                    GameObject tile;
                    int roll = Random.Range(0, 100);

                    if (roll < groundPercentage)
                        tile = groundTile;
                    else
                    {
                        int randomIndex = Random.Range(1, levelCellObjects.Length);
                        tile = levelCellObjects[randomIndex].cellPrefab;
                    }

                    Vector3 pos = new Vector3(x, y, 0);
                    tile = Instantiate(tile, pos, Quaternion.identity);
                    tile.transform.SetParent(groundTileContainer);

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
