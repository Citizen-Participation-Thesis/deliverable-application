using System.Collections.Generic;
using Data;
using EventChannels;
using Models;
using Selection;
using UI;
using UnityEngine;

namespace States
{
    public class PlacementState : State
    {
        private NewPlacementManager<Ground> _placementManager;
        private Dictionary<string, List<GameObject>> _placeableModels;
        
        private ICategoryData _categoryData;
        private GameObject _blueprint;
        private CategorizedTouchPanel _ui;
        private int _index;

        public PlacementState() {}

        public PlacementState(StateManager stateManager) : base(stateManager)
        {
            _placeableModels = Services.Get<DataManager>().GetPlaceableModels();
            _categoryData = new PlaceableInventory();
            
            if (_placeableModels.Count != 0)
            {
                Debug.Log("_placeablemodels count " + _placeableModels.Count);
                Debug.Log("");
                var currentCategory = _categoryData.GetCategories()[_categoryData.GetCurrentCategoryIndex()];
                Debug.Log("Current category " + currentCategory);
                _blueprint = _placeableModels[currentCategory][_categoryData.GetCurrentAlternativeIndex()];
                Debug.Log("_blueprint name " + _blueprint.name);
            }
            else
            {
                Debug.Log("_placeableModels was 0");
                _blueprint = null;
            }
            
            _placementManager = new NewPlacementManager<Ground>();
            _placementManager.SetBlueprint(_blueprint);
        }

        protected void OnAlternativeChanged(int newAlternativeIndex)
        {
            _categoryData.SetAlternative(newAlternativeIndex);
            
            var currentCategory = _categoryData.GetCategories()[_categoryData.GetCurrentCategoryIndex()];
            _blueprint = _placeableModels[currentCategory][_categoryData.GetCurrentAlternativeIndex()];
            _placementManager.SetBlueprint(_blueprint);
        }

        public override void EnterState()
        {
            Debug.Log("ENTER PLACEMENT STATE");
            AlternativeEventChannel.AlternativeChanged += OnAlternativeChanged;
            AlternativeEventChannel.CategoryChanged += OnCategoryChanged;
            StateEventChannel.Confirmed += OnConfirmed;
            UIEventChannel.UIChanged += SetUIReference;
        }
        
        private void SetUIReference(GameObject newUI)
        {
            _ui = newUI.GetComponent<CategorizedTouchPanel>();
            if (_ui == null) Debug.Log("Material state UI was not set correctly!");
            if (_ui != null)
            {
                Debug.Log("UI WAS SET");
                _ui.SetData(_categoryData);
            }
        }

        private void OnCategoryChanged(int newCategoryIndex)
        {
            _categoryData.SetCategory(newCategoryIndex);
            
            var currentCategory = _categoryData.GetCategories()[_categoryData.GetCurrentCategoryIndex()];
            _blueprint = _placeableModels[currentCategory][_categoryData.GetCurrentAlternativeIndex()];
            _placementManager.SetBlueprint(_blueprint);
        }

        public override void UpdateState()
        {
            _placementManager.Update();
        }

        protected override void OnConfirmed()
        {
            Debug.Log("ON CONFIRMED PLACEMENT");
            var blueprint = _placementManager.GetBlueprint();
            if (blueprint == null) return;
            
            var newModel = Object.Instantiate(_blueprint, blueprint.transform.position, blueprint.transform.localRotation);
            Object.Destroy(blueprint);
            newModel.transform.parent = Services.Get<ModelManager>().container.transform;
            newModel.AddComponent<BoxCollider>();
        }

        protected override void OnCancelled()
        {
            
        }

        public override void ExitState()
        {
            AlternativeEventChannel.AlternativeChanged -= OnAlternativeChanged;
            AlternativeEventChannel.CategoryChanged -= OnCategoryChanged;
            StateEventChannel.Confirmed -= OnConfirmed;
            UIEventChannel.UIChanged -= SetUIReference;
            if (_ui != null) _ui.ClearData();
        }
    }
}
