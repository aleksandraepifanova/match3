using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private FieldView fieldView;

    private InputHandler inputHandler;

    private void Start()
    {
        var grid = new Grid(7, 7);

        grid.GetCell(0, 0).Block = new Block(BlockType.Blue);
        grid.GetCell(1, 0).Block = new Block(BlockType.Orange);

        fieldView.Init(grid);

        inputHandler = new InputHandler(grid, fieldView);

        fieldView.OnBlockClicked += inputHandler.OnBlockClicked;
    }
}
