using System.Collections.Generic;

public class MatchFinder
{
    private Grid grid;

    public MatchFinder(Grid grid)
    {
        this.grid = grid;
    }

    public List<List<Cell>> FindMatches()
    {
        var result = new List<List<Cell>>();
        bool[,] visited = new bool[grid.Width, grid.Height];

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if (visited[x, y])
                    continue;

                Cell cell = grid.GetCell(x, y);
                if (cell.IsEmpty)
                    continue;

                List<Cell> area = CollectArea(cell, visited);
                if (area.Count >= 3 && AreaHasNewBlock(area))
                {
                    result.Add(area);
                }
            }
        }

        return result;
    }

    private bool AreaHasNewBlock(List<Cell> area)
    {
        foreach (var cell in area)
        {
            if (cell.Block != null && cell.Block.IsNew)
                return true;
        }
        return false;
    }

    private List<Cell> CollectArea(Cell start, bool[,] visited)
    {
        var area = new List<Cell>();
        var stack = new Stack<Cell>();

        stack.Push(start);
        visited[start.Position.x, start.Position.y] = true;

        BlockType type = start.Block.Type;

        while (stack.Count > 0)
        {
            Cell current = stack.Pop();
            area.Add(current);

            TryAddNeighbour(current.Position.x + 1, current.Position.y);
            TryAddNeighbour(current.Position.x - 1, current.Position.y);
            TryAddNeighbour(current.Position.x, current.Position.y + 1);
            TryAddNeighbour(current.Position.x, current.Position.y - 1);
        }

        return area;

        void TryAddNeighbour(int x, int y)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height)
                return;

            if (visited[x, y])
                return;

            Cell neighbour = grid.GetCell(x, y);
            if (neighbour.IsEmpty)
                return;

            if (neighbour.Block.Type != type)
                return;

            visited[x, y] = true;
            stack.Push(neighbour);
        }
    }
}
