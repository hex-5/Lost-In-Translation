using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Words;

public class ScoreEvaluator : MonoBehaviour
{
    public class ConnotationsCount
    {
        public int neutralCount;
        public int insultingCount;
        public int flatteringCount;
        public int challengingCount;
    }

    public delegate void OnEvaluationFinshed(GameManager.RESULTS result);
    public OnEvaluationFinshed onEvaluationFinshed;

    [Tooltip("Edge collider sitting on a different GameObject, e.g. on a child.")]
    public EdgeCollider2D edgeCollider;

    // List of Colliders that are inside our object
    private List<Collider2D> collidersInside = new List<Collider2D>();

    public MoodManager moodManagerLeader1;
    public MoodManager moodManagerLeader2;

    [SerializeField]
    private float TimeBetweenEvaluations = 1.0F;

    private void Start()
    {
        // Filling the edge collider with the same points as our polygon collider. First point has to be added twice to receive a closed edge.

        PolygonCollider2D polyCollider = GetComponent<PolygonCollider2D>();
        
        Vector2[] tmpVec2 = new Vector2[polyCollider.points.Length + 1];

        for (int i = 0; i < polyCollider.points.Length; i++)
        {
            tmpVec2[i] = polyCollider.points[i];
        }
        tmpVec2[tmpVec2.Length - 1] = tmpVec2[0];
        
        edgeCollider.points = tmpVec2;


        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.onStartEvaluation += OnStartEvaluation;
        }
        else
        {
            Debug.LogWarning("ScoreEvaluator: Could not find GameManager.");
        }
    }

    //public void CountBlocksInside()
    //{
    //    ConnotationsCount connotationsCount = new ConnotationsCount();
    //    int essentialsInside = 0;

    //    // Go through all objects that are within my trigger
    //    foreach (Collider2D c in collidersInside)
    //    {
    //        // If they are not on the edge of my trigger, increment connotation values
    //        if (!Physics2D.IsTouching(c, edgeCollider))
    //        {
    //            Word word = c.GetComponentInParent<Word>();
    //            if (word.isEssential) essentialsInside++;

    //            switch (word.connotation)
    //            {
    //                case Connotation.Neutral:
    //                    connotationsCount.neutralCount++;
    //                    break;
    //                case Connotation.Insulting:
    //                    connotationsCount.insultingCount++;
    //                    break;
    //                case Connotation.Flattering:
    //                    connotationsCount.flatteringCount++;
    //                    break;
    //                case Connotation.Challenging:
    //                    connotationsCount.challengingCount++;
    //                    break;
    //            }
    //        }
    //    }

    //    collidersInside.Clear();

    //    moodManagerLeader1.AdjustMood(connotationsCount, essentialsInside);
    //    moodManagerLeader2.AdjustMood(connotationsCount, essentialsInside);
    //}
    

    private IEnumerator EvaluateScore(GameManager.RESULTS result)
    {
        //WaitForSeconds wait = new WaitForSeconds(TimeBetweenEvaluations);
        bool inside = false;
        foreach (Word w in WordManager.Instance.wordList)
        {
            Debug.Log("Word: " + w.transform.GetChild(0).name);
            // Every Word
            foreach(Collider2D coll in collidersInside)
            {
                if (coll.gameObject == w.transform.GetChild(0).gameObject)
                {
                    inside = true;
                    break;
                }
            }
            if (inside)
            {
                // Word Inside Box
                if (!Physics2D.IsTouching(w.transform.GetChild(0).GetComponent<Collider2D>(), edgeCollider))
                {
                    //Word Inside not on Line
                    AdjustMoodForLeaders(w, true, w.isEssential);
                }
                else
                {
                    //Word "Outside" on Line
                    AdjustMoodForLeaders(w, false, w.isEssential);
                }
            }
            else
            {
                // Word not in Box
                AdjustMoodForLeaders(w, false, w.isEssential);
            }
            inside = false;
            yield return new WaitForSeconds(1F);
        }
        //while (WordManager.Instance.wordList[WordManager.Instance.wordList.Count-1].transform.localScale != Vector3.zero)
        //    yield return new WaitForSeconds(1F);
        collidersInside.Clear();
        onEvaluationFinshed(result);
    }
        
    

    private void AdjustMoodForLeaders(Word word, bool isInside, bool isEssential)
    {
        moodManagerLeader1.AdjustMoodOnce(word, isInside, isEssential);
        moodManagerLeader2.AdjustMoodOnce(word, isInside, isEssential);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word") && !collidersInside.Contains(collision))
        {
            collidersInside.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word") && collidersInside.Contains(collision))
        {
            collidersInside.Remove(collision);
        }
    }


    private void OnStartEvaluation(GameManager.RESULTS result)
    {
        if(result == GameManager.RESULTS.GOOD)
        {
            StartCoroutine(EvaluateScore(result));
        }
    }
}
