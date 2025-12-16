using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private FieldView fieldView;

    private Grid grid;
    private InputHandler inputHandler;
    private MatchFinder matchFinder;

    private void Start()
    {
        //var grid = new Grid(7, 7);
        grid = new Grid(5, 2);

        grid.GetCell(0, 1).Block = new Block(BlockType.Blue);
        grid.GetCell(2, 1).Block = new Block(BlockType.Orange);
        grid.GetCell(0, 0).Block = new Block(BlockType.Blue);
        grid.GetCell(2, 0).Block = new Block(BlockType.Blue);
        grid.GetCell(3, 0).Block = new Block(BlockType.Orange);
        grid.GetCell(4, 0).Block = new Block(BlockType.Orange);

        fieldView.Init(grid);

        matchFinder = new MatchFinder(grid);
        inputHandler = new InputHandler(grid, fieldView);

        fieldView.OnBlockClicked += inputHandler.OnBlockClicked;
        inputHandler.OnMoveCompleted += OnMoveCompleted;
    }
    private void CheckMatches()
    {
        var matches = matchFinder.FindMatches();

        foreach (var area in matches)
        {
            grid.RemoveBlocks(area);
            fieldView.RemoveBlocks(area);
        }
    }

    private void OnMoveCompleted(BlockView a, BlockView b)
    {
        var matches = matchFinder.FindMatches();

        if (matches.Count > 0)
        {
            StartCoroutine(RemoveMatchesAfterDelay(matches, 1f));
        }
        else
        {
            StartCoroutine(RevertMoveAfterDelay(a, b, 1f));
        }
    }

    private System.Collections.IEnumerator RemoveMatchesAfterDelay(
    System.Collections.Generic.List<System.Collections.Generic.List<Cell>> matches,
    float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var area in matches)
        {
            grid.RemoveBlocks(area);
            fieldView.RemoveBlocks(area);
        }

        inputHandler.UnlockInput();
    }

    private System.Collections.IEnumerator RevertMoveAfterDelay(
                                                                BlockView a,
                                                                BlockView b,
                                                                float delay)
    {
        yield return new WaitForSeconds(delay);

        Cell cellA = a.Cell;
        Cell cellB = b.Cell;

        grid.SwapBlocks(cellA, cellB);

        a.UpdateCell(cellB);
        b.UpdateCell(cellA);

        fieldView.UpdateBlockPosition(a);
        fieldView.UpdateBlockPosition(b);
        inputHandler.UnlockInput();
    }



}
