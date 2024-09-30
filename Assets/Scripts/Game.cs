using System.Collections.Generic;
using Data;
using Field;

public class Game
{
    private GameState _gameState = GameState.Stop;

    public GameState GameState => _gameState;

    private readonly Config _config;
    
    private BublesData _bublesData;
    private FieldDataFromFile _fieldDataFromFile;
    private BuilderBubleDataByString _builderBubleDataByString;
    private FieldBuilder _fieldBuilder;
    
    public Game(Config config)
    {
        _config = config;
    }

    public void Start()
    {
        _gameState = GameState.Play;

        _fieldDataFromFile ??= new FieldDataFromFile(_config);

        _bublesData = (_builderBubleDataByString ?? new BuilderBubleDataByString()).GetData(_fieldDataFromFile.GetData());

        _fieldBuilder ??= new FieldBuilder(_config);
        _fieldBuilder.Build(_bublesData);
    }
}