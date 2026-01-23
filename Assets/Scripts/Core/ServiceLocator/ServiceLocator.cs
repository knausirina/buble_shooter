public static class ServiceLocator
{
    public static ServiceContainer Global { get; } = new ServiceContainer();
    public static ServiceContainer Scene { get; private set; }

    public static void InitializeSceneScope()
    {
        Scene = new ServiceContainer(Global);
    }
}