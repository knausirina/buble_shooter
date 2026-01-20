using UnityEngine;

public class PopupsContext : MonoBehaviour
{
    static private PopupsContext _instance = null;

    [field: SerializeField] public Transform Root { get; private set;}

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}