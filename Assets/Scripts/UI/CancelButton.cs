using EventChannels;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CancelButton: MonoBehaviour
    {
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        private void OnClick() => StateEventChannel.RaiseCancelledEvent();
    }
}