using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Utils : MonoBehaviour
{
    public static float RandomFloatBetween(float min, float max)
    {
        Random random = new Random();
        double range = max - min;
        double sample = random.NextDouble();
        double scaled = (sample * range) + min;
        return (float)scaled;
    }
}