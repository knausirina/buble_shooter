using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubblesViewRoot;
    [SerializeField] private RectTransform _fieldRectTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private NextBubbleView _nextBubbleView;

    private Vector3[] _fieldCorners;
    
    public Transform BubblesViewRoot => _bubblesViewRoot;
    public RectTransform FieldRectTransform => _fieldRectTransform;
    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
    public NextBubbleView NextBubbleView => _nextBubbleView;

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