using Field;
using UnityEngine;

public class GameProcess
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
        _game.GameContext.SlingShotLines.ToggleActive(false);
        _game.GameContext.SlingShot.ToggleActive(false);
        
        var worldPosition = _game.GameContext.GetBottomPointOfFiledWorldPosition();
        worldPosition.y += _config.ShooterHeight;
            
        var bubbleView = _nextBubbleSystem.GetNextBubble();
        bubbleView.transform.position = worldPosition;
        bubbleView.transform.localScale = Vector3.one * _ballSize;

        _game.GameContext.SlingShotLines.gameObject.transform.position = worldPosition;
        _game.GameContext.SlingShotLines.SetHolder(bubbleView.gameObject.transform);
        _game.GameContext.SlingShot.SetTargetSpriteRenderer(bubbleView.Renderer);
            
        _game.GameContext.SlingShotLines.UpdatePositions();
        _game.GameContext.SlingShotLines.ToggleActive(true);
        _game.GameContext.SlingShot.ToggleActive(true);
    }
}