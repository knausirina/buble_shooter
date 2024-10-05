using Data;
using UnityEngine;

namespace Field
{
    public class FieldBuilder
    {
        private readonly Config _config;
        
        public FieldBuilder(Config config)
        {
            _config = config;
        }

        public void Build(GameContext gameContext, BubblesData bubblesData)
        {
            for (var i = 0; i < bubblesData.RowsCount; i++)
            {
                for (var j = 0; j < bubblesData.ColumnCount ; j++)
                {
                    var bubleData = bubblesData.Get(i, j);
                    if (bubleData == null)
                    {
                        continue;
                    }

                    var pos = new Vector2(j * 0.3f, -i * 0.3f);
                    var view = Object.Instantiate(_config.BubbleView, Vector3.zero, Quaternion.identity, gameContext.BubblesViewRoot);
                    view.transform.localPosition = pos;
                    view.Renderer.color = _config.GetColor(bubleData.Color);
                }
            }
        }
    }
}
