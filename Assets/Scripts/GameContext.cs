using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubblesViewRoot;
    [SerializeField] private RectTransform _fieldRectTransform;
    [SerializeField] private Transform _shootBubbleTransform;
    [SerializeField] private Transform _slingshotLinesTransform;
    [SerializeField] private Slingshot.SlingShot _slingshot;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    public Transform BubblesViewRoot => _bubblesViewRoot;
    public RectTransform FieldRectTransform => _fieldRectTransform;
    public Slingshot.SlingShot Slingshot => _slingshot;
    public Transform SlingshotLinesTransform => _slingshotLinesTransform;
    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
}