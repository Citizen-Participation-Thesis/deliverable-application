using System;
using System.Threading.Tasks;
using EventChannels;
using States;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI
{
    public class UIManager: MonoBehaviour
    {
        [SerializeField] private UIAreaContentSetter middle;

        private void OnEnable()
        {
            StateEventChannel.StateChanged += SetUIForNewState;
        }

        private void OnDestroy()
        {
            StateEventChannel.StateChanged -= SetUIForNewState;
        }

        private void SetUIForNewState(State state)
        {
            middle.SetContent(state.Data.middleContent);
        }
    }
}