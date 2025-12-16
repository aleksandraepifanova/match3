using UnityEngine;

public class InputHandler
{
    private BlockView selectedBlock;

    public void OnBlockClicked(BlockView block)
    {
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
        
    }
}
