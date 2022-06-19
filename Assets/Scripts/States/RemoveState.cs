using EventChannels;
using Models;
using Selection;
using UI;
using UnityEngine;

namespace States
{
    public class RemoveState : State
    {
        private Transform _selection;
        private TouchManager<Placeable> _touchManager;
        private DeleteTouchPanel _ui;
        public RemoveState(StateManager stateManager) : base(stateManager) {}

        public override void EnterState()
        {
            Debug.Log("ENTER STATE DELETE STATE");
            FocusEventChannel.Focused += OnFocused;
            FocusEventChannel.Unfocused += OnUnfocused;
            StateEventChannel.Confirmed += OnConfirmed;
            StateEventChannel.Cancelled += OnCancelled;
            UIEventChannel.UIChanged += SetUIReference;
            ConfigureSelectionManager();
        }

        public override void ExitState()
        {
            FocusEventChannel.Focused -= OnFocused;
            FocusEventChannel.Unfocused -= OnUnfocused;
            StateEventChannel.Confirmed -= OnConfirmed;
            StateEventChannel.Cancelled -= OnCancelled;
            UIEventChannel.UIChanged -= SetUIReference;
        }
        
        private void SetUIReference(GameObject newUI)
        {
            _ui = newUI.GetComponent<DeleteTouchPanel>();
            if (_ui == null) Debug.Log("Material state UI was not set correctly!");
        }

        private void OnUnfocused()
        {
            _selection = null;
            if (_ui != null) _ui.HideButtons();
        }

        private void OnFocused(Transform transform)
        {
            _selection = transform;
            if (_ui != null) _ui.ShowButtons();
        }

        protected override void OnConfirmed()
        {
            Debug.Log("Attempting to remove " + _selection.gameObject.name);
            Object.Destroy(_selection.gameObject);
            _selection = null;
            if (_ui != null) _ui.HideButtons();
        }

        protected override void OnCancelled()
        {
            Debug.Log("DELETE STATE ::: ON CANCELLED");
            _selection = null;
            if (_ui != null) _ui.HideButtons();
        }

        protected void ConfigureSelectionManager()
        {
            _touchManager = new TouchManager<Placeable>();
        }

        public override void UpdateState()
        {
            _touchManager.Update();
        }
    }
}