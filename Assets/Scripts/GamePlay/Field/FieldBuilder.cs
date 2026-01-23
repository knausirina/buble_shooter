using UnityEngine;
using Views;

public class FieldBuilder
{
    private readonly PoolBalls _poolBalls;
    private GameContext _gameContext;
    private BubbleView[,] _bubblesViews;

    private float _ballSize;
    private Vector2Int _size;

    public float BallSize => _ballSize;
    public BubbleView[,] BubblesViews => _bubblesViews;
    public FieldBuilder(PoolBalls poolBalls)
    {
        _poolBalls = poolBalls;
    }

    public BubbleView[,] Build(GameContext gameContext, BubblesData bubblesData, GameParameters gameParameters)
    {
        _gameContext = gameContext;

        var field = gameContext.FieldRectTransform;
        var corners = new Vector3[4];
        field.GetWorldCorners(corners);

        var fieldInWorldDimentionsWidth = corners[3].x - corners[0].x;
        _ballSize = (fieldInWorldDimentionsWidth / gameParameters.FieldSizeInElements.y);

        var offset = _ballSize / 10;
        _ballSize -= offset;

        var countRows = (int)(fieldInWorldDimentionsWidth / _ballSize);

        gameContext.BubblesViewRoot.position = new Vector3(corners[1].x, corners[1].y, 0);

        _size = new Vector2Int(bubblesData.RowsCount, bubblesData.ColumnCount);
        _bubblesViews = new BubbleView[countRows, bubblesData.ColumnCount];
        Debug.Log($"xxx FieldBuilder Build _size.x={_size.x} _size.y={_size.y}");
        for (var i = 0; i < _size.x; i++)
        {
            for (var j = 0; j < _size.y; j++)
            {
                var bubbleData = bubblesData.Get(i, j);
                if (bubbleData == null)
                {
                    continue;
                }

                var bubble = _poolBalls.Pool.Get();
                bubble.Renderer.color = bubbleData.ColorValue;
                AddBubble(bubble, i, j);
            }
        }

        return _bubblesViews;
    }

    public void Clear()
    {
        Debug.Log("FieldFuilder clear");

        if (_bubblesViews == null)
            return;

        for (var i = 0; i < _bubblesViews.GetLength(0); i++)
        {
            for (var j = 0; j < _bubblesViews.GetLength(1); j++)
            {
                var element = _bubblesViews[i, j];
                if (element != null)
                    Object.Destroy(_bubblesViews[i, j].gameObject);
            }
        }

        _bubblesViews = null;
    }

    public void RemoveBubble(BubbleView bubbleView, int row, int column)
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

        if (_bubblesViews[row, column] != null)
        {
            Debug.Log($"xxx not null in row={row} column={column}");
        }
        else
            _bubblesViews[row, column] = bubbleView;
    }

    public Vector2 GetPosition(int row, int column)
    {
        var offset = _ballSize / 10;
        var remainder = row - row / 2 * 2;
        return new Vector2(column * (_ballSize + offset) + _ballSize / 2 + 3 * remainder * offset,
            -row * (_ballSize + offset) - _ballSize / 2);
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