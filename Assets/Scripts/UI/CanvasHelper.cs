using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        public static UnityEvent onOrientationChange = new UnityEvent();

        private ScreenOrientation _lastOrientation = ScreenOrientation.Portrait;
        private Vector2 _lastResolution = Vector2.zero;
        private Rect _lastSafeArea = Rect.zero;

        private Canvas _canvas;
        private RectTransform _safeAreaTransform;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            _safeAreaTransform = transform as RectTransform;
            _lastOrientation = Screen.orientation;

        }

        private void Start()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Screen.orientation != _lastOrientation)
                OrientationChanged();
            if (Screen.safeArea != _lastSafeArea)
                SafeAreaChanged();
            if (Screen.width != _lastResolution.x || Screen.height != _lastResolution.y)
                ResolutionChanged();      
        }

        private void ApplySafeArea()
        {
            if (_safeAreaTransform == null)
            {
                return;
            }

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            var pixelRect = _canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;
            _safeAreaTransform.anchorMin = anchorMin;
            _safeAreaTransform.anchorMax = anchorMax;
        }
    
        private void ResolutionChanged()
        {
            if (_lastResolution.x == Screen.width && _lastResolution.y == Screen.height)
                return;

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
        }

        private void SafeAreaChanged()
        {
            if (_lastSafeArea == Screen.safeArea)
                return;

            _lastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }

        private void OrientationChanged()
        {
            _lastOrientation = Screen.orientation;

            onOrientationChange.Invoke();
        }
    }
}