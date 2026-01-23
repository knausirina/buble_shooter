using System;

public class ClosePopupEvent : IEvent
{
    public Type type;

    public ClosePopupEvent(Type typePopup)
    {
        type = typePopup;
    }
}