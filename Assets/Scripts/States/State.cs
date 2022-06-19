using ScriptableObjects.StateData;

namespace States
{
    public abstract class State
    {
        public StateData Data { get; set; }

        protected readonly StateManager StateManager;

        protected State() {}
        protected State(StateManager stateManager)
        {
            StateManager = stateManager;
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public virtual void UpdateState() {}

        protected abstract void OnConfirmed();
        protected abstract void OnCancelled();
    }
}
