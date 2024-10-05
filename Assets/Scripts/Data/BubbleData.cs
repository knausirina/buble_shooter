using UnityEngine;

namespace Data
{
    public class BubbleData
    {
        public ColorEnum Color { get; }
        public Vector2Int Position { get; }

        public BubbleData(ColorEnum colorEnum, Vector2Int position)
        {
            Color = colorEnum;
            Position = position;
        }

        public override string ToString()
        {
            return $"Color= {Color} Position={Position}";
        }
    }
}