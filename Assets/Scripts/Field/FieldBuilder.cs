using Data;
using UnityEngine;
using Views;

namespace Field
{
    public class FieldBuilder
    {
        private readonly Config _config;
        private readonly PoolBalls _poolBalls;
        private GameContext _gameContext;
        private BubbleView[,] _bubblesViews;
        
        private readonly Vector2Int _fieldSizeInPixels;
        private readonly Vector2Int _fieldSizeInElements;
        
        private float _ballSize;
        
        public float BallSize => _ballSize;
        public BubbleView[,] BubblesViews => _bubblesViews;
        public FieldBuilder(Config config, PoolBalls poolBalls)
        {
            _config = config;
            _poolBalls = poolBalls;
        }

        public BubbleView[,] Build(GameContext gameContext, BubblesData bubblesData, Vector2Int fieldSizeInElements)
        {
            _gameContext = gameContext;
            
            var field = gameContext.FieldRectTransform;
            var corners = new Vector3[4];
            field.GetWorldCorners(corners);
            
            var fieldInWorldDimentionsWidth = corners[3].x - corners[0].x;
            _ballSize = (fieldInWorldDimentionsWidth / fieldSizeInElements.y);
            
            var offset = _ballSize / 10;
            _ballSize -= offset;

            var countRows = (int)(fieldInWorldDimentionsWidth / _ballSize);
            
            gameContext.BubblesViewRoot.position = new Vector3(corners[1].x, corners[1].y, 0);

            _bubblesViews = new BubbleView[countRows, bubblesData.ColumnCount];
            for (var i = 0; i < bubblesData.RowsCount; i++)
            {
                for (var j = 0; j < bubblesData.ColumnCount; j++)
                {
                    var bubbleData = bubblesData.Get(i, j);
                    if (bubbleData == null)
                    {
                        continue;
                    }

                    var bubble = _poolBalls.Pool.Get();
                    bubble.gameObject.name = $" i={i} j={j}";
                    bubble.Renderer.color = _config.GetColorByEnum(bubbleData.Color);
                    AddBubble(bubble, i, j);
                    
                    _bubblesViews[i, j] = bubble;
                }
            }

            return _bubblesViews;
        }

        public void Clear()
        {
            _bubblesViews = null;
        }

        public void RemoveBubble(BubbleView bubbleView,  int row, int column)
        {
            bubbleView.transform.parent = null;
            _poolBalls.Pool.Release(bubbleView);
        }

        public void AddBubble(BubbleView bubbleView, int row, int column)
        {
            bubbleView.gameObject.transform.parent = _gameContext.BubblesViewRoot;
            bubbleView.transform.localPosition = GetPosition(row, column);
            bubbleView.Renderer.gameObject.transform.localScale = new Vector3(_ballSize, _ballSize, 1);
            bubbleView.gameObject.name = $" i={row} j={column}";

            _bubblesViews[row, column] = bubbleView;
        }

        public Vector2 GetPosition(int row, int column)
        {
            var offset = _ballSize / 10;
            var remainder = row - row / 2 * 2;
            return new Vector2(column * (_ballSize + offset) + _ballSize / 2 +  3 * remainder * offset,
                -row * (_ballSize + offset) - _ballSize / 2 );
        }

        public bool IsHasPosition(int row, int column)
        {
            if (row >= _bubblesViews.GetLength(0))
            {
                return false;
            }

            return column < _bubblesViews.GetLength(1);
        }
    }
}
