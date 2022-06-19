using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScriptableObjects.StateData
{
    [CreateAssetMenu(menuName = "Data/StateData")]
    public class StateData: ScriptableObject
    {
        public bool isModeEntrypoint;
        public string className;
        public string description;
        public string modeName;
        public string componentClassName;
        public AssetReference topContent;
        public AssetReference middleContent;
        public AssetReference sprite;
    }
}