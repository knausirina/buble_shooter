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
    [SerializeField] private ResultPopup _resultGameView;

    private GameLauncher _gameLauncher;

    private void Awake()
    {
        _gameLauncher = FindObjectsByType<GameLauncher>(FindObjectsSortMode.None)[0];

        _menuButton.onClick.AddListener(OnMenuButton);
        _startGameButton.onClick.AddListener(OnStartGameButton);
    }

    private void OnStartGameButton()
    {
        _gameLauncher.StartGame();
    }

    private void OnMenuButton()
    {
        _resultGameView.Close();

        _gameLauncher.StopGame();

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