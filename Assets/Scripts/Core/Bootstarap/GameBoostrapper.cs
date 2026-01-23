using UnityEngine;

public class GameBoostrapper : MonoBehaviour
{
    [SerializeField] private Config _config;

    private void Awake()
    {
        ServiceLocator.InitializeSceneScope();

        ConfigureSceneLocator(ServiceLocator.Scene);
    }

    private void ConfigureSceneLocator(ServiceContainer sceneServiceContainer)
    {
        var game = new Game(_config);
        sceneServiceContainer.Register(game);

        DisposableService.Scene.Register(game);
    }
}