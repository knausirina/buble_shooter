using Slingshot;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubblesViewRoot;
    [SerializeField] private RectTransform _fieldRectTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SlingShot _slingShot;
    [SerializeField] private SlingShotLines _slingShotLines;
    [SerializeField] private NextBubbleView _nextBubbleView;
    [SerializeField] private GameObject _resultGameViewPrefab;
    [SerializeField] private Transform _popupRootTransform;

    private float _canvasHeight;
    private Vector3[] _fieldCorners;
    
    public Transform BubblesViewRoot => _bubblesViewRoot;
    public RectTransform FieldRectTransform => _fieldRectTransform;
    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
    public SlingShot SlingShot => _slingShot;
    public SlingShotLines SlingShotLines => _slingShotLines;
    public NextBubbleView NextBubbleView => _nextBubbleView;
    public GameObject ResultGameViewPrefab => _resultGameViewPrefab;
    public Transform PopupRootTransform => _popupRootTransform;

    public Vector3 GetRightBottomAngle()
    {
        InitFieldCorners();
        
        return _fieldCorners[3];
    }
    
    public Vector3 GetLeftBottomAngle()
    {
        InitFieldCorners();
        
        return _fieldCorners[0];
    }

    private void InitFieldCorners()
    {
        if (_fieldCorners == null)
        {
            _fieldCorners = new Vector3[4];
            FieldRectTransform.GetWorldCorners(_fieldCorners);
        }
    }
}