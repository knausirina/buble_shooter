using UnityEngine;

namespace Slingshot
{
    public class SlingShotLines : MonoBehaviour
    {
        [SerializeField] private Transform leftAnchor;
        [SerializeField] private Transform rightAnchor;
        [SerializeField] private Transform ObjectHolder;
        [SerializeField] private Transform _traectory;
        [SerializeField] private LineRenderer[] lines;
        [SerializeField] private GameObject[] points;
        
        public Transform Traectory => _traectory;
        
        private void Start()
        {
            lines[0].SetPosition(0, leftAnchor.position);
            lines[1].SetPosition(0, rightAnchor.position);
            SetPath(true);
        }

        private void Update()
        {
            foreach (var t in lines)
            {
                t.SetPosition(1, ObjectHolder.position);
            }
        }

        public void SetPath(bool b)
        {
            float yPos = (_traectory.up.y * 50) / points.Length;
            float val = yPos;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].SetActive(b);
                points[i].transform.parent = _traectory;
                points[i].transform.localPosition = new Vector3(0, val, -0.8f);
                val += yPos;
            }
        }
    }
}