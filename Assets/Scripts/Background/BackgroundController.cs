using UnityEngine;
using System.Collections.Generic;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private BackgroundBall ballPrefab;
    [SerializeField] private Sprite[] ballSprites;
    [SerializeField] private int maxBalls = 3;

    private List<BackgroundBall> balls = new();

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        SpawnInitialBalls();
    }

    private void SpawnInitialBalls()
    {
        int count = Random.Range(1, maxBalls + 1);

        for (int i = 0; i < count; i++)
            SpawnBall();
    }

    private void SpawnBall()
    {
        Vector3 pos = GetRandomPosition();
        var ball = Instantiate(ballPrefab, pos, Quaternion.identity, transform);
        Sprite randomSprite = ballSprites[Random.Range(0, ballSprites.Length)];
        ball.Init(randomSprite);
        balls.Add(ball);
    }

    private Vector3 GetRandomPosition()
    {
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        float x = Random.Range(-width / 2f, width / 2f);
        float y = Random.Range(-height / 2f, height / 2f);

        return new Vector3(x, y, 0);
    }
}
