using UnityEngine;

namespace UI
{
    public class DeleteTouchPanel : MonoBehaviour
    {
        [SerializeField] private ConfirmButton confirmButton;
        [SerializeField] private CancelButton cancelButton;

        public void ShowButtons()
        { 
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }

        public void HideButtons()
        {
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }
    }
}