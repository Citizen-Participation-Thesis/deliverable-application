using EventChannels;
using Models;
using Selection;
using UI;
using UnityEngine;

namespace States
{
    public class MaterialState : State
    {
        protected TouchManager<MaterialChanger> _touchManager;
        protected Transform Selection;

        private ICategoryData _categoryData;
        private CategorizedTouchPanel _ui;

        public MaterialState(StateManager stateManager) : base(stateManager) { }

        protected void ConfigureSelectionManager()
        {
            _touchManager = new TouchManager<MaterialChanger>();
        }
        
        public override void EnterState()
        {
            Debug.Log("ENTER STATE MATERIAL STATE");
            AlternativeEventChannel.AlternativeChanged += OnAlternativeChanged;
            AlternativeEventChannel.CategoryChanged += OnCategoryChanged;
            FocusEventChannel.Focused += OnFocused;
            FocusEventChannel.Unfocused += OnUnfocused;
            UIEventChannel.UIChanged += SetUIReference;
            ConfigureSelectionManager();
        }

        private void SetUIReference(GameObject newUI)
        {
            Debug.Log($"MATERIAL STATE: UI was set to {newUI.name}");
            _ui = newUI.GetComponent<CategorizedTouchPanel>();
            if (_ui == null) Debug.Log("Material state UI was not set correctly!");
        }

        public override void ExitState()
        {
            AlternativeEventChannel.AlternativeChanged -= OnAlternativeChanged;
            AlternativeEventChannel.CategoryChanged -= OnCategoryChanged;
            FocusEventChannel.Focused -= OnFocused;
            FocusEventChannel.Unfocused -= OnUnfocused;
            UIEventChannel.UIChanged -= SetUIReference;
            if (_ui != null) _ui.ClearData();
        }

        public override void UpdateState()
        {
            _touchManager.Update();
        }
        
        protected override void OnConfirmed()
        {
            Debug.Log("Material State Confirmed");
        }

        protected override void OnCancelled()
        {
            Debug.Log("Material State Cancelled");
        }

        protected void OnFocused(Transform selection)
        {
            Debug.Log("Material State Focused");
            
            Selection = selection;
            _categoryData = Selection.GetComponent<MaterialChanger>();
            
            if (_ui != null) _ui.SetData(_categoryData);
            
            OnCategoryChanged(_categoryData.GetCurrentCategoryIndex());
        }

        protected void OnUnfocused()
        {
            Debug.Log("Material State Unfocused");
            //_materialChanger.ResetEffects();
            Selection = null;
            _categoryData = null;
            
            if (_ui != null) _ui.ClearData();
        }

        protected void OnAlternativeChanged(int newAlternativeIndex)
        {
            Debug.Log("Material State Alternative Changed");
            _categoryData.SetAlternative(newAlternativeIndex);
        }

        protected void OnCategoryChanged(int newCategoryIndex)
        {
            Debug.Log("Material State Category Changed");
            _categoryData.SetCategory(newCategoryIndex);
        }
    }
}