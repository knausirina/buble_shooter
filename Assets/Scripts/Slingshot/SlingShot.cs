using System;
using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        private const float BorderOffsetYMin = -1f;

        private float _borderXMin;
        private float _borderXMax;

        [SerializeField] private GameObject _line;
        [SerializeField] private float _sensitivity = 40f;
        [SerializeField] private Transform _pointTransform;
        [SerializeField] private Transform AimerTransform;
        [SerializeField] private Transform ReleasePointTransform;

        [SerializeField] private GameObject _prefab;

        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _touchBegin;
        private Vector3 _mouseOffset;
        private bool _isRotating;
        private float _borderYMin;
        private float _borderYMax;
        private bool _isBeginTouch = false;

        private SpriteRenderer _targetSpriteRenderer;
        private Game _game;
        
        private Transform TargetTransform => _targetSpriteRenderer.transform.parent.transform;

        public void Construct(Game game)
        {
            _game = game;

            _borderXMin = -1.5f;
            _borderXMax = 0.5f;
        }

        public void ToggleActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            if (isActive)
            {
                _initPosition = TargetTransform.position;
            }
        }

        public void SetTargetSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            _targetSpriteRenderer = spriteRenderer;

            _borderYMin = TargetTransform.position.y + BorderOffsetYMin;
            _borderYMax = TargetTransform.position.y;

            _game.GameContext.SlingShotLines.UpdatePositions();

            _line.transform.position = TargetTransform.position;
        }

        private void Awake()
        {
            ToggleActive(false);
        }
        
        private void DrawLine()
        {
            if (!_isBeginTouch)
            {
                return;
            }

            UpdateAim();
            var lookDirection = GetShotDirection();
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            
            _line.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90);
            _line.SetActive(true);
        }
        
        private void UpdateAim()
        {
            var pullDirection = ReleasePointTransform.position - (TargetTransform.position - _pointTransform.position).normalized;
            AimerTransform.position = pullDirection;
        }

        private Vector3 GetShotDirection()
        {
            return (AimerTransform.position - _pointTransform.position).normalized;
        }

        private void Update()
        {
            if (_game == null || _game.GameState != GameState.Play)
            {
                return;
            }
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                OnBeginTouch();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnEndTouch();
               _isBeginTouch = false;
            }

            if (_isBeginTouch)
            {
                OnMove();
            }
#else

            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    OnBeginTouch();
                }
                else if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    OnMove();
                }
                else if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    OnEndTouch();
                }
            }

            
#endif
            DrawLine();
        }

        private Vector2 GetTouchPosition()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.GetTouch(0).position;
#endif
        }

        private void OnBeginTouch()
        {
            var position = GetTouchPosition();
            if (!IsTouchTargetObject(position))
            {
                return;
            }

            _isRotating = true;

            var pos2 = (Vector3)position;
          //  pos2.z = _game.GameContext.Camera.nearClipPlane;
            _touchBegin = _game.GameContext.Camera.ScreenToWorldPoint(pos2);
            _isBeginTouch = true;

            _offset = _initPosition - _game.GameContext.Camera.ScreenToWorldPoint(position);
        }

        private GameObject _centerGO;

        private void OnMove()
        {
            var position = GetTouchPosition();
            var curScreenPoint = new Vector3(position.x, position.y, _game.GameContext.Camera.nearClipPlane);
            var curPosition = _game.GameContext.Camera.ScreenToWorldPoint(curScreenPoint) + _offset;
            
            //TargetTransform.position = curPosition;
            
            
            Debug.Log("xxx TargetTransform.position=" + TargetTransform.position + " _initPosition =" + _initPosition);

            var r = 1f;
            
            var centerCircle = _initPosition;
            centerCircle.y = -0.36f;

            /*
            if (_centerGO == null)
            {
                _centerGO = Instantiate(_prefab);
                _centerGO.transform.localScale = Vector3.one * 0.2f;
                
                var _centerGO2 = Instantiate(_prefab);
                _centerGO2.name = "_centerGO2";
                _centerGO2.transform.localScale = Vector3.one * 0.2f;
                _centerGO2.transform.position = new Vector3(centerCircle.x, centerCircle.y + r, centerCircle.z);
                
                var _centerGO3 = Instantiate(_prefab);
                _centerGO3.name = "_centerGO3";
                _centerGO3.transform.localScale = Vector3.one * 0.2f;
                _centerGO3.transform.position = new Vector3(centerCircle.x, centerCircle.y - r, centerCircle.z);
                
                var _centerGO4 = Instantiate(_prefab);
                _centerGO4.name = "_centerGO4";
                _centerGO4.transform.localScale = Vector3.one * 0.2f;
                _centerGO4.transform.position = new Vector3(centerCircle.x + r, centerCircle.y, centerCircle.z);
                
                var _centerGO5 = Instantiate(_prefab);
                _centerGO5.name = "_centerGO5";
                _centerGO5.transform.localScale = Vector3.one * 0.2f;
                _centerGO5.transform.position = new Vector3(centerCircle.x - r, centerCircle.y, centerCircle.z);
            }
            _centerGO.transform.position = centerCircle;
*/
            var v = curPosition - centerCircle;
            var vNormalized = v.normalized * r;
            var pos = centerCircle +  vNormalized;
            
            var center = centerCircle;
            Debug.Log("xxx _initPosition2 = " + centerCircle);
            
            var point = TargetTransform.position;
            var c = Math.Sqrt(Math.Pow(Math.Abs(center.x - point.x), 2) + Math.Pow(Math.Abs(center.y - point.y), 2));
            Debug.Log("xxx c = " + c + " r = " + r);

            Vector2 coordinates;
           // if (c > r)                 
            {
          //      coordinates.x = (float)(TargetTransform.position.x * r / c);
          //      coordinates.y = (float)(TargetTransform.position.y * r / c);
            }
          //  else
            {
                coordinates.x = curPosition.x;
                coordinates.y = curPosition.y;
            }

            var d = -0.342f + 0.8425927f;
            
            var posX = Mathf.Clamp(TargetTransform.position.x, _borderXMin, _borderXMax);
            var posY = Mathf.Clamp(TargetTransform.position.y, _borderYMin, _borderYMax);
           // TargetTransform.position = new Vector3 (posX, posY, curPosition.z);

           Debug.Log("xxx coordinates " + coordinates);
            //TargetTransform.position = new Vector3(coordinates.x, coordinates.y, curPosition.z);
            Debug.Log("xxx vNormalized.magnitude   " + vNormalized.magnitude );
            if (v.sqrMagnitude > r)
            {
                TargetTransform.position = new Vector3(pos.x, pos.y, curPosition.z);
            }
            else
            {
                TargetTransform.position = new Vector3(coordinates.x, coordinates.y, curPosition.z);
            }
            
            var posY2 = Mathf.Clamp(TargetTransform.position.y, _borderYMin, _borderYMax);
            TargetTransform.position = new Vector3(TargetTransform.position.x, posY2, TargetTransform.position.z);
            
            _line.transform.position = TargetTransform.position;
        }
        
        private void OnEndTouch()
        {
            _isRotating = false; 
           _line.SetActive(false);
            TargetTransform.position = _initPosition;
        }

        private bool IsTouchTargetObject(Vector3 position)
        {
            position.z = _game.GameContext.Camera.nearClipPlane;
            var mousePosition = _game.GameContext.Camera.ScreenToWorldPoint(position);
            return _targetSpriteRenderer.bounds.Contains(mousePosition);
        }
    }
}