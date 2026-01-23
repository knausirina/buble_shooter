using UnityEngine;

public class Popup : MonoBehaviour
{
    public virtual void Close()
    {
        gameObject.SetActive(false);

        EventBus<ClosePopupEvent>.Publish(new ClosePopupEvent(GetType()));
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}