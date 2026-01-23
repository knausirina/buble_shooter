public static class DisposableService
{
    public static readonly DisposeStorage Global = new();

    public static DisposeStorage Scene => _sceneScope.Storage;

    private static SceneDisposableScope _sceneScope;
    public static void SetSceneScope(SceneDisposableScope scope) => _sceneScope = scope;
}