using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CommentScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float snapSpeed;

        private float _gap;
        private bool _released;
        private float _thresholdSpeed;
        private void Start()
        {
            // The _gap is the fraction of the total content area of the scroll view that one page takes
            _gap = 1f;

            scrollRect.verticalNormalizedPosition = _gap; 
            _released = true;
        }

        private void FixedUpdate()
        {
            if (_released)
            {
                scrollRect.verticalNormalizedPosition =
                    Mathf.Lerp(scrollRect.verticalNormalizedPosition, 0f, snapSpeed);
            }
        }

        public void OnBeginDrag()
        {
            _released = false;
        }

        public void OnEndDrag()
        {
            _released = true;
        }
    }
}