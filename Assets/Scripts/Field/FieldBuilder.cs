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

        public void Build(BublesData bublesData)
        {
            var gameContext = Object.FindObjectOfType<GameContext>();

            for (var i = 0; i < bublesData.RowsCount; i++)
            {
                for (var j = 0; j < bublesData.ColumnCount; j++)
                {
                    var bubleData = bublesData.Get(i, j);
                    if (bubleData == null)
                    {
                        continue;
                    }

                    var view = Object.Instantiate(_config.BubbleView, Vector3.zero, Quaternion.identity, gameContext.BubbleViewRoot);
                    view.Renderer.color = _config.GetColor(bubleData.Color);
                }
            }
        }
    }
}
