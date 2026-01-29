using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PathGenerator
{
    private int height;
    private int width;
    private int margin = 2;
    private int pathOffset = 2;
    private int startPosition = 1;

    private List<Vector2Int> pathCells;

    public PathGenerator(int height, int width)
    {
        this.height = height;
        this.width = width;
    }

    public List<Vector2Int> GeneratePath()
    {
        pathCells = new List<Vector2Int>();
        int y = height / 2; //mitad grid
        int x = startPosition;
        int finalPosY = height / 2;
        int finalPosX = width - margin;

        while (x < finalPosX - 1)
        {
            pathCells.Add(new Vector2Int(x, y));
            bool validMove = false;

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (x == startPosition || move == 0 || x % pathOffset == 0 || x >= finalPosX - 1) //derecha
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && IsCellFree(x, y + 1) && y < (height - margin)) //arriba
                {
                    y++;
                    validMove = true;
                }
                else if (move == 2 && IsCellFree(x, y - 1) && y > margin-1) //abajo
                {
                    y--;
                    validMove = true;
                }
            }
        }

        while (y != finalPosY)
        {
            pathCells.Add(new Vector2Int(x, y));

            if (y < finalPosY) y++;
            else y--;
        }

        pathCells.Add(new Vector2Int(x, y));
        pathCells.Add(new Vector2Int(finalPosX, finalPosY));

        return pathCells;
    }

    public bool GenerateCrossroad()
    {
        Vector2Int cell;

        for (int i = 0; i < pathCells.Count; i++)
        {
            cell = pathCells[i];

            if (cell.x > startPosition && cell.x < width - margin && cell.y > 2 && cell.y < height - margin-1)
            {
                if (
                    IsCellFree(cell.x, cell.y + 1) && IsCellFree(cell.x, cell.y + 2) && IsCellFree(cell.x, cell.y + 3) &&
                    IsCellFree(cell.x + 1, cell.y) && IsCellFree(cell.x + 1, cell.y + 1) && IsCellFree(cell.x + 1, cell.y + 2) && IsCellFree(cell.x + 1, cell.y + 3) && IsCellFree(cell.x + 1, cell.y - 1) &&
                    IsCellFree(cell.x + 2, cell.y) && IsCellFree(cell.x + 2, cell.y + 1) && IsCellFree(cell.x + 2, cell.y + 2) && IsCellFree(cell.x + 2, cell.y + 3) && IsCellFree(cell.x + 2, cell.y - 1) &&
                    IsCellFree(cell.x + 3, cell.y) && IsCellFree(cell.x + 3, cell.y + 1) && IsCellFree(cell.x + 3, cell.y + 2)
                    )
                {
                    pathCells.InsertRange(i + 1, new List<Vector2Int> { 
                        new Vector2Int(cell.x + 1, cell.y), new Vector2Int(cell.x + 2, cell.y),
                        new Vector2Int(cell.x + 2, cell.y + 1), new Vector2Int(cell.x + 2, cell.y + 2),
                        new Vector2Int(cell.x + 1, cell.y + 2), new Vector2Int(cell.x, cell.y + 2),
                        new Vector2Int(cell.x, cell.y + 1)
                    });

                    return true;
                }

                if (
                    IsCellFree(cell.x, cell.y - 1) && IsCellFree(cell.x, cell.y - 2) && IsCellFree(cell.x, cell.y - 3) &&
                    IsCellFree(cell.x - 1, cell.y) && IsCellFree(cell.x - 1, cell.y - 1) && IsCellFree(cell.x - 1, cell.y - 2) && IsCellFree(cell.x - 1, cell.y - 3) && IsCellFree(cell.x - 1, cell.y + 1) &&
                    IsCellFree(cell.x - 2, cell.y) && IsCellFree(cell.x - 2, cell.y - 1) && IsCellFree(cell.x - 2, cell.y - 2) && IsCellFree(cell.x - 2, cell.y - 3) && IsCellFree(cell.x - 2, cell.y + 1) &&
                    IsCellFree(cell.x - 3, cell.y) && IsCellFree(cell.x - 3, cell.y - 1) && IsCellFree(cell.x - 3, cell.y - 2)
                    )
                {
                    pathCells.InsertRange(i + 1, new List<Vector2Int> {
                        new Vector2Int(cell.x - 1, cell.y), new Vector2Int(cell.x - 2, cell.y),
                        new Vector2Int(cell.x - 2, cell.y - 1), new Vector2Int(cell.x - 2, cell.y - 2),
                        new Vector2Int(cell.x - 1, cell.y - 2), new Vector2Int(cell.x, cell.y - 2),
                        new Vector2Int(cell.x, cell.y - 1)
                    });

                    return true;
                }
            }
        }

        return false;
    }

    public bool IsCellFree(int x, int y)
    {
        return !pathCells.Contains(new Vector2Int(x, y));
    }

    private bool IsCellTaken(int x, int y)
    {
        return pathCells.Contains(new Vector2Int(x, y));
    }

    public int GetCellNeighbourValue(int x, int y)
    {
        int value = 0;

        if (IsCellTaken(x, y - 1)) //abajo
        {
            value++;
        }
        if (IsCellTaken(x - 1, y)) //izquierda
        {
            value += 2;
        }
        if (IsCellTaken(x + 1, y)) //derecha
        {
            value += 4;
        }
        if (IsCellTaken(x, y + 1)) //arriba
        {
            value += 8;
        }

        return value;
    } //sumatorios para determinar la path tile que hay que usar
}
