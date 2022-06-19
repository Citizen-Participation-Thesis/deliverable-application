using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private Image fillBar;
        [SerializeField] private TMP_Text text;
        
        private float _currentValue;
        private float _targetValue;
        private bool _completed;

        private void OnEnable()
        {
            _completed = false;
            _targetValue = 0f;
            _currentValue = _targetValue;
            fillBar.fillAmount = _currentValue;
            text.SetText("0 %");
        }

        private void Update()
        {
            if (_completed) return;
            
            if (_currentValue < 1f)
            {
                _currentValue = Mathf.Lerp(_currentValue, _targetValue, 0.1f);
                fillBar.fillAmount = _currentValue;
                text.SetText(Mathf.RoundToInt(_currentValue * 100f) + " %");   
            }
            else
            {
                fillBar.fillAmount = 1f;
                text.SetText("100 %");

                _completed = true;
                fillBar.CrossFadeColor(Color.green, 0.1f, false, false);
            }
        }

        public void SetTargetValue(float targetValue)
        {
            _targetValue = targetValue;
        }
    }
}