using Unity.Android.Gradle.Manifest;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SceneDisposableScope : MonoBehaviour
{
    public DisposeStorage Storage { get; } = new();

    private void Awake()
    {
        DisposableService.SetSceneScope(this);
    }

    private void OnDestroy()
    {
        Storage.Dispose();

        DisposableService.SetSceneScope(null);
    }
}