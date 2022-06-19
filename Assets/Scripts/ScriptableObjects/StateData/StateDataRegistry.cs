using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.StateData
{
    [CreateAssetMenu(menuName = "Data/StateDataRegistry")]
    public class StateDataRegistry: ScriptableObject
    {
        public List<StateData> stateData;
    }
}