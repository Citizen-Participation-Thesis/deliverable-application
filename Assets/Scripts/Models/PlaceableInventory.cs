using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Models
{
    public class PlaceableInventory : ICategoryData
    {
        private readonly Dictionary<string, List<GameObject>> _dictionary;
        private readonly List<string> _categoryNames;
        private List<GameObject> _currentAlternatives;

        private int _currentCategoryIndex;
        private int _currentAlternativeIndex;
        
        public event Action CategorySet;
        public event Action AlternativeSet;

        public PlaceableInventory()
        {
            _dictionary = Services.Get<DataManager>().GetPlaceableModels();
            _categoryNames = _dictionary.Keys.ToList();
            _currentCategoryIndex = 0;
            _currentAlternatives = _dictionary[_categoryNames[_currentCategoryIndex]];
            _currentAlternativeIndex = 0;
        }

        public List<string> GetAlternatives() => _currentAlternatives.Select(p => p.name).ToList();
        public int GetCurrentAlternativeIndex() => _currentAlternativeIndex;

        public void SetAlternative(int alternativeIndex)
        {
            _currentAlternativeIndex = alternativeIndex;
            AlternativeSet?.Invoke();
        }

        public List<string> GetCategories() => _categoryNames;
        public int GetCurrentCategoryIndex() => _currentCategoryIndex;

        public void SetCategory(int categoryIndex)
        {
            _currentCategoryIndex = categoryIndex;
            _currentAlternatives = _dictionary[_categoryNames[_currentCategoryIndex]];
            _currentAlternativeIndex = 0;
            CategorySet?.Invoke();
        }
    }
}