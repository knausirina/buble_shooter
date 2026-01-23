using System;
using UnityEngine;
using Views;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SlingShot : MonoBehaviour
{
    public Action BallFinishedAction;

    private const float BorderOffsetYMin = -1f;
    private const float Radius = 1f;
    private const float RandomAngle = 10f;

    [SerializeField] private Transform _centerLineTransform;
    [SerializeField] private Transform _leftLineTransform;
    [SerializeField] private Transform _rightLineTransform;
    [SerializeField] private Transform _resultTransform;
    [SerializeField] private Transform _releasePointTransform;
    [SerializeField] private SlingShotLines _slingShotLines;

    private Vector3 _offset;
    private Vector3 _initPosition;
    private Vector3 _touchBegin;
    private float _borderYMin;
    private float _borderYMax;
    private bool _isBeginTouch;

    private FlyBall _flyBall;
    private BubblesContactSystem _bubblesContactSystem;
    private BubbleView _bubbleView;
    private Vector3 _direction;
    private bool _isWaitMoveBall = true;

    private GameContext _gameContext;
    private IGameState _gameState;

    private SpriteRenderer TargetSpriteRenderer => _bubbleView.Renderer;
    private Transform TargetTransform => _bubbleView.transform;

    private void Awake()
    {
        _gameContext = ServiceLocator.Scene.Get<GameContext>();

        ToggleShootElements(false);
    }

    public void Construct(BubblesContactSystem bubblesContactSystem, IGameState gameState, Config config)
    {
        _bubblesContactSystem = bubblesContactSystem;
        _gameState = gameState;

        _flyBall ??= new FlyBall(config.BallSpeed);

        gameObject.SetActive(false);
    }

    public void SetHolder(BubbleView bubbleView) 
    {
        _bubbleView = bubbleView;

        _borderYMin = TargetTransform.position.y + BorderOffsetYMin;
        _borderYMax = TargetTransform.position.y;

        _centerLineTransform.transform.position = TargetTransform.position;

        _initPosition = TargetTransform.position;

        _slingShotLines.SetHolder(bubbleView);
        _slingShotLines.UpdatePositions();
        _slingShotLines.ToggleActive(true);

        gameObject.SetActive(true);

        _isWaitMoveBall = true;
    }

    public void DisableShoot()
    {
        _flyBall.StopMove();
        _isWaitMoveBall = false;
        _bubblesContactSystem.SetTarget(null);
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
        if (_gameState.State != GameState.Play)
        {
            return;
        }

        ControlBall();

        if (_isWaitMoveBall)
        {
            _flyBall.Update();
            _bubblesContactSystem.CheckContact(false);
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

        _touchBegin = _gameContext.Camera.ScreenToWorldPoint(position);
        _isBeginTouch = true;

        _offset = _initPosition - _touchBegin;

        ToggleShootElements(true);
    }

    private void OnMove()
    {
        var position = GetTouchPosition();
        var screenPoint = new Vector3(position.x, position.y, _gameContext.Camera.nearClipPlane);
        var currentPosition = _gameContext.Camera.ScreenToWorldPoint(screenPoint) + _offset;

        var direction = currentPosition - _initPosition;
        var coordinates = new Vector2(currentPosition.x, currentPosition.y);

        if (direction.sqrMagnitude > Radius)
        {
            var directionWithLenghtRadius = direction.normalized * Radius;
            var pos = _initPosition + directionWithLenghtRadius;
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

    private Vector3 GetDirection()
    {
        var positionSlightShotLines = _slingShotLines.gameObject.transform.position;
        positionSlightShotLines.z = 0;
        var pullDirection = _releasePointTransform.position - (TargetTransform.position - positionSlightShotLines).normalized;
        return (pullDirection - positionSlightShotLines).normalized;
    }

    private void DrawLine()
    {
        if (!_isBeginTouch)
        {
            return;
        }

        var lookDirection = GetDirection();
        var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        lookAngle -= 90;
        _centerLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        var distanceFromCenter = GetDragForce();
        _leftLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - RandomAngle * distanceFromCenter);
        _rightLineTransform.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle + RandomAngle * distanceFromCenter);
    }

    private void OnEndTouch()
    {
        ToggleShootElements(false);

        _bubblesContactSystem.SetTarget(_bubbleView);

        _direction = GetDirection();

        var distanceFromCenter = GetDragForce();
        var angleMin = -RandomAngle * distanceFromCenter;
        var angleMax = +RandomAngle * distanceFromCenter;
        var randomAngle = Utils.RandomFloatBetween(angleMin, angleMax);
        var dir = Quaternion.Euler(0, 0, randomAngle) * _direction;

        _flyBall.StartMove(dir, TargetTransform, GetDragForce());
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
        var mousePosition = _gameContext.Camera.ScreenToWorldPoint(position);
        mousePosition.z = _gameContext.Camera.nearClipPlane;
        return TargetSpriteRenderer.bounds.Contains(mousePosition);
    }
}