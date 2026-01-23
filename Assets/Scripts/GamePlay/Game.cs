using System;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

public class Game : IDisposable, IGameState
{
    public GameState State { get; private set; } = GameState.Stop;

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
    private SlingShot _slingShot;

    private int _generatedBubblesCount = 0;
    private bool _inited = false;
    
    public Game(Config config)
    {
        _config = config;

        EventBus<ClosePopupEvent>.Subscribe(OnClosePopup);
        EventBus<RequestChangeGameStateEvent>.Subscribe(OnRequestChangeGameStateEvent);
    }

    private void OnRequestChangeGameStateEvent(RequestChangeGameStateEvent requestChangeGameStateEvent)
    {
        switch (requestChangeGameStateEvent.NewGameState)
        {
            case GameState.Play:
                if (State == GameState.Play)
                    break;
                Start();
                break;
            case GameState.Stop:
                Stop();
                Clear();
                break;
        }
    }

    public void Dispose()
    {
        EventBus<ClosePopupEvent>.Unsubscribe(OnClosePopup);
        EventBus<RequestChangeGameStateEvent>.Unsubscribe(OnRequestChangeGameStateEvent);

        _slingShot.BallFinishedAction -= SetNextBall;
        _bubblesContactSystem.BubbleShoot -= OnBubbleShoot;
    }

    private void Start()
    {
        Debug.Log("Start _gameState = " + State);

        if (State == GameState.Play)
        {
            Debug.Log("Already started game!");
            return;
        }
        State = GameState.Play;

        EventBus<ChangeGameStateEvent>.Publish(new ChangeGameStateEvent(GameState.Play));

        Init();

        _fieldBuilder.Build(_gameContext, _bubblesData, _gameParameters);
        _gameParameters.BallSize = _fieldBuilder.BallSize;

        _bubblesContactSystem.BubbleShoot += OnBubbleShoot;

        SetNextBall();

        _slingShot.BallFinishedAction += SetNextBall;
    }

    private void Init()
    {
        if (_inited)
            return;
        _inited = true;

        var slinghshotGO = Object.Instantiate(_config.SlinghshotPrefab, Vector3.zero, Quaternion.identity);
        
        _slingShot = slinghshotGO.GetComponentInChildren<SlingShot>();

        _builderBubbleData ??= new BuilderBubbleData(_config);

        var fieldData = _config.FieldTextAsset.text;

        _gameParameters = new GameParameters();
        _bubblesData = _builderBubbleData.GetData(fieldData, ref _gameParameters);
        _gameContext = Object.FindObjectsByType<GameContext>(FindObjectsSortMode.None)[0];
        _gameContext.FieldRectTransform.sizeDelta = _gameParameters.FieldSizeInPixels;
        _poolBalls ??= new PoolBalls(_config.BubblePrefab.gameObject);
        _fieldBuilder ??= new FieldBuilder(_poolBalls);

        _resultGameSystem ??= new ResultGameSystem(_config);

        _bubblesContactSystem = new BubblesContactSystem();
        _bubblesContactSystem.SetData(_fieldBuilder);

        _nextBubbleSystem ??= new NextBubbleSystem(_config, _poolBalls);
        _nextBubbleSystem.SetData(_gameContext, _gameParameters);

        _slingShot.Construct(_bubblesContactSystem, this, _config);
    }

    private void OnClosePopup(ClosePopupEvent popupEvent)
    {
        if (popupEvent.type == typeof(ResultPopup))
        {
            Clear();
        }
    }

    private void SetNextBall()
    {
        if (_generatedBubblesCount >= _gameParameters.MaxCountBubbles)
        {
            Stop();
            var isWin = _resultGameSystem.IsWin(_fieldBuilder.BubblesViews, _gameParameters);
            ShowResultGame(isWin);
            return;
        }
       
        _generatedBubblesCount++;

        _nextBubbleSystem.GenerateNextColorOfBubble();

        var bubbleView = _nextBubbleSystem.GetNextBubble();

        _nextBubbleSystem.GenerateNextColorOfBubble();

        _gameContext.NextBubbleView.SetCount(_gameParameters.MaxCountBubbles - _generatedBubblesCount);
        _gameContext.NextBubbleView.SetColor(_nextBubbleSystem.GetNextColorOfBubble());

        _slingShot.SetHolder(bubbleView);
    }

    public void Stop()
    {
        State = GameState.Stop;

        if (_inited)
        _bubblesContactSystem.BubbleShoot -= OnBubbleShoot;

        EventBus<ChangeGameStateEvent>.Publish(new ChangeGameStateEvent(GameState.Stop));
    }

    public void Clear()
    {
        Debug.Log("xxx clear _fieldBuilder is null = " + (_fieldBuilder == null));

        if (_inited)
        {
            _generatedBubblesCount = 0;
            _fieldBuilder.Clear();
            _poolBalls.Pool.Clear();
        }
    }

    private void OnBubbleShoot(BubbleView bubbleView)
    {
        _slingShot.DisableShoot();

        SetNextBall();
    }

    private void ShowResultGame(bool isWin)
    {
        var viewService = ServiceLocator.Global.Get<PopupsStorage>();
        var resultView = viewService.GetView<ResultPopup>();

        resultView.IsWin = isWin;
        resultView.Show();
    }
}