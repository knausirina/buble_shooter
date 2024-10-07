using Field;
using UnityEngine;

public class GameProcess
{
    private const float OffsetBallPositionY = 0.5f;
        
    private readonly NextBubbleSystem _nextBubbleSystem;
    private readonly GameContext _gameContext;
    private float _ballSize;

    public GameProcess(GameContext gameContext, NextBubbleSystem nextBubbleSystem, float ballSize)
    {
        _gameContext = gameContext;
        _nextBubbleSystem = nextBubbleSystem;
        _ballSize = ballSize;
            
        _gameContext.SlingShotLines.ToggleActive(false);
        _gameContext.SlingShot.ToggleActive(false);
    }

    public void Run()
    {
        var worldPosition = _gameContext.GetBottomPointOfFiledWorldPosition();
        worldPosition.y += OffsetBallPositionY;
            
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