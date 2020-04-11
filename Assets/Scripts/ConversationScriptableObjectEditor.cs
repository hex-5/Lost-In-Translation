using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConversationScriptableObject))]
public class ConversationScriptableObjectEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (ConversationScriptableObject)target;

        GUILayout.Space(20);
        if (GUILayout.Button("I can extend the Editor well.", GUILayout.Height(20)))
        {
            script.DontPressMe();
        }

    }
}
#endif