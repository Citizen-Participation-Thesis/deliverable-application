using System;
using UnityEngine;

namespace EventChannels
{
    public static class UIEventChannel
    {
        public static event Action<GameObject> UIChanged;
        
        public static void RaiseUIChangedEvent(GameObject newUI)
        {
            Debug.Log($"UI changed and was set to {newUI.name}");
            UIChanged?.Invoke(newUI);
        }
    }
}