using System;
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
    [SerializeField] private float _shooterHeight;
    [SerializeField] private int _conditionWinInLastRowPercent;
    
    private Dictionary<ColorEnum, Color> _colorsByEnum;
    private Dictionary<Char, ColorEnum> _colorEnumsByChar;
    
    public BubbleView BubbleView => _bubbleView;
    public IReadOnlyList<ColorConfigData> ColorBubbleData => _colorData;
    public TextAsset FieldTextAsset => _fieldTextAsset;
    public float ShooterHeight => _shooterHeight;
    public int ConditionWinInLastRowPercent => _conditionWinInLastRowPercent;


    public Color GetColorByEnum(ColorEnum colorEnum)
    {
        if (_colorsByEnum == null)
        {
            _colorsByEnum = new Dictionary<ColorEnum, Color>();
            foreach (var item in _colorData)
            {
                _colorsByEnum[item.ColorEnum] = item.Color;
            }
        }
        return _colorsByEnum[colorEnum];
    }

    public ColorEnum GetColorByChar(char colorChar)
    {
        if (_colorEnumsByChar == null)
        {
            _colorEnumsByChar = new Dictionary<Char, ColorEnum>();
            foreach (var item in _colorData)
            {
                _colorEnumsByChar[item.Char] = item.ColorEnum;
            }
        }
        return _colorEnumsByChar[colorChar];
    }
}