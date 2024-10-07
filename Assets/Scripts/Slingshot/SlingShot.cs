using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        private const float BorderOffsetYMin = -0.3f;
        private const float BorderOffsetYMax = 0.3f;

        private float _borderXMin;
        private float _borderXMax;
        
        [SerializeField] private float _sensitivity = 40f;
        
        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _mouseReference;
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

            _borderYMin = transform.position.y + BorderOffsetYMin;
            _borderYMax = transform.position.y + BorderOffsetYMax;
            
            var field = game.GameContext.FieldRectTransform;
            var corners = new Vector3[4];
            field.GetWorldCorners(corners);
            _borderXMin = corners[0].x + 0.1f;
            _borderXMax = corners[3].x - 0.1f;
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
            _game.GameContext.SlingShotLines.UpdatePositions();
        }

        private void Awake()
        {
            ToggleActive(false);
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
                _isBeginTouch = true;
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

            _mouseReference = transform.position;

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
        }
        
        private void OnEndTouch()
        {
            _isRotating = false;
            TargetTransform.position = _initPosition;

            ResetDirection();
        }

        private bool IsTouchTargetObject(Vector3 position)
        {
            Vector2 mousePosition = _game.GameContext.Camera.ScreenToWorldPoint(position);

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
            
            _mouseOffset = (TargetTransform.position - _mouseReference);

            var rotation = Vector3.zero;
            rotation.z = (_mouseOffset.x) * _sensitivity;

            _game.GameContext.SlingShotLines.Traectory.Rotate(rotation);

            _mouseReference = TargetTransform.position;
        }

        private Transform TargetTransform => _targetSpriteRenderer.transform.parent.transform;
    }
}