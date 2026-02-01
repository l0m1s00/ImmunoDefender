using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class PathGenerator
{
    private int margin = 2;
    private int pathOffset = 2;
    private int startPosition = 1;

    public List<Cell> pathCells;

    public PathGenerator()
    {
        pathCells = new List<Cell>();
    }

    public List<Cell> GeneratePath(ref GridGraph gridGraph)
    {
        Cell currentCell;
        Cell nextCell;
        int y = gridGraph.height / 2; //mitad grid
        int x = startPosition;
        int finalPosY = gridGraph.height / 2;
        int finalPosX = gridGraph.width - margin;

        while (x < finalPosX - 1)
        {
            currentCell = gridGraph.GetCell(x, y); //celda actual
            currentCell.x = x;
            currentCell.y = y;
            currentCell.type = GridCellObject.CellType.Path; 
            pathCells.Add(currentCell);
            bool validMove = false;

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (x == startPosition || move == 0 || x % pathOffset == 0 || x >= finalPosX - 1) //derecha
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && IsCellFree(gridGraph.GetCell(x, y + 1)) && y < (gridGraph.height - margin)) //arriba
                {
                    y++;
                    validMove = true;
                }
                else if (move == 2 && IsCellFree(gridGraph.GetCell(x, y - 1)) && y > margin-1) //abajo
                {
                    y--;
                    validMove = true;
                }
            }

            nextCell = gridGraph.GetCell(x, y); //celda siguiente
            gridGraph.ConnectCells(currentCell, nextCell); //conexion celda anterior - siguiente
        }

        while (y != finalPosY)
        {
            currentCell = gridGraph.GetCell(x, y);
            currentCell.x = x;
            currentCell.y = y;
            currentCell.type = GridCellObject.CellType.Path;
            pathCells.Add(currentCell);

            if (y < finalPosY) y++;
            else y--;

            nextCell = gridGraph.GetCell(x, y);
            gridGraph.ConnectCells(currentCell, nextCell);
        }

        currentCell = gridGraph.GetCell(x, y);
        currentCell.x = x;
        currentCell.y = y;
        currentCell.type = GridCellObject.CellType.Path;
        pathCells.Add(currentCell);
        nextCell = gridGraph.GetCell(finalPosX, finalPosY);
        gridGraph.ConnectCells(currentCell, nextCell);
        currentCell = gridGraph.GetCell(finalPosX, finalPosY);
        currentCell.x = finalPosX;
        currentCell.y = finalPosY;
        currentCell.type = GridCellObject.CellType.Path;
        pathCells.Add(currentCell);

        return pathCells;
    }

    //public bool GenerateCrossroads(ref GridGraph gridGraph)
    //{
    //    Cell nexusCell;
    //    int width = gridGraph.width;
    //    int height = gridGraph.height;

    //    for (int i = 0; i < pathCells.Count; i++)
    //    {
    //        nexusCell = pathCells[i];
            
    //        if (nexusCell.x > startPosition && nexusCell.x < width - margin && nexusCell.y > 2 && nexusCell.y < height - margin-1)
    //        {
    //            if (
    //                IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 3)) &&
    //                IsCellFree(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y + 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y + 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y + 3)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y - 1)) &&
    //                IsCellFree(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 3)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y - 1)) &&
    //                IsCellFree(gridGraph.GetCell(nexusCell.x + 3, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 3, nexusCell.y + 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 3, nexusCell.y + 2))
    //                )
    //            {

    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 1));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y + 2));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y + 1));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 2));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y + 2));

    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 1, nexusCell.y));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 1));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y + 1));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x + 2, nexusCell.y + 2));
    //                pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y + 2));


    //                return true;
    //            }

    //            //if (
    //            //    IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y - 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y - 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y - 3)) &&
    //            //    IsCellFree(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y - 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y - 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y - 3)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y + 1)) &&
    //            //    IsCellFree(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y - 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y - 2)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y - 3)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y + 1)) &&
    //            //    IsCellFree(gridGraph.GetCell(nexusCell.x - 3, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 3, nexusCell.y - 1)) && IsCellFree(gridGraph.GetCell(nexusCell.x - 3, nexusCell.y - 2))
    //            //    )
    //            //{

    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y - 1));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x - 1, nexusCell.y - 2));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y - 1));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x - 2, nexusCell.y - 2));
    //            //    pathCells.Add(gridGraph.GetCell(nexusCell.x, nexusCell.y - 2));

    //            //    return true;
    //            //}
    //        }
    //    }

    //    return false;
    //}

    public bool IsCellFree(Cell cell)
    {
        return !pathCells.Contains(cell);
    }

    private bool IsCellTaken(Cell cell)
    {
        return pathCells.Contains(cell);
    }

    public int GetCellNeighbourValue(int x, int y, ref GridGraph gridGraph)
    {
        int value = 0;

        if (IsCellTaken(gridGraph.GetCell(x, y - 1))) //abajo
        {
            value++;
        }
        if (IsCellTaken(gridGraph.GetCell(x - 1, y))) //izquierda
        {
            value += 2;
        }
        if (IsCellTaken(gridGraph.GetCell(x + 1, y))) //derecha
        {
            value += 4;
        }
        if (IsCellTaken(gridGraph.GetCell(x, y + 1))) //arriba
        {
            value += 8;
        }

        return value;
    } //sumatorios para determinar la path tile que hay que usar

    public void ShowPathCells()
    {
        for (int i = 0; i < pathCells.Count; i++)
        {
            Debug.Log((pathCells[i].x, pathCells[i].y));
        }
    }
}
