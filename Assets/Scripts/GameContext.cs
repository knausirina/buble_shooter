using Slingshot;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _bubblesViewRoot;
    [SerializeField] private RectTransform _fieldRectTransform;
    [SerializeField] private SlingShot _slingShot;
    [SerializeField] private SlingShotLines _slingShotLines;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    private float _canvasHeight;
    
    public Transform BubblesViewRoot => _bubblesViewRoot;
    public RectTransform FieldRectTransform => _fieldRectTransform;
    public Camera Camera => _camera;
    public SlingShot SlingShot => _slingShot;
    public SlingShotLines SlingShotLines => _slingShotLines;
    
    public float GetHeightCanvas()
    {
        if (_canvasHeight == 0)
            _canvasHeight = _canvas.GetComponent<RectTransform>().rect.height;
        return _canvasHeight;
    }

    public Vector3 GetBottomPointOfFiledWorldPosition()
    {
        var worldPosition = Camera.ScreenToWorldPoint(new Vector3(0,
            GetHeightCanvas() - FieldRectTransform.sizeDelta.y +
            FieldRectTransform.anchoredPosition.y, Camera.nearClipPlane));
        worldPosition.x = 0;

        return worldPosition;
    }
}