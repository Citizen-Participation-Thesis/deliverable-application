using System.Collections.Generic;
using System.Linq;
using Data;
using EventChannels;
using ScriptableObjects.StateData;
using UnityEngine;

namespace States
{
    public class StateManager : Singleton<StateManager>
    {
        public List<StateData> stateData;
        public State CurrentState { get; private set; }
        protected internal StateData PreviousStateData { get; private set; }

        public void SetInitialState()
        {
            stateData = Services.Get<DataManager>().GetStateData();
            ChangeState(new SwapperState(this));
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

        public void ChangeState(State newState)
        {
            if (CurrentState != null)
            {
                Debug.Log("Exiting state " + CurrentState.Data.modeName);
                CurrentState.ExitState();
                Debug.Log("Exiting completed!");
                PreviousStateData = CurrentState.Data;
            }
            
            CurrentState = newState;
            CurrentState.Data = stateData.Find(state => CurrentState.GetType().Name == state.className);
            Debug.Log("Entering state " + CurrentState.Data.className);
            CurrentState.EnterState();
            StateEventChannel.RaiseStateChangedEvent(CurrentState);
        }

        public void ChangeState(string stateName)
        {
            var state = stateData.First(data => data.modeName == stateName).GetFreshState(this);
            ChangeState(state);
        }
    }
}
