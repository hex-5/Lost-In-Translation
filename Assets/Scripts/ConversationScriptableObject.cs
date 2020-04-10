using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConversationScriptableObject", order = 1)]
public class ConversationScriptableObject : ScriptableObject
{
    [Serializable]
    public struct ConversationData
    {
        public Connotation connotation;
        public GameObject spritePrefab;
        public bool isEssential;
    }

    public ConversationData[] conversationData;
    public GameObject speaker;
    public GameObject listener;

    public void DontPressMe()
    {
        for(int i = 0; i<10; i++)
        {
            Debug.LogError("Hi <3 - " + i);
        }
    }
}
