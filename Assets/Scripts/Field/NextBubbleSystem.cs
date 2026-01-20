using Data;
using UnityEngine;
using Views;

public class NextBubbleSystem
{
    private readonly PoolBalls _poolBalls;
    private readonly Config _config;
    private GameContext _gameContext;
    private GameParameters _gameParameters;

    private Color _nextColor;
    private Color _prevColor;

    public NextBubbleSystem(Config config, PoolBalls poolBalls)
    {
        _config = config;
        _poolBalls = poolBalls;
    }

    public void SetData(GameContext gameContext, GameParameters gameParameters)
    {
        _gameContext = gameContext;
        _gameParameters = gameParameters;
    }

    public void GenerateNextColorOfBubble()
    {
        var color = Random.Range(1, System.Enum.GetValues(typeof(ColorEnum)).Length);
        var colorEnum = (ColorEnum)color;
        _nextColor = _config.GetColorByEnum(colorEnum);
    }

    public Color GetNextColorOfBubble()
    {
        return _nextColor;
    }

    public BubbleView GetNextBubble()
    {
        var worldPosition = _gameContext.GetRightBottomAngle();
        worldPosition.x = 0;
        worldPosition.y += _config.ShooterHeight;
        worldPosition.z = _gameContext.Camera.nearClipPlane;

        var bubbleView = _poolBalls.Pool.Get();
        bubbleView.SetColor(_nextColor);

        bubbleView.transform.position = worldPosition;
        bubbleView.Renderer.gameObject.transform.localScale = Vector3.one * _gameParameters.BallSize;

        return bubbleView;
    }
}