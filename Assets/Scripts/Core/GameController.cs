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

    private void OnMoveCompleted(Cell a, Cell b)
    {
        ApplyLocalGravity();

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

        ApplyLocalGravity();
        inputHandler.UnlockInput();
    }

    private System.Collections.IEnumerator RevertMoveAfterDelay(
                                                        Cell a,
                                                        Cell b,
                                                        float delay)
    {
        yield return new WaitForSeconds(delay);

        grid.SwapBlocks(a, b);

        fieldView.SwapVisual(a, b);

        inputHandler.UnlockInput();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null)
            {
                return;
            }

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0; 

            inputHandler.OnPointerClick(worldPos);
        }
    }
    private void ApplyLocalGravity()
    {
        bool anyFell;

        do
        {
            anyFell = false;

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 1; y < grid.Height; y++)
                {
                    Cell cell = grid.GetCell(x, y);

                    if (!grid.CanFall(cell))
                        continue;

                    Cell newCell = grid.FallOne(cell);
                    fieldView.MoveBlockDown(cell, newCell);

                    anyFell = true;
                }
            }

        } while (anyFell);
    }

}
