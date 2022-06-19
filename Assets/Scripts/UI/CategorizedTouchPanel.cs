using System;
using EventChannels;
using Models;
using UnityEngine;

namespace UI
{
    public class CategorizedTouchPanel : MonoBehaviour
    {
        [SerializeField] private ThumbnailRow categoriesRow;
        [SerializeField] private ThumbnailRow alternativesRow;
        
        private ICategoryData _categoryData;

        private void OnDisable()
        {
            if (_categoryData == null) return;
            _categoryData.AlternativeSet -= SetCurrentlySelected;
            _categoryData.CategorySet -= SetAlternatives;
        }

        private void HideRows()
        {
            Debug.Log("\n\nHIDING THE ROWS\n\n");
            categoriesRow.content.SetActive(false);
            categoriesRow.title.gameObject.SetActive(false);
            alternativesRow.content.SetActive(false);
            alternativesRow.title.gameObject.SetActive(false);
        }

        private void ShowRows()
        {
            // SetActive on the whole row gameobject
            Debug.Log("\n\nSHOWING THE ROWS\n\n");
            categoriesRow.content.SetActive(true);
            categoriesRow.title.gameObject.SetActive(true);
            alternativesRow.content.SetActive(true);
            alternativesRow.title.gameObject.SetActive(true);
        }

        public void SetData(ICategoryData categoryData)
        {
            if (_categoryData != null) ClearData();

                _categoryData = categoryData;
            _categoryData.CategorySet += FillRows;
            _categoryData.AlternativeSet += SetCurrentlySelected;
            ShowRows();
            FillRows();
            SetCurrentlySelected();
        }

        public void ClearData()
        {
            Debug.Log("Entering CTP Clear Data");
            ClearRows();
            HideRows();
            if (_categoryData == null) return;
            Debug.Log("CTP: Category data was not null!");
            _categoryData.AlternativeSet -= SetCurrentlySelected;
            _categoryData.CategorySet -= FillRows;
            _categoryData = null;
        }

        private void FillRows()
        {
            Debug.Log("FILLING CATEGORY ROW");
            _categoryData.GetCategories().ForEach(c => Debug.Log($"\t{c}"));
            categoriesRow.Clear();
            categoriesRow.Fill(_categoryData.GetCategories(), AlternativeEventChannel.RaiseCategoryChangedEvent);
            SetAlternatives();
        }

        private void ClearRows()
        {
            Debug.Log($"CLEARING ROWS \n\t CATEGORIES ROW: {categoriesRow != null} \n\t ALTERNATIVES ROW: {alternativesRow != null}");
            categoriesRow.Clear();
            alternativesRow.Clear();
        }

        private void SetCurrentlySelected()
        {
            var currentCategoryIndex = _categoryData.GetCurrentCategoryIndex();
            categoriesRow.HighlightThumbnail(currentCategoryIndex);
            var currentAlternativeIndex = _categoryData.GetCurrentAlternativeIndex();
            alternativesRow.HighlightThumbnail(currentAlternativeIndex);
        }

        private void SetAlternatives()
        {
            alternativesRow.title.SetText("Alternativer for " + _categoryData.GetCategories()[_categoryData.GetCurrentCategoryIndex()]);
            Debug.Log("FILLING ALTERNATIVE ROW");
            _categoryData.GetAlternatives().ForEach(c => Debug.Log($"\t{c}"));
            alternativesRow.Clear();
            alternativesRow.Fill(_categoryData.GetAlternatives(), AlternativeEventChannel.RaiseAlternativeChangedEvent);
            SetCurrentlySelected();
        }
    }
}