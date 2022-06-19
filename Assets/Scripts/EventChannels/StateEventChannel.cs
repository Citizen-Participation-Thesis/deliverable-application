using System;
using States;
using UnityEngine;

namespace EventChannels
{
    public static class StateEventChannel
    {
        public static event Action<State> StateChanged;
        public static event Action<State> PreviewChanged;
        public static event Action Confirmed;
        public static event Action Cancelled;

        public static void RaiseStateChangedEvent(State state)
        {
            //Debug.Log("State Change Event Raised! Changing to " + state.Data.className);
            StateChanged?.Invoke(state);
        }

        public static void RaisePreviewChangedEvent(State state)
        {
            //Debug.Log("Preview Event Raised! Previewing " + state.Data.className);
            PreviewChanged?.Invoke(state);
        }

        public static void RaiseConfirmedEvent()
        {
            Debug.Log("Confirm Event Raised!");
            Confirmed?.Invoke();
        }

        public static void RaiseCancelledEvent()
        {
            //Debug.Log("Cancelled Event Raised!");
            Cancelled?.Invoke();
        }
    }
}