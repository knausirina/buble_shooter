using UnityEngine;

public class GameBoostrapper : MonoBehaviour
{
    [SerializeField] private Config _config;
    [SerializeField] private GameContext _gameContext;

    private void Awake()
    {
        ServiceLocator.InitializeSceneScope();

        ConfigureSceneLocator(ServiceLocator.Scene);
    }

    private void ConfigureSceneLocator(ServiceContainer sceneServiceContainer)
    {
        var game = new Game(_config);

        DisposableService.Scene.Register(game);

        sceneServiceContainer.Register(_gameContext);
    }
}