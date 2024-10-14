using System;
using Data;
using Field;
using UnityEngine;
using Object = UnityEngine.Object;

public class Game
{
    public GameState GameState => _gameState;

    public Action GameStateChanged; 
    private GameState _gameState = GameState.Stop;
    private Vector2Int _fieldSizeInPixels;
    private Vector2Int _fieldTypeInPixel;

    private readonly Config _config;
    
    private BubblesData _bubblesData;
    private FieldDataFromFile _fieldDataFromFile;
    private BuilderBubbleDataByString _builderBubbleDataByString;
    private FieldBuilder _fieldBuilder;
    private FieldDecoration _fieldDecoration;
    private GameContext _gameContext;
    private NextBubbleSystem _nextBubbleSystem;
    private PoolBalls _poolBalls;
    private GameProcess _gameProcess;

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
        _fieldBuilder.Build(_gameContext, _bubblesData, _fieldSizeInPixels, _fieldTypeInPixel);
        
        _nextBubbleSystem ??= new NextBubbleSystem(_config, _poolBalls);
        
        _gameContext.SlingShot.Construct(this);

        _gameProcess ??= new GameProcess(this, _config, _nextBubbleSystem, _fieldBuilder.SizeBall);
        
        GameStateChanged?.Invoke();
        
        _gameProcess.Run();
    }

    public void Stop()
    {
        _gameState = GameState.Stop;
        
        _fieldBuilder.Clear();
        GameStateChanged?.Invoke();
        
        _poolBalls.Pool.Clear();
    }
}