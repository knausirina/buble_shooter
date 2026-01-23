using System;

public static class EventBus<T> where T : IEvent
{
    private static Action<T> _onEvent;
    public static void Subscribe(Action<T> action) => _onEvent += action;
    public static void Unsubscribe(Action<T> action) => _onEvent -= action;
    public static void Publish(T eventArgs) => _onEvent?.Invoke(eventArgs);
}