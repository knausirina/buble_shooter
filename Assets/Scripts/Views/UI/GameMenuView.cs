using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Views.UI
{
   public class GameMenu : MonoBehaviour
   {
      [SerializeField] private Button _menuButton;
      [SerializeField] private Button _startGameButton;

      private GameLauncher _gameLauncher;

      private void Awake()
      {
         _gameLauncher = FindObjectOfType<GameLauncher>();
         
         _menuButton.onClick.AddListener(OnMenuButton);
               _startGameButton.onClick.AddListener(OnStartGameButton);
      }

      private void OnStartGameButton()
      {
         _gameLauncher.StartGame();
      }

      private void OnMenuButton()
      {
         OnMenuButtonAsync().Forget(Debug.LogError);
      }

      private async UniTask OnMenuButtonAsync()
      {
         LoaderView.Instance.ToggleShow(true);
        
         await SceneManager.LoadSceneAsync(Scenes.MenuScene);
       
         LoaderView.Instance.ToggleShow(false);
      }

      private void OnDestroy()
      {
         _menuButton.onClick.RemoveListener(OnMenuButton);
      }
   }
}
