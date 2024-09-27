using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Views.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button _gameButton;
        [SerializeField] private Button _aboutButton;
        [SerializeField] private Button _exitButton;
    
        private void Awake()
        {
            _gameButton.onClick.AddListener(OnGameButton);
        }

        private void OnGameButton()
        {
            OnGameButtonAsync().Forget(Debug.LogError);
        }

        private async UniTask OnGameButtonAsync()
        {
            LoaderView.Instance.ToggleShow(true);
        
            await SceneManager.LoadSceneAsync(Scenes.GameScene);
       
            LoaderView.Instance.ToggleShow(false);
        }

        private void OnDestroy()
        {
            _gameButton.onClick.RemoveListener(OnGameButton);
        }
    }
}
