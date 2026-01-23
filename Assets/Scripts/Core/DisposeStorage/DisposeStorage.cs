using System;
using System.Collections.Generic;

public class DisposeStorage
{
    private readonly List<IDisposable> _resources = new();

    public void Register(IDisposable resource) => _resources.Add(resource);

    public void Dispose()
    {
        for (var i = _resources.Count - 1; i >= 0; i--)
        {
            _resources[i]?.Dispose();
        }
        _resources.Clear();
    }
}