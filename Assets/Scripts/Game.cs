using Data;
using Field;
using GamePlay;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

public class Game
{
    public GameState GameState => _gameState;
    
    private GameState _gameState = GameState.Stop;
    private Vector2Int _fieldSizeInPixels;
    private Vector2Int _fieldTypeInPixel;

    private readonly Config _config;
    
    private BubblesData _bubblesData;
    private FieldDataFromFile _fieldDataFromFile;
    private BuilderBubbleDataByString _builderBubbleDataByString;
    private FieldBuilder _fieldBuilder;
    private BubblesContact _bubblesContact;
    private FieldDecoration _fieldDecoration;
    private GameContext _gameContext;
    private NextBubbleSystem _nextBubbleSystem;
    private PoolBalls _poolBalls;

    private BubbleView[,] _bubbleViews;

    public GameContext GameContext => _gameContext;
    public Vector2Int FieldSizeInPixels => _fieldSizeInPixels;
    public Vector2Int FieldTypeInPixel => _fieldTypeInPixel;
    
    public Game(Config config)
    {
        _config = config;
    }

    public void Start()
    {
        if (_gameState == GameState.Play)
        {
            return;
        }
        _gameState = GameState.Play;

        _fieldDataFromFile ??= new FieldDataFromFile(_config);
        
        _bubblesData = (_builderBubbleDataByString ?? new BuilderBubbleDataByString(_config)).GetData(_fieldDataFromFile.GetData(), out _fieldSizeInPixels, out _fieldTypeInPixel);

        _gameContext = Object.FindObjectOfType<GameContext>();
        
        _fieldDecoration ??= new FieldDecoration();
        _fieldDecoration.Build(this);

        _poolBalls ??= new PoolBalls(_config.BubbleView.gameObject);
        
        _fieldBuilder ??= new FieldBuilder(_config, _poolBalls);
        _bubbleViews = _fieldBuilder.Build(_gameContext, _bubblesData, _fieldTypeInPixel);

        _bubblesContact = new BubblesContact();
        _bubblesContact.SetData(_fieldBuilder, _bubbleViews, _fieldBuilder.SizeBall);
        _bubblesContact.BubbleShoot += OnBubbleShoot;
        
        _nextBubbleSystem ??= new NextBubbleSystem(_config, _poolBalls);
        
        _gameContext.SlingShot.Construct(this, _bubblesContact);
        
        SetNextBall();
        
        GameContext.SlingShot.BallFinishedAction += SetNextBall;
    }
    
    private void SetNextBall()
    {
        Debug.Log("SetNextBall");
        var worldPosition = GameContext.GetRightBottomAngle();
        worldPosition.x = 0;
        worldPosition.y += _config.ShooterHeight;
        worldPosition.z = GameContext.Camera.nearClipPlane;

        var bubbleView = _nextBubbleSystem.GetNextBubble();
        bubbleView.transform.position = worldPosition;
        bubbleView.Renderer.gameObject.transform.localScale = Vector3.one * _fieldBuilder.SizeBall;

        GameContext.SlingShot.SetHolder(bubbleView);
        
        GameContext.SlingShotLines.ToggleActive(true);
        GameContext.SlingShot.ToggleActive(true);
        
        GameContext.SlingShot.AllowShoot();
    }

    public void Stop()
    {
        _gameState = GameState.Stop;
        
        _bubblesContact.BubbleShoot -= OnBubbleShoot;

        foreach (var bubble in _bubbleViews)
        {
            _poolBalls.Pool.Release(bubble);
        }

        _bubbleViews = null;

        _poolBalls.Pool.Clear();
    }

    private void OnBubbleShoot(BubbleView bubbleView, BubblePosition bubblePosition)
    {
        Debug.Log("xxx OnBubbleShoot column= " + bubblePosition.Column + " row = " + bubblePosition.Column);
        _gameContext.SlingShot.DisableShoot();

        SetNextBall();
        
        _gameContext.SlingShot.ToggleAllowControlBall(true);
    }
}