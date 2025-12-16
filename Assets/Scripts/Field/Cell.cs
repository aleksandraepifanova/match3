using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public Block Block { get; set; }

    public Cell(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public bool IsEmpty => Block == null;
}
