using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _startGameButton;

    private PopupsStorage _popupsStorage;

    private void Awake()
    {
        _popupsStorage = ServiceLocator.Global.Get<PopupsStorage>();

        _menuButton.onClick.AddListener(OnMenuButton);
        _startGameButton.onClick.AddListener(OnStartGameButton);
    }

    private void OnStartGameButton()
    {
        EventBus<ChangeGameStateEvent>.Publish(new ChangeGameStateEvent(GameState.Play));
    }

    private void OnMenuButton()
    {
        _popupsStorage.CloseAll();

        EventBus<ChangeGameStateEvent>.Publish(new ChangeGameStateEvent(GameState.Stop));

        OnMenuButtonAsync(destroyCancellationToken).Forget();
    }

    private async UniTask OnMenuButtonAsync(CancellationToken cancellationToken)
    {
        try
        {
            LoaderView.ToggleShow(true);

            await SceneManager.LoadSceneAsync(Scenes.MenuScene).WithCancellation(cancellationToken);

            LoaderView.ToggleShow(false);
        }
        catch (OperationCanceledException exception)
        {
            Debug.Log("Cancel operation " + exception.Message);

            LoaderView.ToggleShow(false);
        }
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveListener(OnMenuButton);
        _startGameButton.onClick.RemoveListener(OnStartGameButton);
    }
}