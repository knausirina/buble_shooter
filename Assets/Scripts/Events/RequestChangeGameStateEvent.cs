public class RequestChangeGameStateEvent : IEvent
{
    public GameState NewGameState { get; private set; }

    public RequestChangeGameStateEvent(GameState newState)
    {
        NewGameState = newState;
    }
}