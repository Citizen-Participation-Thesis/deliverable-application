using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModeButton: MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private SVGImage svgImage;
        [SerializeField] private TMP_Text title;

        private int _index;
        private Action<int> _action;

        public void SetData(int index, string text, Sprite sprite, Action<int> action)
        {
            _index = index;
            svgImage.sprite = sprite;
            _action = action;
            title.text = text;
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log("Mode Button was Clicked");
            _action?.Invoke(_index);
        }
    }
}