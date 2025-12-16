using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [SerializeField] private BlockView blockPrefab;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private float cellSize = 2f;

    public Action<BlockView> OnBlockClicked;

    private Dictionary<Cell, BlockView> views = new Dictionary<Cell, BlockView>();

    private Grid grid;

    public void Init(Grid grid)
    {
        this.grid = grid;


        CalculateCellSize();
        CenterField();
        DrawField();
    }

    private void CalculateCellSize()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        float cellSizeX = screenWidth / grid.Width;
        float cellSizeY = screenHeight / grid.Height;

        cellSize = Mathf.Min(cellSizeX, cellSizeY);
    }

    private void CenterField()
    {
        float offsetX = (grid.Width - 1) * cellSize * 0.5f;
        float offsetY = (grid.Height - 1) * cellSize * 0.5f;

        transform.localPosition = new Vector3(-offsetX, -offsetY, 0);
    }

    private void DrawField()
    {
        views.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                var cell = grid.GetCell(x, y);
                if (cell.IsEmpty) continue;

                var blockView = Instantiate(blockPrefab, transform);
                blockView.transform.localPosition =
                                            new Vector3(
                                                cell.Position.x * cellSize,
                                                cell.Position.y * cellSize,
                                                0
                                            );

                blockView.Init(cell);
                blockView.SetSprite(GetSprite(cell.Block.Type));

                views[cell] = blockView;

                blockView.OnClicked += HandleBlockClicked;
            }
        }
    }

    private void HandleBlockClicked(BlockView blockView)
    {
        OnBlockClicked?.Invoke(blockView);
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
    public void UpdateBlockPosition(BlockView blockView)
    {
        Cell cell = blockView.Cell;

        blockView.transform.localPosition = new Vector3(
            cell.Position.x * cellSize,
            cell.Position.y * cellSize,
            0
        );
    }

    public void RemoveBlockView(BlockView blockView)
    {
        Destroy(blockView.gameObject);
    }

    public void RemoveBlocks(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (views.TryGetValue(cell, out var view))
            {
                Destroy(view.gameObject);
                views.Remove(cell);
            }
        }
    }

    public void SwapBlockViews(Cell cellA, Cell cellB, BlockView a, BlockView b)
    {
        views[cellA] = b;
        views[cellB] = a;
    }

    public void RebindBlockView(Cell cell, BlockView view)
    {
        views[cell] = view;
    }

}
