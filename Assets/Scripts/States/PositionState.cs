using EventChannels;
using Models;
using Selection;
using UI;
using UnityEngine;

namespace States
{
    public class PositionState : State
    {
        private TouchManager<Movable> _touchManager;
        private Transform _selection;
        private IAlternativeData _alternativeData;
        private AlternativeTouchPanel _ui;

        public PositionState(StateManager stateManager) : base(stateManager) { }
        public override void EnterState()
        {
            AlternativeEventChannel.AlternativeChanged += OnAlternativeChanged;
            FocusEventChannel.Focused += OnFocused;
            FocusEventChannel.Unfocused += OnUnfocused;
            UIEventChannel.UIChanged += SetUIReference; 
            ConfigureSelectionManager();
        }

        public override void ExitState()
        {            
            AlternativeEventChannel.AlternativeChanged -= OnAlternativeChanged;
            FocusEventChannel.Focused -= OnFocused;
            FocusEventChannel.Unfocused -= OnUnfocused;
            UIEventChannel.UIChanged -= SetUIReference;
            if (_ui != null) _ui.ClearData();
        }
        
        private void SetUIReference(GameObject newUI)
        {
            _ui = newUI.GetComponent<AlternativeTouchPanel>();
            if (_ui == null) Debug.Log("Material state UI was not set correctly! Incorrect Touch Panel Type for State");
        }

        public override void UpdateState()
        {
            _touchManager.Update();
        }
        
        protected void ConfigureSelectionManager()
        {
            _touchManager = new TouchManager<Movable>();
        }

        protected override void OnConfirmed()
        {
            Debug.Log("Position Change OnConfirmed");
        }

        protected override void OnCancelled()
        {
            Debug.Log("Position Change Cancelled");
        }

        protected void OnFocused(Transform selection)
        {
            Debug.Log("Position Change OnFocused");
            _alternativeData = selection.GetComponent<Movable>();
            if (_ui != null) _ui.SetData(_alternativeData);
        }

        protected void OnUnfocused()
        {
            Debug.Log("Position Change OnUnfocused");
            if (_ui != null) _ui.ClearData();
        }

        protected void OnAlternativeChanged(int alternativeIndex)
        {
            Debug.Log("Position Change Alternative");
            _alternativeData.SetAlternative(alternativeIndex);
        }
    }
}