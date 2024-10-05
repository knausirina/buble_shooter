using UnityEngine;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
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

        private Game _game;
        
        public void Construct(Game game)
        {
            _game = game;
            _game.GameStateChanged += OnGameStateChanged;
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

        private void Awake()
        {
            _slingShotLines = FindObjectOfType<SlingShotLines>();
        }

        private bool _inMouseDown = false;
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
                OnMouseDown1();
                _inMouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp2();
                _inMouseDown = false;
            }

            if (_inMouseDown)
            {
                OnMouseDrag2(Input.mousePosition);
            }
#else

            if (Input.touchCount > 0)
            {
                Debug.Log("xxx Input.touchCount " + Input.touchCount);
                
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    OnMouseDown1();
                }
                else if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    OnMouseDrag2(Input.touches[0].position);
                }
                else if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    OnMouseUp2();
                }
            }

            
#endif
            
            SetDirection();
        }
        
        private void OnMouseDown1()
        {
            Debug.Log("xxx OnMouseDown1");
            _isRotating = true;

            _mouseReference = transform.position;

            _offset = _initPosition - _game.GameContext.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));

            _slingShotLines.Traectory.eulerAngles = new Vector3 (0,0,0);
            _slingShotLines.SetPath(true);
        }

        private void OnMouseDrag2(Vector3 position)
        {
            Debug.Log("xxx OnMouseDrag2");
            var curScreenPoint = position;//new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            var curPosition = _game.GameContext.Camera.ScreenToWorldPoint(curScreenPoint) + _offset;
            transform.position = curPosition;

            var posX = Mathf.Clamp (transform.position.x, _borderXMin, _borderXMax);
            var posY = Mathf.Clamp (transform.position.y, _borderYMin, _borderYMax);

            transform.position = new Vector3 (posX, posY, curPosition.z);
        }
        private void OnMouseUp2()
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