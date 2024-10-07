using System;
using UnityEngine;

namespace Slingshot
{
    public class SlingShotLines : MonoBehaviour
    {
        [SerializeField] private Transform leftAnchor;
        [SerializeField] private Transform rightAnchor;
        [SerializeField] private Transform _traectory;
        [SerializeField] private LineRenderer[] lines;
        [SerializeField] private GameObject[] points;
        
        private Transform _holderTransform;
        
        public Transform Traectory => _traectory;

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
            
            SetPath(true);
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

        public void SetPath(bool b)
        {
            /*
            float yPos = (_traectory.up.y * 50) / points.Length;
            float val = yPos;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].SetActive(b);
                points[i].transform.parent = _traectory;
                points[i].transform.localPosition = new Vector3(0, val, -0.8f);
                val += yPos;
            }
            */
        }
    }
}