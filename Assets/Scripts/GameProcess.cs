using System;
using Field;
using UnityEngine;

public class GameProcess : IDisposable
{
    private readonly NextBubbleSystem _nextBubbleSystem;
    private readonly Game _game;
    private readonly Config _config;
    private readonly float _ballSize;

    public GameProcess(Game game, Config config, NextBubbleSystem nextBubbleSystem, float ballSize)
    {
        _game = game;
        _config = config;
        _nextBubbleSystem = nextBubbleSystem;
        _ballSize = ballSize;
    }

    public void Run()
    {
        SetNextBall();
        
        _game.GameContext.SlingShot.BallFinishedAction += SetNextBall;
    }

    private void SetNextBall()
    {
        var worldPosition = _game.GameContext.GetRightBottomAngle();
        worldPosition.x = 0;
        worldPosition.y += _config.ShooterHeight;
        worldPosition.z = _game.GameContext.Camera.nearClipPlane;

        var bubbleView = _nextBubbleSystem.GetNextBubble();
        bubbleView.transform.position = worldPosition;
        bubbleView.transform.localScale = Vector3.one * _ballSize;

        _game.GameContext.SlingShot.SetHolder(bubbleView);
        
        _game.GameContext.SlingShotLines.ToggleActive(true);
        _game.GameContext.SlingShot.ToggleActive(true);

    }

    public void Dispose()
    {
        _game.GameContext.SlingShot.BallFinishedAction -= SetNextBall;
    }
}