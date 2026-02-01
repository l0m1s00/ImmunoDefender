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
        //if (restart) //debug
        //{
        //    RestartMapGeneration();
        //}
    }

    private void NewPath()
    {
        gridGraph = new GridGraph(width, height);
        pathGenerator = new PathGenerator();
        pathCells = pathGenerator.GeneratePath(ref gridGraph);
        pathSize = pathCells.Count;
    }

    //private void RestartMapGeneration()
    //{
    //    restart = false;
    //    Transform pathTileContainer = this.transform.GetChild(0);
    //    for (int i = 0; i < pathTileContainer.childCount; i++)
    //    {
    //        Destroy(pathTileContainer.GetChild(i).gameObject);
    //    }
    //    Transform groundTileContainer = this.transform.GetChild(1);
    //    for (int i = 0; i < groundTileContainer.childCount; i++)
    //    {
    //        Destroy(groundTileContainer.GetChild(i).gameObject);
    //    }

    //    pathGenerator = new PathGenerator(height, width);
    //    pathCells = pathGenerator.GeneratePath();
    //    while (pathGenerator.GenerateCrossroad()) ;
    //    pathSize = pathCells.Count;

    //    while (pathSize < minPathLength)
    //    {
    //        pathCells = pathGenerator.GeneratePath();
    //        pathSize = pathCells.Count;
    //    }

    //    StartCoroutine(GenerateMap());
    //} //debug

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

                    yield return new WaitForSeconds(0.01f); //espera
                }
            }
        }

        yield return null;
    }

    private IEnumerator PlacePathTiles()
    {
        Transform pathTileContainer = this.transform.GetChild(0);

        foreach (Cell cell in pathCells)
        {
            int neighboursValue = pathGenerator.GetCellNeighbourValue(cell.x, cell.y, ref gridGraph);
            GameObject pathTile = pathCellObjects[neighboursValue].cellPrefab;
            Vector3 pos = new Vector3(cell.x, cell.y, 0f);
            pathTile = Instantiate(pathTile, pos, Quaternion.identity);
            Vector3 rotation = new Vector3(0f, 0f, pathCellObjects[neighboursValue].zRotation);
            pathTile.transform.Rotate(rotation, Space.Self);
            pathTile.transform.SetParent(pathTileContainer);

            yield return new WaitForSeconds(0.05f); //espera
        }

        yield return null;
    }

    private void ShowPathConnections() //debug
    {
        for (int i = 0; i < pathCells.Count-1; i++)
        {
            Debug.Log("Act: " + (pathCells[i].x, pathCells[i].y) + " | " + "Sig: " + (pathCells[i + 1].x, pathCells[i + 1].y));
        }

        Debug.Log("Ultima celda: " + (pathCells[pathCells.Count - 1].x, pathCells[pathCells.Count - 1].y) + " | Conex: " + pathCells[pathCells.Count - 1].nextCells.Count);
    }
}
