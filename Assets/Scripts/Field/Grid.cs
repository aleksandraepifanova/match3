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
}
