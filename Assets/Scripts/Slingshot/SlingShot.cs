using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        private const float BorderOffsetYMin = -0.3f;
        private const float BorderOffsetYMax = 0.3f;

        [SerializeField] private GameObject _line;

        private float _borderXMin;
        private float _borderXMax;
        
        [SerializeField] private float _sensitivity = 40f;
        
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
        
        public void Construct(Game game)
        {
            _game = game;
            
            _borderXMin = -0.6f;
            _borderXMax = 0.6f;
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

            _borderYMin = TargetTransform.position.y;
            _borderYMax = TargetTransform.position.y;// + BorderOffsetYMax;
            
            _game.GameContext.SlingShotLines.UpdatePositions();

            _line.transform.position = TargetTransform.position;
        }

        private void Awake()
        {
            ToggleActive(false);
        }

        private void DrawLine()
        {
            var lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _touchBegin;
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg; 
            
            lookAngle = Mathf.Clamp(lookAngle, -158, -23);
           
            if (Input.GetMouseButton(0))
            {
              _line.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle + 90);

               _line.SetActive(true);
            }
           else
           {
           }
/*
           if (canShoot
               && Input.GetMouseButtonUp(0)
               && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > bottomShootPoint.transform.position.y)
               && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < limit.transform.position.y))
                    {
                        canShoot = false;
                        Shoot();
                    }
                }
            }
            */
        }

        private void Update()
        {
            if (_game == null)
            {
                return;
            }

            if (_game.GameState != GameState.Play)
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
            if (_isBeginTouch)
            {
                DrawLine();
            }
            
            SetDirection();
        }

        private Vector2 GetTouchPosition()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.GetTouch(0).position;
#endif
        }

        void OnBeginTouch()
        {
            var position = GetTouchPosition();
            if (!IsTouchTargetObject(position))
            {
                return;
            }

            _isRotating = true;

            _touchBegin = TargetTransform.position;
            _isBeginTouch = true;

            _offset = _initPosition - _game.GameContext.Camera.ScreenToWorldPoint(position);

            _game.GameContext.SlingShotLines.Traectory.eulerAngles = Vector3.zero;
            _game.GameContext.SlingShotLines.SetPath(true);
        }

        private void OnMove()
        {
            var position = GetTouchPosition();
            var curScreenPoint = new Vector3(position.x, position.y, _game.GameContext.Camera.nearClipPlane);
            var curPosition = _game.GameContext.Camera.ScreenToWorldPoint(curScreenPoint) + _offset;
            
            TargetTransform.position = curPosition;
            var posX = Mathf.Clamp(TargetTransform.position.x, _borderXMin, _borderXMax);
            var posY = Mathf.Clamp(TargetTransform.position.y, _borderYMin, _borderYMax);
            TargetTransform.position = new Vector3 (posX, posY, curPosition.z);
            
            _line.transform.position = TargetTransform.position;
        }
        
        private void OnEndTouch()
        {
            _isRotating = false; 
            _line.SetActive(false);
            TargetTransform.position = _initPosition;

            ResetDirection();
        }

        private bool IsTouchTargetObject(Vector3 position)
        {
            position.z = _game.GameContext.Camera.nearClipPlane;
            var mousePosition = _game.GameContext.Camera.ScreenToWorldPoint(position);
            return _targetSpriteRenderer.bounds.Contains(mousePosition);
        }

        private void ResetDirection()
        {
            _game.GameContext.SlingShotLines.Traectory.eulerAngles = Vector3.zero;
            _game.GameContext.SlingShotLines.SetPath(false);
        }

        private void SetDirection()
        {
            if (!_isRotating)
            {
                return;
            }
            
            _mouseOffset = (TargetTransform.position - _touchBegin);

            var rotation = Vector3.zero;
            rotation.z = (_mouseOffset.x) * _sensitivity;

            _game.GameContext.SlingShotLines.Traectory.Rotate(rotation);

            _touchBegin = TargetTransform.position;
        }

        private Transform TargetTransform => _targetSpriteRenderer.transform.parent.transform;
    }
}