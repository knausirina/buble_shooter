using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Views.UI
{
    public class NextBubbleView : MonoBehaviour
    {
        [SerializeField] TMP_Text _countField;
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
}