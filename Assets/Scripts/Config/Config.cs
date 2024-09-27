using System.Collections.Generic;
using Data;
using UnityEngine;
using Views;

[CreateAssetMenu(fileName = "Config", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    [SerializeField] private BubbleView _bubbleView;
    [SerializeField] private ColorData[] _colorData;
    [SerializeField] private TextAsset _fieldTextAsset;
        
    public BubbleView BubbleView => _bubbleView;
    public IReadOnlyList<ColorData> BubbleData => _colorData;
    public TextAsset FieldTextAsset => _fieldTextAsset;
}