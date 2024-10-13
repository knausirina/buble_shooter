using Data;
using UnityEngine;
using Views;

namespace Field
{
    public class NextBubbleSystem
    {
        private readonly PoolBalls _poolBalls;
        private readonly Config _config;

        public NextBubbleSystem(Config config, PoolBalls poolBalls)
        {
            _config = config;
            _poolBalls = poolBalls;
        }

        public BubbleView GetNextBubble()
        {
            var bubbleView = _poolBalls.Pool.Get();
            var color = Random.Range(1, System.Enum.GetValues(typeof(ColorEnum)).Length);
            var colorEnum = (ColorEnum)color;
            bubbleView.SetColor(_config.GetColorByEnum(colorEnum));
            return bubbleView;
        }

        public void ReturnBubble(BubbleView bubbleView)
        {
            _poolBalls.Pool.Release(bubbleView);
        }
    }
}