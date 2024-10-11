using UnityEngine;

namespace Slingshot
{
    public class SlingShotLines : MonoBehaviour
    {
        [SerializeField] private Transform leftAnchor;
        [SerializeField] private Transform rightAnchor;
        [SerializeField] private LineRenderer[] lines;
        [SerializeField] private GameObject[] points;
        
        private Transform _holderTransform;

        public void UpdatePositions()
        {
            UpdateBeginPositions();
        }
        
        public void ToggleActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetHolder(Transform holderTransform)
        {
            _holderTransform = holderTransform;
        }

        private void Awake()
        {
            ToggleActive(false);
        }

        private void Start()
        {
            UpdateBeginPositions();
        }

        private void UpdateBeginPositions()
        {
            lines[0].SetPosition(0, leftAnchor.position);
            lines[1].SetPosition(0, rightAnchor.position);
        }

        private void Update()
        {
            if (_holderTransform == null)
            {
                return;
            }

            foreach (var t in lines)
            {
                t.SetPosition(1, _holderTransform.position);
            }
        }
    }
}