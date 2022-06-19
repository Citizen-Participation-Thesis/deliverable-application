using System;
using UnityEngine;

namespace EventChannels
{
    public static class FocusEventChannel
    {
        public static event Action<Transform> Focused;
        public static event Action Unfocused;
        
        public static void RaiseFocusedEvent(Transform transform)
        {
            //Debug.Log("Focus Event Raised");
            Focused?.Invoke(transform);
        }

        public static void RaiseUnfocusedEvent()
        {
            //Debug.Log("Unfocused Event Raised");
            Unfocused?.Invoke();
        }
    }
}