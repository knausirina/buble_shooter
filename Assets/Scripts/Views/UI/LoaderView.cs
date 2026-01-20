using UnityEngine;

public class LoaderView : MonoBehaviour
{
    public static LoaderView Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ToggleShow(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ToggleShow(bool isVisible)
    {
        Instance.gameObject.SetActive(isVisible);
    }
}
