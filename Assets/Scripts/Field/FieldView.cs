using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [SerializeField] private BlockView blockPrefab;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float cellPadding = 0.1f;
    [SerializeField] private int baseSortingOrder = 10;

    public Action<BlockView> OnBlockClicked;

    private Dictionary<Cell, BlockView> views = new Dictionary<Cell, BlockView>();

    private Grid grid;

    public void Init(Grid grid)
    {
        this.grid = grid;

        FitCameraToGrid();
        //CalculateCellSize();
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
                NormalizeBlockScale(blockView);

                blockView.transform.localPosition = new Vector3(
                    cell.Position.x * cellSize,
                    cell.Position.y * cellSize,
                    0
                );
                UpdateSortingOrder(blockView);

                views[cell] = blockView;

                blockView.OnClicked += HandleBlockClicked;
            }
        }
    }

    private void NormalizeBlockScale(BlockView view)
    {
        var sr = view.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
            return;

        Sprite sprite = sr.sprite;

        float spritePixels = sprite.rect.width;
        float ppu = sprite.pixelsPerUnit;
        float spriteWorldSize = spritePixels / ppu;
        float targetSize = cellSize * (1f);
        float scale = (targetSize / spriteWorldSize) * 1.35f;

        view.transform.localScale = Vector3.one * scale;
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

    public IEnumerator RemoveBlocks(List<Cell> cells)
    {
        var viewsToDestroy = new List<BlockView>();

        foreach (var cell in cells)
        {
            if (views.TryGetValue(cell, out var view))
            {
                //Destroy(view.gameObject);
                views.Remove(cell);
                viewsToDestroy.Add(view);
            }
        }
        foreach (var view in viewsToDestroy)
        {
            yield return view.PlayDestroy();
        }
    }
    public void HighlightCell(Cell cell)
    {
        if (views.TryGetValue(cell, out var view))
        {
            view.transform.localScale = Vector3.one;
        }
    }

    public void UnhighlightCell(Cell cell)
    {
        if (views.TryGetValue(cell, out var view))
        {
            view.transform.localScale = Vector3.one;
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
    public Cell GetCellFromWorldPosition(Vector3 worldPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos);

        int x = Mathf.RoundToInt(localPos.x / cellSize);
        int y = Mathf.RoundToInt(localPos.y / cellSize);

        if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height)
            return null;

        return grid.GetCell(x, y);
    }

    public void SwapVisual(Cell a, Cell b)
    {
        views.TryGetValue(a, out var viewA);
        views.TryGetValue(b, out var viewB);

        if (viewA != null) views[b] = viewA; else views.Remove(b);
        if (viewB != null) views[a] = viewB; else views.Remove(a);

        if (viewA != null)
        {
            viewA.UpdateCell(b);
            UpdateBlockPosition(viewA);
            UpdateSortingOrder(viewA);
        }

        if (viewB != null)
        {
            viewB.UpdateCell(a);
            UpdateBlockPosition(viewB);
            UpdateSortingOrder(viewB);
        }
    }
    public void MoveBlockDown(Cell from, Cell to)
    {
        if (!views.TryGetValue(from, out var view))
            return;

        views.Remove(from);
        views[to] = view;

        view.UpdateCell(to);
        UpdateBlockPosition(view);
        UpdateSortingOrder(view);
    }

    private void UpdateSortingOrder(BlockView view)
    {
        var sr = view.GetComponent<SpriteRenderer>();
        if (sr == null)
            return;

        sr.sortingOrder = baseSortingOrder + view.Cell.Position.y;
    }
    public void FitCameraToGrid()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float gridWidth = grid.Width * cellSize;
        float gridHeight = grid.Height * cellSize;

        float halfHeight = gridHeight / 2f;
        float halfWidth = gridWidth / cam.aspect / 2f;

        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);
    }

}
