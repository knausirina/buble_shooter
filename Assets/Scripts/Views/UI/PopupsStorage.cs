using System;
using System.Collections.Generic;
using UnityEngine;
using Component = UnityEngine.Component;

public class PopupsStorage
{
    public static PopupsStorage Instance { get; private set; }

    private readonly PopupsConfig _viewsConfig;
    private readonly PopupsContext _viewsContext;

    private Dictionary<Type, Component> _activePopups = new Dictionary<Type, Component>();

    public PopupsStorage(PopupsConfig viewsConfig, PopupsContext viewsContext)
    {
        Instance = this;

        _viewsConfig = viewsConfig;
        _viewsContext = viewsContext;
    }

    public T GetView<T>() where T: Component
    {
        Type type = typeof(T);
        if (_activePopups.ContainsKey(type))
        {
            return (T)_activePopups[type];
        }

        var prefab = _viewsConfig.GetPrefab<T>();
        var viewGameObject = UnityEngine.Object.Instantiate(prefab, _viewsContext.Root, true);
        viewGameObject.transform.position = Vector3.zero;
        viewGameObject.transform.localPosition = Vector3.zero;
        var component = viewGameObject.GetComponent<T>();

         _activePopups.Add(type, component);
        return component;
    }
}