using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConversationScriptableObject))]
public class ConversationScriptableObjectEditor : Editor
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
