using System;
using UnityEngine;

public class FlyBall
{
    private float _speed;

    private bool _startMove = false;
    private Vector3 _beginPos;

    private Vector3 _direction;
    private Transform _targetTransform;
    private float _coefficientSpeed;

    private GameContext _gameContext;

    public FlyBall(float speed)
    {
        _speed = speed;
        _gameContext = ServiceLocator.Scene.Get<GameContext>();
    }

    public void StartMove(Vector3 direction, Transform targetTransform, float coefficientSpeed)
    {
        _direction = direction;
        _targetTransform = targetTransform;
        _coefficientSpeed = coefficientSpeed;

        _startMove = true;
    }

    public void StopMove()
    {
        _targetTransform = null;
    }

    public void Update()
    {
        if (!_startMove)
        {
            return;
        }

        if (_targetTransform == null)
        {
            return;
        }

        var distance1 = Math.Abs(_targetTransform.position.x - _gameContext.GetRightBottomAngle().x);
        var distance2 = Math.Abs(_targetTransform.position.x - _gameContext.GetLeftBottomAngle().x);

        var angleRight = _gameContext.GetRightBottomAngle();
        var angleLeft = _gameContext.GetLeftBottomAngle();
        var n1 = angleRight - angleLeft;
        var n2 = angleLeft - angleRight;

        _direction.z = 0;
        var diff = 0.02f;
        // Debug.Log(
        //    $"Fly distance1={distance1} distance1 <= 0.02f ={distance1 <= 0.02f} distance2={distance2} distance2 <= 0.02f = {distance2 <= 0.02f}");
        if (distance1 <= diff)
        {
            n2 = n2.normalized;
            _direction = Vector3.Reflect(_targetTransform.position, -n2);
            //Debug.Log("xxx1");
        }

        if (distance2 <= diff)
        {
            n2 = n2.normalized;
            _direction = Vector3.Reflect(_targetTransform.position, n2);
            //Debug.Log("xxx2");
        }

        _targetTransform.position = Vector3.MoveTowards(_targetTransform.position,
            _beginPos + _direction.normalized * 1000, _coefficientSpeed * _speed * Time.deltaTime);

    }
}