using Data;
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
    private BuilderBubbleData _builderBubbleData;
    private FieldBuilder _fieldBuilder;
    private BubblesContactSystem _bubblesContactSystem;
    private GameContext _gameContext;
    private NextBubbleSystem _nextBubbleSystem;
    private ResultGameSystem _resultGameSystem;
    private PoolBalls _poolBalls;
    private GameParameters _gameParameters;

    private ResultPopup _resultGameView;
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

        _builderBubbleData ??= new BuilderBubbleData(_config);

        var textAsset = _config.FieldTextAsset;
        var fieldData = textAsset.text;

        _gameParameters = new GameParameters();
        _bubblesData = _builderBubbleData.GetData(fieldData, ref _gameParameters);

        _gameContext = Object.FindObjectsByType<GameContext>(FindObjectsSortMode.None)[0];

        _gameContext.FieldRectTransform.sizeDelta = _gameParameters.FieldSizeInPixels;//_gameParameters.FieldSizeInElements;

        _poolBalls ??= new PoolBalls(_config.BubbleView.gameObject);
        
        _fieldBuilder ??= new FieldBuilder(_config, _poolBalls);
        _fieldBuilder.Build(_gameContext, _bubblesData, _gameParameters);

        _gameParameters.BallSize = _fieldBuilder.BallSize;
        Debug.Log("_gameParameters.BallSize = " + _gameParameters.BallSize);

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
        Debug.Log("SetNextBall");

        if (_generatedBubblesCount >= _gameParameters.MaxCountBubbles)
        {
            var isWin = _resultGameSystem.IsWin(_fieldBuilder.BubblesViews, _gameParameters.FieldSizeInElements);
            ShowResultGame(isWin);
            Stop();
            return;
        }
       
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
        Debug.Log("xxxx Stop");
        _gameState = GameState.Stop;
        
        _bubblesContactSystem.BubbleShoot -= OnBubbleShoot;

        _fieldBuilder.Clear();
        _poolBalls.Pool.Clear();
    }

    private void OnBubbleShoot(BubbleView bubbleView)
    {
        _gameContext.SlingShot.DisableShoot();

        SetNextBall();
    }

    private void ShowResultGame(bool isWin)
    {
        var viewService = ServiceLocator.Get<PopupsStorage>();
        var resultView = viewService.GetView<ResultPopup>();

        resultView.IsWin = isWin;
        resultView.Show();
    }
}