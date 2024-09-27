using UnityEngine;

namespace Views.UI
{
    public class LoaderView : MonoBehaviour
    {
        private static LoaderView _loaderView;

        public static LoaderView Instance => _loaderView;

        private void Awake()
        {
            ToggleShow(false);

            if (_loaderView == null)
            {
                _loaderView = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ToggleShow(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}
