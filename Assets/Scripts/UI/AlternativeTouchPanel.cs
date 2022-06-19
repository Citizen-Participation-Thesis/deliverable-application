using EventChannels;
using Models;
using UnityEngine;

namespace UI
{
    public class AlternativeTouchPanel : MonoBehaviour
    {
        [SerializeField] private ThumbnailRow alternativesRow;
        private IAlternativeData _alternativeData;

        private void Start() => HideRows();

        private void OnDisable()
        {
            if (_alternativeData == null) return;
            _alternativeData.AlternativeSet -= SetCurrentlySelected;
        }

        private void HideRows()
        {
            alternativesRow.content.SetActive(false);
            alternativesRow.title.gameObject.SetActive(false);
        }

        private void ShowRows()
        {
            alternativesRow.content.SetActive(true);
            alternativesRow.title.gameObject.SetActive(true);
        }

        public void SetData(IAlternativeData alternativeData)
        {
            if (_alternativeData != null) ClearData();

            _alternativeData = alternativeData;
            _alternativeData.AlternativeSet += SetCurrentlySelected;
            ShowRows();
            FillRows();
            SetCurrentlySelected();
        }

        public void ClearData()
        {
            Debug.Log("Entering ATP clear children");
            ClearRows();
            HideRows();
            if (_alternativeData == null) return;
            _alternativeData.AlternativeSet -= SetCurrentlySelected;
            _alternativeData = null;
        }

        private void FillRows()
        {
            alternativesRow.Fill(_alternativeData.GetAlternatives(), AlternativeEventChannel.RaiseAlternativeChangedEvent);
        }

        private void ClearRows()
        {
            
            Debug.Log($"CLEARING ROW \n\t ALTERNATIVES ROW: {alternativesRow != null}");
            alternativesRow.Clear();
        }

        private void SetCurrentlySelected()
        {
            Debug.Log("Setting currently selected in alternative touch panel");
            var currentAlternativeIndex = _alternativeData.GetCurrentAlternativeIndex();
            alternativesRow.HighlightThumbnail(currentAlternativeIndex);
        }
    }
}