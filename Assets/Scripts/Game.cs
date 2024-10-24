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

    private readonly Config _config;
    
    private BubblesData _bubblesData;
    private FieldDataFromFile _fieldDataFromFile;
    private BuilderBubbleDataByString _builderBubbleDataByString;
    private FieldBuilder _fieldBuilder;
    private BubblesContactSystem _bubblesContactSystem;
    private GameContext _gameContext;
    private NextBubbleSystem _nextBubbleSystem;
    private ResultGameSystem _resultGameSystem;
    private PoolBalls _poolBalls;
    private GameParameters _gameParameters;

    private int _generatedBubblesCount = 0;

    public GameContext GameContext => _gameContext;
    
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

        Vector2Int fieldSizeInPixels;
        Vector2Int fieldSizeInElements;
        int maxCountBubbles;

        _bubblesData = (_builderBubbleDataByString ?? new BuilderBubbleDataByString(_config))
            .GetData(_fieldDataFromFile.GetData(), out fieldSizeInPixels, out fieldSizeInElements, out maxCountBubbles);

        _gameContext = Object.FindObjectOfType<GameContext>();
        
        _gameContext.FieldRectTransform.sizeDelta = new Vector2(fieldSizeInPixels.x, fieldSizeInPixels.y);

        _poolBalls ??= new PoolBalls(_config.BubbleView.gameObject);
        
        _fieldBuilder ??= new FieldBuilder(_config, _poolBalls);
        _fieldBuilder.Build(_gameContext, _bubblesData, fieldSizeInElements);

        _gameParameters = new GameParameters(_fieldBuilder.BallSize, fieldSizeInPixels, fieldSizeInElements, maxCountBubbles);

        _resultGameSystem ??= new ResultGameSystem(_config);

        _bubblesContactSystem = new BubblesContactSystem();
        _bubblesContactSystem.SetData(_fieldBuilder);
        _bubblesContactSystem.BubbleShoot += OnBubbleShoot;
        
        _nextBubbleSystem ??= new NextBubbleSystem(_config, _poolBalls);
        _nextBubbleSystem.SetData(_gameContext, _gameParameters);

        _gameContext.SlingShot.Construct(this, _bubblesContactSystem);
        
        SetNextBall();
        
        GameContext.SlingShot.BallFinishedAction += SetNextBall;
    }
    
    private void SetNextBall()
    {
        if (_generatedBubblesCount >= _gameParameters.MaxCountBubbles)
        {
            var isWin = _resultGameSystem.IsWin(_fieldBuilder.BubblesViews, _gameParameters.FieldSizeInElements);
            return;
        }

        Debug.Log("SetNextBall");
        _generatedBubblesCount++;

        _nextBubbleSystem.GenerateNextColorOfBubble();

        var bubbleView = _nextBubbleSystem.GetNextBubble();

        _nextBubbleSystem.GenerateNextColorOfBubble();

        GameContext.NextBubbleView.SetCount(_gameParameters.MaxCountBubbles - _generatedBubblesCount);
        GameContext.NextBubbleView.SetColor(_nextBubbleSystem.GetNextColorOfBubble());

        GameContext.SlingShot.SetHolder(bubbleView);
    }

    public void Stop()
    {
        _gameState = GameState.Stop;
        
        _bubblesContactSystem.BubbleShoot -= OnBubbleShoot;

        foreach (var bubble in _fieldBuilder.BubblesViews)
        {
            _poolBalls.Pool.Release(bubble);
        }

        _fieldBuilder.Clear();

        _poolBalls.Pool.Clear();
    }

    private void OnBubbleShoot(BubbleView bubbleView)
    {
        _gameContext.SlingShot.DisableShoot();

        SetNextBall();
    }
}