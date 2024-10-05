using System;
using Data;
using Field;
using Object = UnityEngine.Object;

public class Game
{
    public GameState GameState => _gameState;

    public Action GameStateChanged; 
    private GameState _gameState = GameState.Stop;

    private readonly Config _config;
    
    private BublesData _bublesData;
    private FieldDataFromFile _fieldDataFromFile;
    private BuilderBubleDataByString _builderBubleDataByString;
    private FieldBuilder _fieldBuilder;
    private FieldDecoration _fieldDecoration;
    private GameContext _gameContext;

    public GameContext GameContext => _gameContext;
    
    public Game(Config config)
    {
        _config = config;
    }

    public void Start()
    {
        _fieldDataFromFile ??= new FieldDataFromFile(_config);

        _bublesData = (_builderBubleDataByString ?? new BuilderBubleDataByString()).GetData(_fieldDataFromFile.GetData());

        _gameContext = Object.FindObjectOfType<GameContext>();
        
        _fieldBuilder ??= new FieldBuilder(_config);
        _fieldBuilder.Build(_gameContext, _bublesData);

        _fieldDecoration ??= new FieldDecoration();
        _fieldDecoration.Build(_gameContext, _bublesData);
        
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