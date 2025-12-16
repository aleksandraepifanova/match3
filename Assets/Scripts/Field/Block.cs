public class Block
{
    public BlockType Type { get; private set; }
    public bool IsNew;

    public Block(BlockType type)
    {
        Type = type;
        IsNew = true;
    }
}
