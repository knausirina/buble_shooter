using System;
using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _borderXMin = -1.4f;
        [SerializeField] private float _borderXMax = 1.4f;
        [SerializeField] private float _borderYMin = -3f;
        [SerializeField] private float _borderYMax = -2.36f;
        [SerializeField] private float _sensitivity = 40f;
        
        private SlingShotLines _slingShotLines;
        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private bool _isRotating;
        
        private void Start()
        {
            _initPosition = transform.position;
        }

        private void Awake()
        {
            _slingShotLines = FindObjectOfType<SlingShotLines>();
        }

        private void Update()
        {
            SetDirection();
        }
        
        private void OnMouseDown()
        {
            _isRotating = true;

            _mouseReference = transform.position;

            _offset = _initPosition - _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));

            _slingShotLines.Traectory.eulerAngles = new Vector3 (0,0,0);
            _slingShotLines.SetPath(true);
        }

        private void OnMouseDrag()
        {
            var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            var curPosition = _camera.ScreenToWorldPoint(curScreenPoint) + _offset;
            transform.position = curPosition;

            var posX = Mathf.Clamp (transform.position.x, _borderXMin, _borderXMax);
            var posY = Mathf.Clamp (transform.position.y, _borderYMin, _borderYMax);

            transform.position = new Vector3 (posX, posY, curPosition.z);
        }
        private void OnMouseUp()
        {
            _isRotating = false;
            transform.position = _initPosition;

            ResetDirection();
        }

        private void ResetDirection()
        {
            _slingShotLines.Traectory.eulerAngles = new Vector3 (0,0,0);
            _slingShotLines.SetPath(false);
        }

        private void SetDirection()
        {
            if (!_isRotating)
            {
                return;
            }
            
            _mouseOffset = (transform.position - _mouseReference);

            var rotation = Vector3.zero;
            rotation.z = (_mouseOffset.x) * _sensitivity;

            _slingShotLines.Traectory.Rotate(rotation);

            _mouseReference = transform.position;
        }
    }
}