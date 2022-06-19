using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public class ObjectSwapper : MonoBehaviour, IAlternativeData
    {
        public List<GameObject> alternativeData;
        private int _currentAlternativeIndex;

        private void OnEnable()
        {
            alternativeData.ForEach(o => o.SetActive(false));
            _currentAlternativeIndex = 0;
            alternativeData[_currentAlternativeIndex].SetActive(true);
        }

        public event Action AlternativeSet;
        public List<string> GetAlternatives() => alternativeData.Select(o => o.name).ToList();
        public int GetCurrentAlternativeIndex() => _currentAlternativeIndex;

        public void SetAlternative(int alternativeIndex)
        {
            alternativeData[_currentAlternativeIndex].SetActive(false);
            _currentAlternativeIndex = alternativeIndex;
            alternativeData[_currentAlternativeIndex].SetActive(true);
            AlternativeSet?.Invoke();
        }
    }
}