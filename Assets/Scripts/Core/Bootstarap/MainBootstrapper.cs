using UnityEngine;

public class MainBootstrapper : MonoBehaviour
{
    [SerializeField] private PopupsConfig _viewsConfig;
    [SerializeField] private PopupsContext _viewsContext;

    private bool _hasBeenBootstrapped;
    private static MainBootstrapper _instance;

    private void Awake() => BootstrapOnDemand();

    public void BootstrapOnDemand()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (_hasBeenBootstrapped) return;
        _hasBeenBootstrapped = true;
        Bootstrap();
    }

    private void Bootstrap()
    {
        var viewsService = new PopupsStorage(_viewsConfig, _viewsContext);
        ServiceLocator.Global.Register(viewsService);

        Debug.Log("all registred");
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}