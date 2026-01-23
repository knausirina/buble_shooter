public class ChangeGameStateEvent : IEvent
{
    public GameState NewState {get;}

    public ChangeGameStateEvent(GameState gameState)
    {
        NewState = gameState;
    }
}