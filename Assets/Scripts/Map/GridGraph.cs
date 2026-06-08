using UnityEngine;

public class GridGraph
{
    public int width;
    public int height;
    public Cell entryPoint = null;

    private Cell[,] gridGraph;

    public GridGraph(int width, int height)
    {
        this.width = width;
        this.height = height;
        gridGraph = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridGraph[x, y] = new Cell(x, y);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridGraph[x, y];

        //Debug.LogError("Intentando acceder a una celda fuera de los límites del grafo. Celda: " + (x, y));
        return null;
    }

    public void ConnectCells(Cell currentCell, Cell nextCell)
    {
        if (!currentCell.nextCells.Contains(nextCell))
        {
            currentCell.nextCells.Add(nextCell);
            currentCell.nextCells.Reverse();
            //end.previousCells.Add(ini);

            //ini.type = GridCellObject.CellType.Path;
            //end.type = GridCellObject.CellType.Path;
        }
        else
            Debug.LogError("Intentando conectar una celda ya enlazada.");
    }
}
