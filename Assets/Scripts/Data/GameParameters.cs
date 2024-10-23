using System.Collections;
using UnityEngine;

namespace Data
{
    public class GameParameters
    {
        private float _ballSize;
        private Vector2Int _fieldSizeInPixels;
        private Vector2Int _fieldSizeInElements;
        private int _maxCountBubbles;

        public float BallSize => _ballSize;
        public Vector2Int FieldSizeInPixels => _fieldSizeInPixels;
        public Vector2Int FieldSizeInElements => _fieldSizeInElements;
        public int MaxCountBubbles => _maxCountBubbles;

        public GameParameters(float ballSize, Vector2Int fieldSizeInPixels, Vector2Int fieldSizeInElements, int maxCountBubbles)
        {
            _ballSize = ballSize;
            _fieldSizeInPixels = fieldSizeInPixels;
            _fieldSizeInElements = fieldSizeInElements;
            _maxCountBubbles = maxCountBubbles;
        }
    }
}