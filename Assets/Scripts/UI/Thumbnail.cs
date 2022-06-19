using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Thumbnail : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image background;
        [SerializeField] private Image image;
        [SerializeField] private Button button;

        [SerializeField] private Color highlightColor;
        [SerializeField] private Color defaultColor;

        private Button _button;
        private int _index;
        private Action<int> _action;
        
        private void OnEnable() => button.onClick.AddListener(OnClick);
        private void OnDisable() => button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            Debug.Log($"Thumbnail {_index} clicked!");
            _action.Invoke(_index);
            SetHighlight(true);
        }

        public void SetData(int index, string text, Sprite sprite, Action<int> action)
        {
            _index = index;
            _action = action;

            title.text = text;
            image.sprite = sprite;
            background.color = defaultColor;
        }

        public void SetHighlight(bool set) => background.color = set ? highlightColor : defaultColor;
    }
}