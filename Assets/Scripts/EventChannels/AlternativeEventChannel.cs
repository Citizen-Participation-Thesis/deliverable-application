using System;
using UnityEngine;

namespace EventChannels
{
    public static class AlternativeEventChannel
    {
        public static event Action<int> AlternativeChanged;
        public static event Action<int> CategoryChanged; 

        public static void RaiseAlternativeChangedEvent(int newAlternativeName)
        {
            Debug.Log("Alternative Event Raised");
            AlternativeChanged?.Invoke(newAlternativeName);
        }
        
        public static void RaiseCategoryChangedEvent(int newCategoryName) 
        {
            Debug.Log("Category Event Raised");
            CategoryChanged?.Invoke(newCategoryName);
        }
    }
}