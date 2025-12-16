using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField] private FieldView fieldView;

    private InputHandler inputHandler;

    private void Start()
    {
        inputHandler = new InputHandler();
        var grid = new Grid(5, 5);

        grid.GetCell(0, 0).Block = new Block(BlockType.Blue);
        grid.GetCell(1, 0).Block = new Block(BlockType.Orange);

        fieldView.Init(grid);

        fieldView.OnBlockClicked += inputHandler.OnBlockClicked;
    }
}
