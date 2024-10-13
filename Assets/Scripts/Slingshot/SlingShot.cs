using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        private const float BorderOffsetYMin = -1f;
        private const float Radius = 1f;

        [SerializeField] private GameObject _line;
        [SerializeField] private Transform _pointTransform;
        [SerializeField] private Transform AimerTransform;
        [SerializeField] private Transform ReleasePointTransform;

        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _touchBegin;
        private Vector3 _mouseOffset;
        private float _borderYMin;
        private float _borderYMax;
        private bool _isBeginTouch;

        private SpriteRenderer _targetSpriteRenderer;
        private Game _game;
        
        private Transform TargetTransform => _targetSpriteRenderer.transform.parent.transform;

        public void Construct(Game game)
        {
            _game = game;
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

            _touchBegin = _game.GameContext.Camera.ScreenToWorldPoint(position);
            _isBeginTouch = true;

            _offset = _initPosition - _touchBegin;
        }

        private void OnMove()
        {
            var position = GetTouchPosition();
            var screenPoint = new Vector3(position.x, position.y, _game.GameContext.Camera.nearClipPlane);
            var currentPosition = _game.GameContext.Camera.ScreenToWorldPoint(screenPoint) + _offset;
            
            var centerCircle = new Vector3(_initPosition.x, _initPosition.y -0.36f, _initPosition.z);
            var direction = currentPosition - centerCircle;

            var coordinates = new Vector2(currentPosition.x, currentPosition.y);
            
            if (direction.sqrMagnitude > Radius)
            {
                var directionWithLenghtRadius = direction.normalized * Radius;
                var pos = centerCircle +  directionWithLenghtRadius;
                TargetTransform.position = new Vector3(pos.x, pos.y, currentPosition.z);
            }
            else
            {
                TargetTransform.position = new Vector3(coordinates.x, coordinates.y, currentPosition.z);
            }
            
            var posY2 = Mathf.Clamp(TargetTransform.position.y, _borderYMin, _borderYMax);
            TargetTransform.position = new Vector3(TargetTransform.position.x, posY2, TargetTransform.position.z);
            
            _line.transform.position = TargetTransform.position;
        }
        
        private void OnEndTouch()
        {
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