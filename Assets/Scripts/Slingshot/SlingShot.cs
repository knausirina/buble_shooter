using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        private const float BORDER_OFFSET_Y_MIN = -0.3f;
        private const float BORDER_OFFSET_Y_MAX = 0.3f;
        
        [SerializeField] private float _borderXMin = -1.4f;
        [SerializeField] private float _borderXMax = 1.4f;
        
        [SerializeField] private float _sensitivity = 40f;
        
        private SlingShotLines _slingShotLines;
        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private bool _isRotating;
        private float _borderYMin;
        private float _borderYMax;
        private bool _isBeginTouch = false;

        private SpriteRenderer _spriteRenderer;
        private Game _game;
        
        public void Construct(Game game)
        {
            _game = game;
            _game.GameStateChanged += OnGameStateChanged;

            _borderYMin = transform.position.y + BORDER_OFFSET_Y_MIN;
            _borderYMax = transform.position.y + BORDER_OFFSET_Y_MAX;

            _slingShotLines.UpdatePositions();
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _slingShotLines = FindObjectOfType<SlingShotLines>();
        }

        private void OnDestroy()
        {
            _game.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged()
        {
            switch (_game.GameState)
            {
                case GameState.Play:
                    _initPosition = transform.position;
                    break;
                case GameState.Stop:
                    break;
            }
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
                    OnMove(Input.touches[0].position);
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

            _slingShotLines.Traectory.eulerAngles = Vector3.zero;
            _slingShotLines.SetPath(true);
        }

        private void OnMove()
        {
            var position = GetTouchPosition();
            var curScreenPoint = new Vector3(position.x, position.y, _game.GameContext.Camera.nearClipPlane);
            var curPosition = _game.GameContext.Camera.ScreenToWorldPoint(curScreenPoint) + _offset;
            transform.position = curPosition;

            var posX = Mathf.Clamp(transform.position.x, _borderXMin, _borderXMax);
            var posY = Mathf.Clamp(transform.position.y, _borderYMin, _borderYMax);
            transform.position = new Vector3 (posX, posY, curPosition.z);
        }
        
        private void OnEndTouch()
        {
            _isRotating = false;
            transform.position = _initPosition;

            ResetDirection();
        }

        private bool IsTouchTargetObject(Vector3 position)
        {
            Vector2 mousePosition = _game.GameContext.Camera.ScreenToWorldPoint(position);

            return _spriteRenderer.bounds.Contains(mousePosition);
        }

        private void ResetDirection()
        {
            _slingShotLines.Traectory.eulerAngles = Vector3.zero;
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