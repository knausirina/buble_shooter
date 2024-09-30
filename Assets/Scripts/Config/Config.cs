using System.Collections.Generic;
using Data;
using UnityEngine;
using Views;

[CreateAssetMenu(fileName = "Config", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    [SerializeField] private BubbleView _bubbleView;
    [SerializeField] private ColorConfigData[] _colorData;
    [SerializeField] private TextAsset _fieldTextAsset;
        
    public BubbleView BubbleView => _bubbleView;
    public IReadOnlyList<ColorConfigData> BubbleData => _colorData;
    public TextAsset FieldTextAsset => _fieldTextAsset;

    private Dictionary<ColorEnum, Color> _colorsByEnum;
    public Color GetColor(ColorEnum colorEnum)
    {
        if (_colorsByEnum == null)
        {
            _colorsByEnum = new Dictionary<ColorEnum, Color>();
            foreach (var item in _colorData)
            {
                _colorsByEnum[item.ColorEnum] = item.Color;
            }
        }
        _colorsByEnum.TryGetValue(colorEnum, out var result);
        return result;
    }
}