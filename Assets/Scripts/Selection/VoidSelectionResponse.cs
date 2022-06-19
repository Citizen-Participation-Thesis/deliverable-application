using UnityEngine;

namespace Selection
{
    public class VoidSelectionResponse: ISelectionResponse
    {
        public void OnSelect(Transform selection) { }

        public void OnDeselect(Transform selection) { }
    }
}