using UnityEngine;

public class FieldView : MonoBehaviour
{
    [SerializeField] private BlockView blockPrefab;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite orangeSprite;

    private Grid grid;

    public void Init(Grid grid)
    {
        this.grid = grid;
        DrawField();
    }

    private void DrawField()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                var cell = grid.GetCell(x, y);
                if (cell.IsEmpty) continue;

                var blockView = Instantiate(blockPrefab, transform);
                blockView.transform.localPosition = new Vector3(x, y, 0);

                blockView.Init(cell);
                blockView.SetSprite(GetSprite(cell.Block.Type));
            }
        }
    }

    private Sprite GetSprite(BlockType type)
    {
        return type switch
        {
            BlockType.Blue => blueSprite,
            BlockType.Orange => orangeSprite,
            _ => null
        };
    }
}
