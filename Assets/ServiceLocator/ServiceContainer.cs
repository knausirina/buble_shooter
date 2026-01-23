using System;
using System.Collections.Generic;

public class ServiceContainer
{
    private readonly Dictionary<Type, object> _services = new();
    private readonly ServiceContainer _parent;

    public ServiceContainer(ServiceContainer parent = null)
    {
        _parent = parent;
    }

    public void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        return _parent?.Get<T>();
    }
}