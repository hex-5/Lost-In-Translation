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

        void Start()
        {
        }

        void Update()
        {
        }

        public Word SpawnWord(Connotation connotation, Sprite sprite, Transform position, bool isEssential)
        {
            Word wordComponent = Instantiate(WordPrefab, position).GetComponent<Word>();

            // Set Word Parameters
            wordComponent.connotation = connotation;
            wordComponent.isEssential = isEssential;
            // Adjust Word-Shape
            SpriteRenderer spriteRenderer = wordComponent.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

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

            for (int i = 0; i < obj.conversationData.Length; i++)
            {
                words[i] = Instance.SpawnWord(obj.conversationData[i].connotation, obj.conversationData[i].sprite, transform, obj.conversationData[i].isEssential);
            }
        }
    }
}
