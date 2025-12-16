using UnityEngine;

public class BlockView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Cell Cell { get; private set; }

    public void Init(Cell cell)
    {
        Cell = cell;
    }

    public System.Action<BlockView> OnClicked;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void UpdateCell(Cell newCell)
    {
        Cell = newCell;
    }

    private void OnMouseDown()
    {
        OnClicked?.Invoke(this);
    }
}
