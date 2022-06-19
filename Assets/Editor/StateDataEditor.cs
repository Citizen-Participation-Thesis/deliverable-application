/*
#if UNITY_EDITOR
using ScriptableObjects.StateData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(StateData))]
    public class StateDataEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var myScript = target as StateData;

            if (myScript == null) return;
            myScript.isModeEntrypoint = GUILayout.Toggle(myScript.isModeEntrypoint, "Is Mode Entrypoint");

            if (myScript.isModeEntrypoint)
                myScript.componentClassName = EditorGUILayout.TextField(myScript.componentClassName);
        }
    }
}
#endif*/