using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Cell
{
    public int x;
    public int y;

    public GridCellDataSO.CellType type;
    public bool isObstacle;

    public List<Cell> nextCells; //arcos de salida
    //public List<Cell> prevCells; //arcos de entrada

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.type = GridCellDataSO.CellType.Ground; //por defecto
        isObstacle = false;
        nextCells = new List<Cell>();
        //prevCells = new List<Cell>();
    }
}
