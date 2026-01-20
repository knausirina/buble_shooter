using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _gameButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _gameButton.onClick.AddListener(OnGameButton);
        _exitButton.onClick.AddListener(OnExitButton);
    }

    private void OnGameButton()
    {
        OnGameButtonAsync(this.GetCancellationTokenOnDestroy()).Forget(Debug.LogError);
    }

    private async UniTask OnGameButtonAsync(CancellationToken cancellationToken)
    {
        try
        {
            LoaderView.ToggleShow(true);

            await SceneManager.LoadSceneAsync(Scenes.GameScene).WithCancellation(cancellationToken);

            LoaderView.ToggleShow(false);
        }
        catch (OperationCanceledException exception)
        {
            Debug.Log("Cancel operation " + exception.Message);
            LoaderView.ToggleShow(false);
        }
    }

    public void OnExitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnDestroy()
    {
        _gameButton.onClick.RemoveListener(OnGameButton);
        _exitButton.onClick.RemoveListener(OnExitButton);
    }
}