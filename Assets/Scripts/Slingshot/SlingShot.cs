using System;
using GamePlay;
using UnityEngine;
using Views;

namespace Slingshot
{
    public class SlingShot : MonoBehaviour
    {
        public Action BallFinishedAction;
        
        private const float BorderOffsetYMin = -1f;
        private const float Radius = 1f;
        private const float RandromAngle = 10f;

        [SerializeField] private Transform _centerLineTransform;
        [SerializeField] private Transform _leftLineTransform;
        [SerializeField] private Transform _rightLineTransform;
        [SerializeField] private Transform _resultTransform;
        [SerializeField] private Transform _releasePointTransform;
        [SerializeField] private SlingShotLines _slingShotLines;

        private Vector3 _offset;
        private Vector3 _initPosition;
        private Vector3 _touchBegin;
        private Vector3 _mouseOffset;
        private float _borderYMin;
        private float _borderYMax;
        private bool _isBeginTouch;
        
        private Game _game;
        private FlyBall _flyBall;
        private BubblesContact _bubblesContact;
        private BubbleView _bubbleView;
        private Vector3 _direction;
        private bool _isAllowControlBall = true;
        private bool _isWaitMoveBall = true;
        
        private Camera Camera => _game.GameContext.Camera;
        private SpriteRenderer TargetSpriteRenderer => _bubbleView.Renderer;
        private Transform TargetTransform => _bubbleView.transform;

        private void Awake()
        {
            ToggleShootElements(false);
        }

        public void Construct(Game game, BubblesContact bubblesContact)
        {
            _game = game;
            
            _flyBall ??= new FlyBall();
            _flyBall.SetContext(game.GameContext);

            _bubblesContact = bubblesContact;
        }

        public void ToggleActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            if (isActive)
            {
                _initPosition = TargetTransform.position;
            }
        }

        public void SetHolder(BubbleView bubbleView)
        {
            _bubbleView = bubbleView;
            _slingShotLines.SetHolder(bubbleView);

            _borderYMin = TargetTransform.position.y + BorderOffsetYMin;
            _borderYMax = TargetTransform.position.y;
            
            _game.GameContext.SlingShotLines.UpdatePositions();

            _centerLineTransform.transform.position = TargetTransform.position;
        }
        
        private void DrawLine()
        {
            if (!_isBeginTouch)
            {
                return;
            }
            
            var lookDirection = GetDirection();
            var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            var angle = lookAngle - 90;
            _centerLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            
            var distanceFromCenter = (_initPosition - TargetTransform.position).sqrMagnitude;
            _leftLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, angle - RandromAngle * distanceFromCenter);
            _rightLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, angle + RandromAngle * distanceFromCenter);
        }
        
        private Vector3 GetDirection()
        {
            var positionBall = _slingShotLines.gameObject.transform.position;
            var pullDirection = _releasePointTransform.position - (TargetTransform.position - positionBall).normalized;
            return (pullDirection- positionBall).normalized;
        }

        public void ToggleAllowControlBall(bool isAllowControlBall)
        {
            _isAllowControlBall = isAllowControlBall;
        }

        public void DisableShoot()
        {
            _flyBall.StopMove();
            _isWaitMoveBall = false;
            _bubblesContact.SetTarget(null);
        }

        public void AllowShoot()
        {
            _isWaitMoveBall = true;
        }

        private void ControlBall()
        {
            if (_isWaitMoveBall)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    OnBeginTouch();
                }

                if (Input.GetMouseButtonUp(0) && _isBeginTouch)
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
        }

        private void Update()
        {
            if (_game == null || _game.GameState != GameState.Play)
            {
                return;
            }

            ControlBall();
            
            if (_isWaitMoveBall)
            {
                _flyBall.Update();
                _bubblesContact.CheckContact(false);
            }
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
            
            ToggleShootElements(true);
        }

        private void OnMove()
        {
            var position = GetTouchPosition();
            var screenPoint = new Vector3(position.x, position.y, Camera.nearClipPlane);
            var currentPosition = Camera.ScreenToWorldPoint(screenPoint) + _offset;
            
            var direction = currentPosition - _initPosition;
            var coordinates = new Vector2(currentPosition.x, currentPosition.y);
            
            if (direction.sqrMagnitude > Radius)
            {
                var directionWithLenghtRadius = direction.normalized * Radius;
                var pos = _initPosition +  directionWithLenghtRadius;
                TargetTransform.position = new Vector3(pos.x, pos.y, currentPosition.z);
            }
            else
            {
                TargetTransform.position = new Vector3(coordinates.x, coordinates.y, currentPosition.z);
            }
            
            var clampYPosition = Mathf.Clamp(TargetTransform.position.y, _borderYMin, _borderYMax);
            TargetTransform.position = new Vector3(TargetTransform.position.x, clampYPosition, TargetTransform.position.z);
            
            _centerLineTransform.position = TargetTransform.position;
            _leftLineTransform.position = TargetTransform.position;
            _rightLineTransform.position = TargetTransform.position;
        }

        private void OnEndTouch()
        {
            _isAllowControlBall = false;
            
            ToggleShootElements(false);

            _direction = GetDirection();

            _bubblesContact.SetTarget(_bubbleView);

            _flyBall.StartMove(_direction, TargetTransform, GetDragForce());
        }

        private float GetDragForce()
        {
            var diff = (_initPosition - TargetTransform.position).sqrMagnitude;
            return diff / Radius;
        }

        private void ToggleShootElements(bool isShow)
        {
            _centerLineTransform.gameObject.SetActive(isShow);
            _leftLineTransform.gameObject.SetActive(isShow);
            _rightLineTransform.gameObject.SetActive(isShow);
            _slingShotLines.ToggleActive(isShow);
        }

        private bool IsTouchTargetObject(Vector3 position)
        {
            var mousePosition = Camera.ScreenToWorldPoint(position);
            mousePosition.z = Camera.nearClipPlane;
            return TargetSpriteRenderer.bounds.Contains(mousePosition);
        }
    }
}