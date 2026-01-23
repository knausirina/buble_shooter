using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupsConfig", menuName = "Configs/PopupsConfig")]
public class PopupsConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> _popupsPrefabs;

    private Dictionary<Type, GameObject> _cache;

    private void Initialize()
    {
        _cache = new Dictionary<Type, GameObject>();

        foreach (var prefab in _popupsPrefabs)
        {
            var components = prefab.GetComponents<MonoBehaviour>();
            foreach (var comp in components)
            {
                if (comp == null) continue;

                Type type = comp.GetType();
                if (!_cache.ContainsKey(type))
                {
                    _cache.Add(type, prefab);
                }
            }
        }
    }

    public GameObject GetPrefab<T>() where T : Component
    {
        if (_cache == null)
            Initialize();

        if (_cache.TryGetValue(typeof(T), out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }
}