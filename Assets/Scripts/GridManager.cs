using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool restart; //debug

    private PathGenerator pathGenerator;
    private List<Vector2Int> pathCells;
    private int pathSize;
    [SerializeField] private GridCellObject[] pathCellObjects;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int minPathLength;

    private void Start()
    {
        pathGenerator = new PathGenerator(height, width);
        pathCells = pathGenerator.GeneratePath();
        pathSize = pathCells.Count;

        while (pathSize < minPathLength)
        {
            pathCells = pathGenerator.GeneratePath();
            pathSize = pathCells.Count;
        }

        StartCoroutine(PlacePathTiles()); //debug
    }

    private void Update()
    {
        if (restart) //debug
        {
            RestartPathGeneration();
        }
    }

    private void RestartPathGeneration()
    {
        restart = false;
        Transform pathTileContainer = this.transform.GetChild(0);

        for (int i = 0; i < pathTileContainer.childCount; i++)
        {
            Destroy(pathTileContainer.GetChild(i).gameObject);
        }

        pathGenerator = new PathGenerator(height, width);
        pathCells = pathGenerator.GeneratePath();
        pathSize = pathCells.Count;

        while (pathSize < minPathLength)
        {
            pathCells = pathGenerator.GeneratePath();
            pathSize = pathCells.Count;
        }

        StartCoroutine(PlacePathTiles()); //debug
    } //debug

    private IEnumerator PlacePathTiles()
    {
        Transform pathTileContainer = this.transform.GetChild(0);

        foreach (Vector2Int cell in pathCells)
        {
            int neighboursValue = pathGenerator.GetCellNeighbourValue(cell.x, cell.y);
            GameObject pathTile = pathCellObjects[neighboursValue].cellPrefab;
            Vector3 pos = new Vector3(cell.x, cell.y, 0f);
            pathTile = Instantiate(pathTile, pos, Quaternion.identity);
            Vector3 rotation = new Vector3(0f, 0f, pathCellObjects[neighboursValue].zRotation);
            pathTile.transform.Rotate(rotation, Space.Self);
            pathTile.transform.SetParent(pathTileContainer);

            yield return new WaitForSeconds(0.02f); //espera
        }

        yield return null;
    }
}
