using EventChannels;
using UnityEngine;

namespace Selection
{
    public class FocusEventSelectionResponse : ISelectionResponse
    {
        public void OnSelect(Transform selection)
        {
            Debug.Log("SELECTING " + selection.name);
            FocusEventChannel.RaiseFocusedEvent(selection);
        }

        public void OnDeselect(Transform selection)
        {
            Debug.Log("DESELECTING " + selection.name);
            FocusEventChannel.RaiseUnfocusedEvent();
        }
    }
}