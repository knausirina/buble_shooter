using System.Collections.Generic;
using Data;
using UnityEngine;
using Views;

namespace Field
{
    public class FieldBuilder
    {
        private readonly Config _config;
        private readonly PoolBalls _poolBalls;
        
        private readonly Vector2Int _fieldSizeInPixels;
        private readonly Vector2Int _fieldSizeInElements;
        
        private float _sizeBall;
        private List<BubbleView> _bubblesViews;
        
        public float SizeBall => _sizeBall;
        
        public FieldBuilder(Config config, PoolBalls poolBalls)
        {
            _config = config;
            _poolBalls = poolBalls;
        }

        public void Build(GameContext gameContext, BubblesData bubblesData, Vector2Int fieldSizeInPixels, Vector2Int fieldSizeInElements)
        {
            var field = gameContext.FieldRectTransform;
            var corners = new Vector3[4];
            field.GetWorldCorners(corners);
            
            var widthFieldInWorldDimentions = corners[3].x - corners[0].x;
            _sizeBall = 1.5f * widthFieldInWorldDimentions / fieldSizeInElements.y;
            var offset = _sizeBall / 10;
            
            gameContext.BubblesViewRoot.position = new Vector3(corners[1].x, corners[1].y, 0);

            _bubblesViews ??= new List<BubbleView>();
            for (var i = 0; i < bubblesData.RowsCount; i++)
            {
                for (var j = 0; j < bubblesData.ColumnCount ; j++)
                {
                    var bubbleData = bubblesData.Get(i, j);
                    if (bubbleData == null)
                    {
                        continue;
                    }

                    var remainder = i - i / 2 * 2;
                    var pos = new Vector2(j * (_sizeBall + offset) + 3 * remainder * offset, -i * (_sizeBall + offset));
                    var bubble = _poolBalls.Pool.Get();
                    bubble.gameObject.transform.parent = gameContext.BubblesViewRoot;
                    bubble.transform.localPosition = pos;
                    bubble.Renderer.color = _config.GetColorByEnum(bubbleData.Color);
                    bubble.Renderer.gameObject.transform.localScale = new Vector3(_sizeBall, _sizeBall, 1);
                    
                    _bubblesViews.Add(bubble);
                }
            }
        }

        public void Clear()
        {
            foreach (var bubble in _bubblesViews)
            {
                _poolBalls.Pool.Release(bubble);
            }
            
            _bubblesViews.Clear();
        }
    }
}
