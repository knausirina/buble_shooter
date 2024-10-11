using Field;
using UnityEngine;

public class GameProcess
{
    private const float OffsetBallPositionY = 1f;
        
    private readonly NextBubbleSystem _nextBubbleSystem;
    private readonly GameContext _gameContext;
    private readonly Config _config;
    private float _ballSize;

    public GameProcess(Game game, Config config, NextBubbleSystem nextBubbleSystem, float ballSize)
    {
        _gameContext = game.GameContext;
        _config = config;
        _nextBubbleSystem = nextBubbleSystem;
        _ballSize = ballSize;
            
        game.GameContext.SlingShotLines.ToggleActive(false);
        game.GameContext.SlingShot.ToggleActive(false);
    }

    public void Run()
    {
        var worldPosition = _gameContext.GetBottomPointOfFiledWorldPosition();
        worldPosition.y += _config.ShooterHeight;
            
        var bubbleView = _nextBubbleSystem.GetNextBubble();
        bubbleView.transform.position = worldPosition;
        bubbleView.transform.localScale = Vector3.one * _ballSize;

        _gameContext.SlingShotLines.gameObject.transform.position = worldPosition;
        _gameContext.SlingShotLines.SetHolder(bubbleView.gameObject.transform);
        _gameContext.SlingShot.SetTargetSpriteRenderer(bubbleView.Renderer);
            
        _gameContext.SlingShotLines.UpdatePositions();
        _gameContext.SlingShotLines.ToggleActive(true);
        _gameContext.SlingShot.ToggleActive(true);
    }
}