using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Movable: MonoBehaviour, IAlternativeData
    {
        public List<Vector3> Positions;

        public List<Vector3> EulerAngleRotations;

        public List<string> AlternativeNames;

        private int _currentAlternativeIndex;
        private Transform _modelTransform;
        public event Action AlternativeSet;

        private void Awake()
        {
            _currentAlternativeIndex = 0;
            _modelTransform = GetComponent<Transform>();
        }

        public List<string> GetAlternatives() => AlternativeNames;
        public int GetCurrentAlternativeIndex() => _currentAlternativeIndex;

        public void SetAlternative(int alternativeIndex)
        {
            _currentAlternativeIndex = alternativeIndex;
            _modelTransform.localRotation = Quaternion.Euler(EulerAngleRotations[_currentAlternativeIndex]);
            _modelTransform.localPosition = Positions[_currentAlternativeIndex];
            AlternativeSet?.Invoke();
        }
    }
}