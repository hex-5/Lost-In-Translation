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
        GameObject WordPrefab = null;
        [SerializeField]
        ConversationScriptableObject[] Conversations = null;
        [SerializeField]
        List<Material> ConnotationMats = null;
        [SerializeField]
        Transform Leader1Pos = null;
        [SerializeField]
        Transform Leader2Pos = null;
        [SerializeField]
        AnimationCurve WordScale = null;
        [SerializeField]
        AnimationCurve WordMove = null;
        [SerializeField]
        float initialWordScale = 0F;
        [SerializeField]
        float targetWordScale = 1F;
        [SerializeField]
        float time = 0.5f;
        [SerializeField]
        Transform[] SpeechBubblePositions = null;
        [SerializeField]
        Transform[] AngrySpeechBubblePositions = null;

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
            SpawnLeaders(Conversations[currentConversation]);
            initialWordScaleVector = new Vector3(initialWordScale, initialWordScale, initialWordScale);
            targetWordScaleVector = new Vector3(targetWordScale, targetWordScale, targetWordScale);
        }

        void Update()
        {
        }

        private Transform GetDeepestChild(Transform parent)
        {
            if (parent.childCount > 0)
                return (GetDeepestChild(parent.GetChild(0)));
            else
                return parent;
        }

        private Word SpawnWord(Connotation connotation, GameObject spritePrefab, Transform parent, Transform spawnPosition, bool isEssential)
        {
            Word wordComponent = Instantiate(WordPrefab, parent, true).GetComponent<Word>();
            wordComponent.transform.position = spawnPosition.GetChild(0).transform.position;
            wordComponent.gameObject.transform.localScale = Vector3.one;

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
                    spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Neutral"));
                    break;
                case Connotation.Insulting:
                    spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Insulting"));
                    break;
                case Connotation.Flattering:
                    spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Flattering"));
                    break;
                case Connotation.Challenging:
                    spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Challenging"));
                    break;
                default:
                    spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Neutral"));
                    break;
            }

            if (isEssential)
                spriteRenderer.material = ConnotationMats.Find(x => x.name.Contains("Essential"));

            return wordComponent;
        }

        public void ConvertConversation(ConversationScriptableObject obj)
        {
            //Spawn Words
            wordList.Clear();
            Transform wordPos = GetDeepestChild(GameObject.Find("BubbleController").GetComponent<BubbleController>().ActiveBubble.transform);
            for (int i = 0; i < obj.conversationData.Length; i++)
            {
                wordList.Add(Instance.SpawnWord(obj.conversationData[i].connotation, obj.conversationData[i].spritePrefab, wordPos, speaker.transform, obj.conversationData[i].isEssential));
                wordList[i].spritePrefab.GetComponent<SpriteRenderer>().enabled = false;
            }
            //GetComponent<GameManager>().TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue = GetComponent<GameManager>().SecondsPerCycle / wordList.Count;
        }

        public void SpawnLeaders(ConversationScriptableObject obj)
        {
            //Spawn Leaders
            speaker = Instantiate(obj.speaker, Leader1Pos);
            listener = Instantiate(obj.listener, Leader2Pos);
        }

        public bool SpawnWord()
        {
            if (wordList.Count > 0 && currentWord < wordList.Count)
            {
                wordList[currentWord].spritePrefab.GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(ScaleWord(wordList[currentWord]));
                StartCoroutine(MoveWord(wordList[currentWord]));
                return true;
            }
            else
            {
                //Debug.Log("Every Word was spawned // No Word in Wordlist");
                return false;
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
            Vector3 targetPosition = SelectionFreeBubbleSpace(GameObject.Find("BubbleController").GetComponent<BubbleController>().ActiveBubble.transform).position;
            while (i < 1)
            {
                i += rate * Time.deltaTime;
                word.transform.position = Vector3.Lerp(speaker.transform.GetChild(0).transform.position, targetPosition, WordMove.Evaluate(i));
                yield return 0;
            }
        }

        private Transform SelectionFreeBubbleSpace(Transform activeBubble)
        {
            int RandomOption = 0;
            if (activeBubble.name.Contains("simple"))
            {
                RandomOption = UnityEngine.Random.Range(0, SpeechBubblePositions.Length);
                return SpeechBubblePositions[RandomOption];
            }
            else if (activeBubble.name.Contains("angry"))
            {
                RandomOption = UnityEngine.Random.Range(0, AngrySpeechBubblePositions.Length);
                return AngrySpeechBubblePositions[RandomOption];
            }
            else
            {
                Debug.LogError("No Speechbubble Existing!");
                return null;
            }
        }

        public bool CheckNextConversation()
        {
            if (currentConversation + 1 < Conversations.Length)
                return true;
            else return false;
        }

        public void NextConversation()
        {
            ResetWords();
            currentConversation++;
            currentWord = 0;
            ConvertConversation(Conversations[currentConversation]);
        }

        public void RestartConversaitons()
        {
            ResetWords();
            currentConversation = 0;
            currentWord = 0;
            ConvertConversation(Conversations[currentConversation]);
        }

        public void ResetWords()
        {
            if (wordList.Count > 0)
            {
                foreach (Word word in wordList)
                {
                    if(word != null)
                        Destroy(word.gameObject);
                        //StartCoroutine(DissolveWord(word.gameObject));
                }
                wordList.Clear();
            }
        }

        public void ResetLeaders()
        {
            if (speaker)
                Destroy(speaker);
            if (listener)
                Destroy(listener);
        }

        //private IEnumerator DissolveWord(GameObject word)
        //{
        //    if (!word)
        //        yield break;
        //    Vector3 initialScale = word.transform.localScale;
        //    float i = 0;
        //    float rate = 1 / 0.3f;
        //    while (i < 1)
        //    {
        //        i += rate * Time.deltaTime;
        //        word.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, WordScale.Evaluate(i));
        //        yield return 0;
        //    }
        //    Destroy(word.gameObject);
        //    yield return 0;
        //}
    }
}
