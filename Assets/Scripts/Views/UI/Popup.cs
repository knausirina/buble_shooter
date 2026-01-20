using UnityEngine;

public class Popup : MonoBehaviour
{
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}