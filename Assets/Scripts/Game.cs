using System;
using System.Resources;
using Data;
using Field;
using Slingshot;
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
        _fieldDataFromFile ??= new FieldDataFromFile(_config);
        
        _bubblesData = (_builderBubbleDataByString ?? new BuilderBubbleDataByString(_config)).GetData(_fieldDataFromFile.GetData(), out _fieldSizeInPixels, out _fieldTypeInPixel);

        _gameContext = Object.FindObjectOfType<GameContext>();
        
        _fieldDecoration ??= new FieldDecoration();
        _fieldDecoration.Build(this);
        
        _fieldBuilder ??= new FieldBuilder(_config);
        _fieldBuilder.Build(_gameContext, _bubblesData, _fieldSizeInPixels, _fieldTypeInPixel);

        _poolBalls = new PoolBalls(_config.BubbleView.gameObject);
        _nextBubbleSystem = new NextBubbleSystem(_config, _poolBalls);
        
        _gameContext.SlingShot.Construct(this);

        _gameProcess = new GameProcess(this, _config, _nextBubbleSystem, _fieldBuilder.SizeBall);
        
        _gameState = GameState.Play;
        GameStateChanged?.Invoke();
        
        _gameProcess.Run();
    }

    public void Stop()
    {
        _gameState = GameState.Stop;
        GameStateChanged?.Invoke();
    }
}