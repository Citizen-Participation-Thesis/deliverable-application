using EventChannels;
using Selection;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace States
{
    public class PhotoState : State
    {
        private readonly SelectionManager _selectionManager;

        public PhotoState() {}
        public PhotoState(StateManager stateManager) : base(stateManager) { }

        ~PhotoState()
        {
            StateEventChannel.Confirmed -= OnConfirmed;
            StateEventChannel.Cancelled -= OnCancelled;
        }
        
        public override void EnterState()
        {
            StateEventChannel.Confirmed += OnConfirmed;
            StateEventChannel.Cancelled += OnCancelled;
        }

        public override void UpdateState()
        {
            
        }

        protected override void OnConfirmed()
        {
            Object.FindObjectOfType<ModeScrollView>().gameObject.SetActive(false);
            Services.Get<ScreenshotManager>().TakeScreenshot();
        }

        protected override void OnCancelled()
        {
            
            // StateManager.ChangeState(new ModeAlternativeState(StateManager));
        }

        public override void ExitState()
        {
            StateEventChannel.Confirmed -= OnConfirmed;
            StateEventChannel.Cancelled -= OnCancelled;
        }
    }
}
