public static class LevelFactory
{
    public static Grid CreateGrid(LevelData data)
    {
        var grid = new Grid(data.width, data.height);

        for (int row = 0; row < data.rows.Length; row++)
        {
            string line = data.rows[row];
            int y = data.height - 1 - row;

            for (int x = 0; x < data.width; x++)
            {
                char c = line[x];
                if (c == 'B')
                {
                    var block = new Block(BlockType.Blue);
                    block.IsNew = false;
                    grid.GetCell(x, y).Block = block;
                }
                else if (c == 'O')
                {
                    var block = new Block(BlockType.Orange);
                    block.IsNew = false;
                    grid.GetCell(x, y).Block = block;
                }
            }
        }

        return grid;
    }
}
