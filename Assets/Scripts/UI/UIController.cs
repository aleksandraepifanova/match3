using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();

        restartButton.onClick.AddListener(OnRestartClicked);
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnRestartClicked()
    {
        gameController.RestartLevel();
    }

    private void OnNextClicked()
    {
        gameController.LoadNextLevel();
    }
}
