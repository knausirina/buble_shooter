using Data;
using UnityEngine;

namespace Field
{
    public class FieldBuilder
    {
        private readonly Config _config;
        
        private readonly Vector2Int _fieldSizeInPixels;
        private readonly Vector2Int _fieldSizeInElements;
        
        private float _sizeBall;
        
        public float SizeBall => _sizeBall;
        
        public FieldBuilder(Config config)
        {
            _config = config;
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
                    var view = Object.Instantiate(_config.BubbleView, Vector3.zero, Quaternion.identity, gameContext.BubblesViewRoot);
                    view.transform.localPosition = pos;
                    view.Renderer.color = _config.GetColor(bubbleData.Color);
                    view.Renderer.gameObject.transform.localScale = new Vector3(_sizeBall, _sizeBall, 1);
                }
            }
        }
    }
}
