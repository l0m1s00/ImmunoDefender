using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PathGenerator
{
    private int height;
    private int width;
    private int margin = 1;
    private int pathOffset = 2;
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
        int x = 0;
        int finalPosY = height / 2;
        int finalPosX = width - margin;

        while (x < finalPosX - 1)
        {
            pathCells.Add(new Vector2Int(x, y));
            bool validMove = false;

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (move == 0 || x % pathOffset == 0 || x >= finalPosX - 1) //derecha
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && IsCellFree(x, y + 1) && y < (height - margin)) //arriba
                {
                    y++;
                    validMove = true;
                }
                else if (move == 2 && IsCellFree(x, y - 1) && y > margin) //abajo
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

    private bool IsCellFree(int x, int y)
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
    }
}
