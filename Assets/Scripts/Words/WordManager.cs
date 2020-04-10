using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Words
{
    public enum Connotation
    {
        Neutral,
        Insulting,
        Flattering,
        Challenging,
    }

    public class WordManager : MonoBehaviour
    {
        public static WordManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [SerializeField]
        GameObject WordPrefab;

        [SerializeField]
        ConversationScriptableObject[] Conversations;

        private int conversationIndex = 0;
        private GameObject mainCanvas;
        private Vector3 wordSpawnPosition;
        private GameObject speaker;

        void Start()
        {
            mainCanvas = GameObject.Find("Main_Canvas");
        }

        void Update()
        {
        }

        private Word SpawnWord(Connotation connotation, GameObject spritePrefab, Vector3 position, bool isEssential)
        {
            Word wordComponent = Instantiate(WordPrefab, mainCanvas.transform).GetComponent<Word>();
            wordComponent.transform.position = position;

            // Set Word Parameters
            wordComponent.connotation = connotation;
            wordComponent.isEssential = isEssential;
            GameObject wordSprite = Instantiate(spritePrefab, wordComponent.transform);

            // Adjust Word-Shape
            SpriteRenderer spriteRenderer = wordSprite.GetComponent<SpriteRenderer>();

            switch (connotation)
            {
                case Connotation.Neutral:
                    spriteRenderer.color = Color.blue;
                    break;
                case Connotation.Insulting:
                    spriteRenderer.color = Color.red;
                    break;
                case Connotation.Flattering:
                    spriteRenderer.color = Color.magenta;
                    break;
                case Connotation.Challenging:
                    spriteRenderer.color = Color.yellow;
                    break;
                default:
                    spriteRenderer.color = Color.white;
                    break;
            }
            return wordComponent;
        }

        public void ConvertConversationToWords(ConversationScriptableObject obj)
        {
            Word[] words = new Word[obj.conversationData.Length];
            speaker = obj.speaker;

            for (int i = 0; i < obj.conversationData.Length; i++)
            {
                words[i] = Instance.SpawnWord(obj.conversationData[i].connotation, obj.conversationData[i].spritePrefab, obj.speaker.transform.GetChild(0).transform.position, obj.conversationData[i].isEssential);
            }
        }


    }
}
