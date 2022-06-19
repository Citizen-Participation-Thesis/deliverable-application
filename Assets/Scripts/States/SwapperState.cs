using EventChannels;
using Models;
using Selection;
using UI;
using UnityEngine;

namespace States
{
    public class SwapperState : State
    {
        private TouchManager<ObjectSwapper> _touchManager;
        private Transform _selection;
        
        private IAlternativeData _alternativeData;
        private AlternativeTouchPanel _ui;

        public SwapperState(StateManager stateManager) : base(stateManager) { }

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
            if (_ui == null) Debug.Log("Material state UI was not set correctly!");
        }

        public override void UpdateState()
        {
            _touchManager.Update();
        }

        protected override void OnConfirmed()
        {
            Debug.Log("Swapper OnConfirmed");
        }

        protected override void OnCancelled()
        {
            Debug.Log("Swapper OnCancelled");
        }

        private void ConfigureSelectionManager()
        {
            _touchManager = new TouchManager<ObjectSwapper>();
        }
        
        protected void OnFocused(Transform selection)
        {
            Debug.Log("Swapper OnFocused");
            _selection = selection;
            _alternativeData = _selection.GetComponent<ObjectSwapper>();
            if (_ui != null) _ui.SetData(_alternativeData);
        }

        protected void OnUnfocused()
        {
            Debug.Log("Swapper OnUnfocused");
            _selection = null;
            _alternativeData = null;
            if (_ui != null) _ui.ClearData();
        }

        protected void OnAlternativeChanged(int alternativeIndex)
        {
            Debug.Log("Swapper OnAlternativeChanged");
            _alternativeData.SetAlternative(alternativeIndex);
        }
    }
}