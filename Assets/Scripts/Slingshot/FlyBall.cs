using System;
using UnityEngine;

namespace Slingshot
{
    public class FlyBall
    {
        private float Speed = 5;
        
        private bool _startMove = false;
        private Vector3 _beginPos;

        private GameContext _gameContext;
        private Vector3 _direction;
        private Transform _targetTransform;
        private float _coefficientSpeed;
        
        public void Construct(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void StartMove(Vector3 direction, Transform targetTransform, float coefficientSpeed)
        {
            _direction = direction;
            _targetTransform = targetTransform;
            _coefficientSpeed = coefficientSpeed;

            _startMove = true;
        }

        public void Update()
        {
            if (!_startMove)
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
            if (distance1 <= 0.01f || distance2 <= 0.01f)
            {
                if (distance1 <= 0.01f)
                {
                    n2 = n2.normalized;
                    _direction = Vector3.Reflect(_targetTransform.position, -n2);
                    Debug.Log("xxx1");
                }

                if (distance2 <= 0.01f)
                {
                    n2 = n2.normalized;
                    _direction = Vector3.Reflect(_targetTransform.position, n2);
                    Debug.Log("xxx2");
                }
            }

            _targetTransform.position = Vector3.MoveTowards(_targetTransform.position,
                _beginPos +  _direction.normalized * 1000 , _coefficientSpeed * Speed * Time.deltaTime);
            
        }
    }
}