using Slingshot;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubbleViewRoot;
    [SerializeField] private RectTransform _fieldRectTransform;

    public Transform BubbleViewRoot => _bubbleViewRoot;
}