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

        
        _bubblesData = (_builderBubbleDataByString ?? new BuilderBubbleDataByString()).GetData(_fieldDataFromFile.GetData(), out _fieldSizeInPixels, out _fieldTypeInPixel);

        _gameContext = Object.FindObjectOfType<GameContext>();
        
        _fieldDecoration ??= new FieldDecoration();
        _fieldDecoration.Build(this, _bubblesData);
        
        _fieldBuilder ??= new FieldBuilder(_config);
        _fieldBuilder.Build(_gameContext, _bubblesData, _fieldSizeInPixels, _fieldTypeInPixel);
        
        _gameContext.Slingshot.Construct(this);
        
        _gameState = GameState.Play;
        GameStateChanged?.Invoke();
    }

    public void Stop()
    {
        _gameState = GameState.Stop;
        GameStateChanged?.Invoke();
    }
}