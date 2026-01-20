using UnityEngine;

public class BubbleData
{
    public Color ColorValue { get; }
    public Vector2Int Position { get; }

    public BubbleData(Color colorValue, Vector2Int position)
    {
        ColorValue = colorValue;
        Position = position;
    }

    public override string ToString()
    {
        return $"Color = {ColorValue} Position = {Position}";
    }
}