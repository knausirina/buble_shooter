using System.Collections.Generic;
using Data;
using Field;

public class Game
{
    private GameState _gameState = GameState.Stop;
    private BubleData[][] _bubleData;

    public GameState GameState => _gameState;

    private readonly Config _config;
    
    public Game(Config config)
    {
        _config = config;
    }

    public void Start()
    {
        _gameState = GameState.Play;
        
        var fieldDataFromFile = new FieldDataFromFile(_config);
        var fieldDataString = fieldDataFromFile.GetData();

        _bubleData = new BuilderBubleData().GetData(fieldDataString);
    }
}