using System;
using UnityEngine;

public class InputHandler
{
    private BlockView selectedBlock;
    private Grid grid;
    private FieldView fieldView;
    private Cell selectedCell;
    private bool inputLocked;

    public Action<Cell, Cell> OnMoveCompleted;

    public InputHandler(Grid grid, FieldView fieldView)
    {
        this.grid = grid;
        this.fieldView = fieldView;
    }
    
    private void TryMove(Cell fromCell, Cell toCell)
    {
        if (!grid.AreNeighbours(fromCell, toCell))
            return;

        grid.SwapBlocks(fromCell, toCell);

        fieldView.SwapVisual(fromCell, toCell);

        inputLocked = true;
        OnMoveCompleted?.Invoke(fromCell, toCell);
    }


    public void UnlockInput()
    {
        inputLocked = false;
    }
    public void OnPointerClick(Vector3 worldPos)
    {
        if (inputLocked)
            return;

        Cell clickedCell = fieldView.GetCellFromWorldPosition(worldPos);
        if (clickedCell == null)
            return;

        HandleCellClick(clickedCell);
    }
    private void HandleCellClick(Cell cell)
    {
        if (selectedCell == null)
        {
            if (cell.IsEmpty)
                return;

            selectedCell = cell;
            fieldView.HighlightCell(cell);
            return;
        }

        fieldView.UnhighlightCell(selectedCell);
        TryMove(selectedCell, cell);
        selectedCell = null;
    }

}
