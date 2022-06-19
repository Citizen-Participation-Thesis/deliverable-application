using UnityEngine;

namespace Selection
{
    public class SelectionManager
    {
        private readonly ISelector _selector;
        private readonly IRayProvider _rayProvider;
        private readonly ISelectionResponse _selectionResponse;
        
        private Transform _currentSelection;

        public SelectionManager(ISelector selector, IRayProvider rayProvider, ISelectionResponse selectionResponse)
        {
            _selector = selector;
            _rayProvider = rayProvider;
            _selectionResponse = selectionResponse;
        }

        public void Update()
        {
            var ray = _rayProvider.CreateRay();

            _selector.Check(ray);
            var selected = _selector.GetSelection();

            // If the new selection is the same as the old, there is nothing to do.
            if (selected == _currentSelection) return;

            // The new selection is different from the old. If we had something selected before, deselect it.
            if (_currentSelection != null) _selectionResponse.OnDeselect(_currentSelection);
            
            // Set the new selection
            _currentSelection = selected;
            Debug.Log("New selection " + _currentSelection.name);
            
            // If the new selection is of something, then trigger OnSelect
            if (_currentSelection != null) _selectionResponse.OnSelect(_currentSelection);
        }
    }
}
