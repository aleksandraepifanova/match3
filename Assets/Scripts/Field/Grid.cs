using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int Width { get; }
    public int Height { get; }

    private Cell[,] cells;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;

        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(x, y);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }

    public bool AreNeighbours(Cell a, Cell b)
    {
        int dx = Mathf.Abs(a.Position.x - b.Position.x);
        int dy = Mathf.Abs(a.Position.y - b.Position.y);

        return (dx + dy) == 1;
    }

    public void SwapBlocks(Cell a, Cell b)
    {
        Block temp = a.Block;
        a.Block = b.Block;
        b.Block = temp;
        if (a.Block != null)
            a.Block.IsNew = true;

        if (b.Block != null)
            b.Block.IsNew = true;
    }

    public void RemoveBlocks(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            cell.Block = null;
        }
    }

    public bool CanFall(Cell cell)
    {
        if (cell.IsEmpty)
            return false;

        int x = cell.Position.x;
        int y = cell.Position.y;

        if (y == 0)
            return false;

        Cell below = GetCell(x, y - 1);
        return below.IsEmpty;
    }

    public Cell FallOne(Cell cell)
    {
        Cell below = GetCell(cell.Position.x, cell.Position.y - 1);

        below.Block = cell.Block;
        cell.Block = null;

        if (below.Block != null)
            below.Block.IsNew = true;

        return below;
    }

    public bool HasFallingBlocks()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 1; y < Height; y++)
            {
                Cell cell = GetCell(x, y);
                if (CanFall(cell))
                    return true;
            }
        }
        return false;
    }

    public bool HasAnyBlocks()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (!GetCell(x, y).IsEmpty)
                    return true;

        return false;
    }
}
