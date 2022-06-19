using System;
using EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public class UIAreaContentSetter : MonoBehaviour
    {
        private GameObject _currentContent;

        public void SetContent(AssetReference reference)
        {
            if (!reference.RuntimeKeyIsValid())
            {
                if (_currentContent != null) Destroy(_currentContent);
                return;
            }
            
            var uiLoadingHandle = reference.InstantiateAsync(transform);
            
            uiLoadingHandle.Completed += operation =>
            {
                switch (operation.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        if (_currentContent != null) Destroy(_currentContent);
                        _currentContent = operation.Result;
                        UIEventChannel.RaiseUIChangedEvent(_currentContent);
                        break;
                    case AsyncOperationStatus.Failed:
                        break;
                    case AsyncOperationStatus.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
    }
}
