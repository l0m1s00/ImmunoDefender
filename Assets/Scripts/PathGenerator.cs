using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PathGenerator
{
    private int margin = 2;
    private int pathOffset = 2;

    public List<Cell> pathCells; //historial de generacion del camino
    public int finalPosX;
    public int finalPosY;
    public int startPosX;
    public int startPosY;

    public PathGenerator()
    {
        pathCells = new List<Cell>();
    }

    public void GenerateBranches(GridGraph gridGraph)
    {
        Cell currentCell;
        int generateBranchRoll;

        for (int i = 0; i < pathCells.Count; i++)
        {
            currentCell = pathCells[i];

            if (currentCell.x == startPosX && currentCell.y == startPosY) continue;
            if (currentCell.x == finalPosX && currentCell.y == finalPosY) continue;

            generateBranchRoll = Random.Range(0, 4); //25%
            if (generateBranchRoll == 1)
            {
                TryGenerateBranch(gridGraph, currentCell);
            }
        }
    }

    private bool TryGenerateBranch(GridGraph gridGraph, Cell startCell)
    {
        List<Cell> freeNeighbours = GetPossibleInitialBranchPoints(startCell, gridGraph);

        if (freeNeighbours.Count == 0) return false; //no hay espacio

        int roll = Random.Range(0, freeNeighbours.Count);
        Cell next = freeNeighbours[roll];

        gridGraph.ConnectCells(startCell, next);
        pathCells.Add(next);

        return ExpandBranch(next, gridGraph);
    }

    private bool ExpandBranch(Cell currentCell, GridGraph gridGraph)
    {
        return true;
    }

    private List<Cell> GetPossibleInitialBranchPoints(Cell cell, GridGraph gridGraph)
    {
        List<Cell> possibleInitialBranchCells = new List<Cell>();
        int neighBoursValue = GetCellNeighbourValue(cell.x, cell.y, gridGraph);

        if (neighBoursValue == 6) //horizontal
        {

        }
        else if (neighBoursValue == 9) //vertical
        {

        }
        else if (neighBoursValue == 10) //esquina abajo derecha
        {

        }
        else if (neighBoursValue == 5) //esquina arriba izquierda
        {

        }
        else if (neighBoursValue == 3) //esquina arriba derecha
        {

        }
        else if (neighBoursValue == 12) //esquina abajo izquierda
        {

        }

        return possibleInitialBranchCells;
    }

    public List<Cell> GeneratePath(GridGraph gridGraph)
    {
        startPosX = 1;
        startPosY = gridGraph.height / 2;

        Cell currentCell;
        Cell nextCell;
        int y = startPosY; //mitad grid
        int x = startPosX;
        finalPosY = gridGraph.height / 2;
        finalPosX = gridGraph.width - margin;

        while (x < finalPosX - 1)
        {
            currentCell = gridGraph.GetCell(x, y); //celda actual
            currentCell.type = GridCellObject.CellType.Path; 
            pathCells.Add(currentCell);
            bool validMove = false;

            if (gridGraph.entryPoint == null) gridGraph.entryPoint = currentCell; //punto de acceso al grafo

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (x == startPosX || move == 0 || x % pathOffset == 0 || x >= finalPosX - 1) //derecha
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
            currentCell.type = GridCellObject.CellType.Path;
            pathCells.Add(currentCell);

            if (y < finalPosY) y++;
            else y--;

            nextCell = gridGraph.GetCell(x, y);
            gridGraph.ConnectCells(currentCell, nextCell);
        }

        currentCell = gridGraph.GetCell(x, y);
        currentCell.type = GridCellObject.CellType.Path;
        pathCells.Add(currentCell);
        nextCell = gridGraph.GetCell(finalPosX, finalPosY);
        gridGraph.ConnectCells(currentCell, nextCell);
        currentCell = gridGraph.GetCell(finalPosX, finalPosY);
        currentCell.type = GridCellObject.CellType.Path;
        pathCells.Add(currentCell);

        return pathCells;
    }

    public void GenerateCrossroads(GridGraph gridGraph)
    {
        Cell currentCell;
        int width = gridGraph.width;
        int height = gridGraph.height;

        //int roll = Random.Range(0, 2); //no siempre se hace una rotonda, 50%

        for (int i = 0; i < pathCells.Count; i++)
        {
            currentCell = pathCells[i];

            if (CheckAreaClearLoop(gridGraph, currentCell, 1, 1) && Random.Range(0, 2) == 1) //x == 1, y == 1 --> rotonda arriba derecha
            {
                Debug.Log("Nexo arriba derecha encontrado: " + (currentCell.x, currentCell.y));
                CreateLoop(gridGraph, currentCell, 1, 1);
            }
            if (CheckAreaClearLoop(gridGraph, currentCell, -1, -1) && Random.Range(0, 2) == 1) //x == -1, y == -1 --> rotonda abajo izquierda
            {
                Debug.Log("Nexo abajo izquierda encontrado: " + (currentCell.x, currentCell.y));
                CreateLoop(gridGraph, currentCell, -1, -1);
            }
            if (CheckAreaClearLoop(gridGraph, currentCell, 1, -1) && Random.Range(0, 2) == 1) //x == 1, y == -1 --> rotonda abajo derecha
            {
                Debug.Log("Nexo abajo derecha encontrado: " + (currentCell.x, currentCell.y));
                CreateLoop(gridGraph, currentCell, 1, -1);
            }
            if (CheckAreaClearLoop(gridGraph, currentCell, -1, 1) && Random.Range(0, 2) == 1) //x == -1, y == 1 --> rotonda arriba izquierda
            {
                Debug.Log("Nexo arriba izquierda encontrado: " + (currentCell.x, currentCell.y));
                CreateLoop(gridGraph, currentCell, -1, 1);
            }
        }
    }

    private void CreateLoop(GridGraph gridGraph, Cell nexusCell, int directionX, int directionY)
    {
        Cell currentCell = nexusCell;
        Cell nextCell;
        int newX;
        int newY;

        newX = nexusCell.x + 1 * directionX;
        newY = nexusCell.y;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x + 2 * directionX;
        newY = nexusCell.y;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x + 2 * directionX;
        newY = nexusCell.y + 1 * directionY;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x + 2 * directionX;
        newY = nexusCell.y + 2 * directionY;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x + 1 * directionX;
        newY = nexusCell.y + 2 * directionY;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x;
        newY = nexusCell.y + 2 * directionY;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        newX = nexusCell.x;
        newY = nexusCell.y + 1 * directionY;
        nextCell = gridGraph.GetCell(newX, newY);
        gridGraph.ConnectCells(currentCell, nextCell);
        pathCells.Add(nextCell);
        currentCell = nextCell;

        gridGraph.ConnectCells(currentCell, nexusCell);
    }

    private bool CheckAreaClearLoop(GridGraph gridGraph, Cell nexusCell, int directionX, int directionY)
    {
        if (nexusCell.x > startPosX && nexusCell.x < gridGraph.width - margin && nexusCell.y > 2 && nexusCell.y < gridGraph.height - margin - 1)
        {
            if (
                IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 1 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 2 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x, nexusCell.y + 3 * directionY)) &&
                IsCellFree(gridGraph.GetCell(nexusCell.x + 1 * directionX, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1 * directionX, nexusCell.y + 1 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1 * directionX, nexusCell.y + 2 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1 * directionX, nexusCell.y + 3 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 1 * directionX, nexusCell.y - 1 * directionY)) &&
                IsCellFree(gridGraph.GetCell(nexusCell.x + 2 * directionX, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2 * directionX, nexusCell.y + 1 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2 * directionX, nexusCell.y + 2 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2 * directionX, nexusCell.y + 3 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 2 * directionX, nexusCell.y - 1 * directionY)) &&
                IsCellFree(gridGraph.GetCell(nexusCell.x + 3 * directionX, nexusCell.y)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 3 * directionX, nexusCell.y + 1 * directionY)) && IsCellFree(gridGraph.GetCell(nexusCell.x + 3 * directionX, nexusCell.y + 2 * directionY))
                )
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCellFree(Cell cell)
    {
        return !pathCells.Contains(cell);
    }

    private bool IsCellTaken(Cell cell)
    {
        return pathCells.Contains(cell);
    }

    public int GetCellNeighbourValue(int x, int y, GridGraph gridGraph)
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
