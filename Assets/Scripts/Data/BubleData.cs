using UnityEngine;

namespace Data
{
    public class BubleData
    {
        public ColorEnum Color { get; }
        public Vector2Int Position { get; }

        public BubleData(ColorEnum colorEnum, Vector2Int position)
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