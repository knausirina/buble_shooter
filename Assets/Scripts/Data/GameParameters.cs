using System.Collections;
using UnityEngine;

public class GameParameters
{
    public float BallSize { get; set; }
    public Vector2Int FieldSizeInPixels { get; set; } = Vector2Int.zero;
    public Vector2Int FieldSizeInElements { get; set; } = Vector2Int.zero;
    public int MaxCountBubbles { get; set; } = 0;
}