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
        transform.localPosition = new Vector3(newCell.Position.x, newCell.Position.y, 0);
    }

    private void OnMouseDown()
    {
        OnClicked?.Invoke(this);
    }
}
