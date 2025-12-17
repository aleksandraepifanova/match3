using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private FieldView fieldView;
    [SerializeField] private int firstLevelIndex = 1;
    [SerializeField] private int lastLevelIndex = 3;

    private int currentLevelIndex;

    private Grid grid;
    private InputHandler inputHandler;
    private MatchFinder matchFinder;

    private void Start()
    {
        if (SaveService.HasSave())
        {
            LoadFromSave();
        }
        else
        {
            currentLevelIndex = firstLevelIndex;
            LoadLevel(currentLevelIndex);
        }
    }

    private void LoadLevel(int index)
    {
        var levelData = LevelLoader.Load(index);
        grid = LevelFactory.CreateGrid(levelData);

        fieldView.Init(grid);

        matchFinder = new MatchFinder(grid);
        inputHandler = new InputHandler(grid, fieldView);
        inputHandler.OnMoveCompleted += OnMoveCompleted;
    }

    private void OnMoveCompleted(Cell a, Cell b)
    {
        StartCoroutine(RunNormalization());
    }

    private System.Collections.IEnumerator RunNormalization()
    {
        inputHandler.LockInput();

        bool changed;

        do
        {
            changed = false;

            if (grid.HasFallingBlocks())
            {
                ApplyLocalGravity();
                changed = true;
                yield return new WaitForSeconds(0.1f); 
            }

            var matches = matchFinder.FindMatches();
            if (matches.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);

                foreach (var area in matches)
                {
                    grid.RemoveBlocks(area);
                    yield return fieldView.RemoveBlocks(area);
                }

                changed = true;
            }

        } while (changed);

        inputHandler.UnlockInput();

        ResetNewFlags();
        CheckLevelCompleted();
    }

    private void ResetNewFlags()
    {
        for (int x = 0; x < grid.Width; x++)
            for (int y = 0; y < grid.Height; y++)
                if (!grid.GetCell(x, y).IsEmpty)
                    grid.GetCell(x, y).Block.IsNew = false;
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

    private void CheckLevelCompleted()
    {
        if (!grid.HasAnyBlocks())
        {
            currentLevelIndex++;
            if (currentLevelIndex > lastLevelIndex)
                currentLevelIndex = firstLevelIndex;

            LoadLevel(currentLevelIndex);
        }
    }
    public void RestartLevel()
    {
        SaveService.Clear();
        LoadLevel(currentLevelIndex);
    }

    public void LoadNextLevel()
    {
        SaveGame();
        currentLevelIndex++;

        if (currentLevelIndex > lastLevelIndex)
            currentLevelIndex = firstLevelIndex;

        LoadLevel(currentLevelIndex);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void SaveGame()
    {
        var data = new SaveData
        {
            levelIndex = currentLevelIndex,
            width = grid.Width,
            height = grid.Height,
            cells = new int[grid.Width * grid.Height]
        };

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                int index = y * grid.Width + x;
                Cell cell = grid.GetCell(x, y);

                if (cell.IsEmpty)
                {
                    data.cells[index] = -1;
                }
                else
                {
                    data.cells[index] = (int)cell.Block.Type;
                }
            }
        }

        SaveService.Save(data);
    }

    private void LoadFromSave()
    {
        SaveData data = SaveService.Load();

        currentLevelIndex = data.levelIndex;

        grid = new Grid(data.width, data.height);

        for (int y = 0; y < data.height; y++)
        {
            for (int x = 0; x < data.width; x++)
            {
                int index = y * data.width + x;
                int value = data.cells[index];

                if (value >= 0)
                {
                    grid.GetCell(x, y).Block =
                        new Block((BlockType)value);
                }
            }
        }

        fieldView.Init(grid);

        matchFinder = new MatchFinder(grid);
        inputHandler = new InputHandler(grid, fieldView);
        inputHandler.OnMoveCompleted += OnMoveCompleted;
    }


}
