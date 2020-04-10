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
        [SerializeField]
        Transform Leader1Pos;
        [SerializeField]
        Transform Leader2Pos;
        [SerializeField]
        AnimationCurve WordScale;
        [SerializeField]
        AnimationCurve WordMove;
        [SerializeField]
        float initialWordScale = 0F;
        [SerializeField]
        float targetWordScale = 1F;
        [SerializeField]
        float time = 0.5f;
        [SerializeField]
        Transform[] SpeechBubblePositions;
        
        private int currentConversation = 0;
        public List<Word> wordList = new List<Word>();
        private int currentWord = 0;
        private Vector3 wordSpawnPosition;
        private GameObject speaker;
        private GameObject listener;
        private Vector3 initialWordScaleVector;
        private Vector3 targetWordScaleVector;

        void Start()
        {
            ConvertConversation(Conversations[currentConversation]);
            initialWordScaleVector = new Vector3(initialWordScale, initialWordScale, initialWordScale);
            targetWordScaleVector = new Vector3(targetWordScale, targetWordScale, targetWordScale);
        }

        void Update()
        {
        }

        private Word SpawnWord(Connotation connotation, GameObject spritePrefab, GameObject parent, bool isEssential)
        {
            Word wordComponent = Instantiate(WordPrefab).GetComponent<Word>();
            wordComponent.transform.position = parent.transform.GetChild(0).transform.position;

            // Set Word Parameters
            wordComponent.connotation = connotation;
            wordComponent.isEssential = isEssential;
            GameObject wordSprite = Instantiate(spritePrefab, wordComponent.transform);
            wordComponent.spritePrefab = wordSprite;

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

        public void ConvertConversation(ConversationScriptableObject obj)
        {
            //Spawn Leaders
            speaker = Instantiate(obj.speaker, Leader1Pos);
            listener = Instantiate(obj.listener, Leader2Pos);

            //Spawn Words
            wordList.Clear();
            for (int i = 0; i < obj.conversationData.Length; i++)
            {
                wordList.Add(Instance.SpawnWord(obj.conversationData[i].connotation, obj.conversationData[i].spritePrefab, speaker, obj.conversationData[i].isEssential));
                wordList[i].spritePrefab.GetComponent<SpriteRenderer>().enabled = false;
            }
            GetComponent<GameManager>().TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue = GetComponent<GameManager>().SecondsPerCycle / wordList.Count;
        }

        public void SpawnWord()
        {
            if (wordList.Count > 0 && currentWord < wordList.Count)
            {
                wordList[currentWord].spritePrefab.GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(ScaleWord(wordList[currentWord]));
                StartCoroutine(MoveWord(wordList[currentWord]));
            }
            else
            {
                Debug.Log("Every Word was spawned // No Word in Wordlist");
            }
        }
        
        private IEnumerator ScaleWord(Word word)
        {
            float i = 0;
            float rate = 1 / time;
            while (i < 1)
            {
                i += rate * Time.deltaTime;
                word.transform.localScale = Vector3.Lerp(initialWordScaleVector, targetWordScaleVector, WordScale.Evaluate(i));
                yield return 0;
            }
        }

        private IEnumerator MoveWord(Word word)
        {
            currentWord++;
            float i = 0;
            float rate = 1 / time;
            Vector3 targetPosition = SelectionFreeBubbleSpace().position;
            while (i < 1)
            {
                i += rate * Time.deltaTime;
                word.transform.position = Vector3.Lerp(speaker.transform.GetChild(0).transform.position, targetPosition, WordMove.Evaluate(i));
                yield return 0;
            }
        }

        private Transform SelectionFreeBubbleSpace()
        {
            int RandomOption = UnityEngine.Random.Range(0, SpeechBubblePositions.Length);
            return SpeechBubblePositions[RandomOption];
        }

        public bool CheckNextConversation()
        {
            if (currentConversation + 1 < Conversations.Length)
                return true;
            else return false;
        }

        public void NextConversation()
        {
            ResetWordsAndLeaders();
            currentConversation++;
            currentWord = 0;
            ConvertConversation(Conversations[currentConversation]);
        }

        public void RestartConversaitons()
        {
            ResetWordsAndLeaders();
            currentConversation = 0;
            currentWord = 0;
            ConvertConversation(Conversations[currentConversation]);
        }

        public void ResetWordsAndLeaders()
        {
            if (wordList.Count > 0)
            {
                foreach (Word word in wordList)
                {
                    StartCoroutine(DissolveWord(word.gameObject));
                }
            }
            if(speaker)
                Destroy(speaker);
            if (listener)
                Destroy(listener);
        }

        private IEnumerator DissolveWord(GameObject word)
        {
            if (!word)
                yield break;
            Vector3 initialScale = word.transform.localScale;
            float i = 0;
            float rate = 1 / 0.2f;
            while (i < 1)
            {
                i += rate * Time.deltaTime;
                word.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, WordScale.Evaluate(i));
                yield return 0;
            }
            Destroy(word.gameObject);
            yield return 0;
        }
    }
}
