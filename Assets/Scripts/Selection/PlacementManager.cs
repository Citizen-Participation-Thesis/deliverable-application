using Data;
using EventChannels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Selection
{
    public class PlacementManager
    {
        private readonly ISelector _selector;
        private readonly IRayProvider _rayProvider;
        private readonly ISelectionResponse _selectionResponse;
        private readonly IHitResponse _hitResponse;
        
        private Transform _currentSelection;

        public PlacementManager(ISelector selector, IRayProvider rayProvider, 
            ISelectionResponse selectionResponse, IHitResponse hitResponse)
        {
            _selector = selector;
            _rayProvider = rayProvider;
            _selectionResponse = selectionResponse;
            _hitResponse = hitResponse;
        }

        public void Update()
        {
            if (!(Input.touchCount > 0))
            {
                _hitResponse.OnMiss();
                return;
            }

            if (EventSystem.current.IsPointerOverRaycastTargetUI())
            {
                _hitResponse.OnMiss();
                return;
            }
            
            if (Input.GetTouch(0).phase != TouchPhase.Ended) return;
            
            Debug.Log("TOUCH ENDED");
            StateEventChannel.RaiseConfirmedEvent();
            
            var hit = _selector.Check(_rayProvider.CreateRay());
            var selected = _selector.GetSelection();

            if (selected == null)
            {
                _hitResponse.OnMiss();
                return;
            }
            
            _hitResponse.OnHit(hit);
        }
    }
}