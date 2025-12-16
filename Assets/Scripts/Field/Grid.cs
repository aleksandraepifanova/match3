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
    }
}
