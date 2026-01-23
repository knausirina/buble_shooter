using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

[CreateAssetMenu(fileName = "Config", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    [field: SerializeField] public BubbleView BubblePrefab { get; private set; }
    [field: SerializeField] public GameObject SlinghshotPrefab { get; private set; }
    [field: SerializeField] public TextAsset FieldTextAsset { get; private set; }
    [field: SerializeField] public float ShooterHeight { get; private set; }
    [field: SerializeField] public int ConditionWinInLastRowPercent { get; private set; }

    [SerializeField] private ColorConfigData[] _colorData;

    private Dictionary<Char, Color> _colors;
    
    public IReadOnlyList<ColorConfigData> ColorBubbleData => _colorData;

    public Color GetColor(char colorChar)
    {
        if (_colors == null)
        {
            _colors = new Dictionary<Char, Color>();
            foreach (var item in _colorData)
            {
                _colors[item.Char] = item.Color;
            }
        }
        return _colors[colorChar];
    }

    public Color GetRandomColor()
    {
        var randomIndex = UnityEngine.Random.Range(0, _colorData.Length);
        var colorData = _colorData[randomIndex];
        return colorData.Color;
    }
}