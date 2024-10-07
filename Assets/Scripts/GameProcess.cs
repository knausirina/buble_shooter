using Field;

public class GameProcess
{
    private const float OffsetBallPositionY = 0.5f;
        
    private readonly NextBubbleSystem _nextBubbleSystem;
    private readonly GameContext _gameContext;

    public GameProcess(GameContext gameContext, NextBubbleSystem nextBubbleSystem)
    {
        _gameContext = gameContext;
        _nextBubbleSystem = nextBubbleSystem;
            
        _gameContext.SlingShotLines.ToggleActive(false);
        _gameContext.SlingShot.ToggleActive(false);
    }

    public void Run()
    {
        var worldPosition = _gameContext.GetBottomPointOfFiledWorldPosition();
        worldPosition.y += OffsetBallPositionY;
            
        var bubbleView = _nextBubbleSystem.GetNextBubble();
        bubbleView.transform.position = worldPosition;

        _gameContext.SlingShotLines.gameObject.transform.position = worldPosition;
        _gameContext.SlingShotLines.SetHolder(bubbleView.gameObject.transform);
        _gameContext.SlingShot.SetTargetSpriteRenderer(bubbleView.Renderer);
            
        _gameContext.SlingShotLines.UpdatePositions();
        _gameContext.SlingShotLines.ToggleActive(true);
        _gameContext.SlingShot.ToggleActive(true);
    }
}