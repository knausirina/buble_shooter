using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubbleViewRoot;

    public Transform BubbleViewRoot => _bubbleViewRoot;
}