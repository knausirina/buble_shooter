using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NextBubbleView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countField;
    [SerializeField] private Image _image;

    public void SetCount(int count)
    {
        _countField.text = count.ToString();
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }
}