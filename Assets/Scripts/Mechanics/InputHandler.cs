using System;
using UnityEngine;

public class InputHandler
{
    private BlockView selectedBlock;
    private Grid grid;
    private FieldView fieldView;
    private bool inputLocked;

    public Action<BlockView, BlockView> OnMoveCompleted;

    public InputHandler(Grid grid, FieldView fieldView)
    {
        this.grid = grid;
        this.fieldView = fieldView;
    }

    public void OnBlockClicked(BlockView block)
    {
        if (inputLocked)
            return;

        if (selectedBlock == null)
        {
            Select(block);
        }
        else
        {
            TryMove(selectedBlock, block);
            Deselect();
        }
    }

    private void Select(BlockView block)
    {
        selectedBlock = block;
        block.transform.localScale = Vector3.one * 1.1f;
    }

    private void Deselect()
    {
        if (selectedBlock != null)
            selectedBlock.transform.localScale = Vector3.one;

        selectedBlock = null;
    }

    private void TryMove(BlockView from, BlockView to)
    {
        Cell fromCell = from.Cell;
        Cell toCell = to.Cell;

        if (!grid.AreNeighbours(fromCell, toCell))
            return;

        grid.SwapBlocks(fromCell, toCell);

        from.UpdateCell(toCell);
        to.UpdateCell(fromCell);

        fieldView.RebindBlockView(toCell, from);
        fieldView.RebindBlockView(fromCell, to);

        fieldView.UpdateBlockPosition(from);
        fieldView.UpdateBlockPosition(to);

        inputLocked = true;
        OnMoveCompleted?.Invoke(from, to);
    }

    public void UnlockInput()
    {
        inputLocked = false;
    }
}
