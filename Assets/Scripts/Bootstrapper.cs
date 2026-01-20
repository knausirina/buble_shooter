using UnityEditor;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PopupsConfig _viewsConfig;
    [SerializeField] private PopupsContext _viewsContext;
    [SerializeField] private Config _config;

    private bool hasBeenBootstrapped;

    private void Awake() => BootstrapOnDemand();

    public void BootstrapOnDemand()
    {
        if (hasBeenBootstrapped) return;
        hasBeenBootstrapped = true;
        Bootstrap();
    }

    private void Bootstrap()
    {
        var viewsService = new PopupsStorage(_viewsConfig, _viewsContext);
        ServiceLocator.Register(viewsService);

        var game = new Game(_config);
        ServiceLocator.Register(game);

        Debug.Log("all registred");
    }
}