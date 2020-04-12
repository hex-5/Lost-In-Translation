using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Words;

public class MoodManager : MonoBehaviour
{
    [SerializeField]
    private bool isOurLeader = true;

    public float currentMoodScore;
    private Slider moodSlider;
    private float initialMoodScore;

    [SerializeField, Tooltip("When Mood sinks under this value the leader will be angry")]
    private float angerThreshold = 40;

    public float neutralValue;
    public float insultingValue;
    public float flatteringValue;
    public float challengingValue;
    public float unusedEssentialPenaltyValue;

    private GameManager gameManager;

    [SerializeField]
    private float interpolationSpeed = 1;
    [SerializeField]
    private float wordDissolveSpeed = 1F;

    private int essentialsOutside = 0;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        moodSlider = GetComponent<Slider>();
        initialMoodScore = moodSlider.value;
        currentMoodScore = initialMoodScore;
    }

    //public void AdjustMood(ScoreEvaluator.ConnotationsCount connotations, int essentialsInside)
    //{
    //    int essentialsOutside = 0;

    //    foreach (Words.Word w in wordManager.wordList)
    //    {
    //        if (w.isEssential)
    //        {
    //            essentialsOutside++;
    //        }
    //    }
    //    essentialsOutside -= essentialsInside;

    //    currentMoodScore += connotations.neutralCount * neutralValue;
    //    currentMoodScore += connotations.insultingCount * insultingValue;
    //    currentMoodScore += connotations.flatteringCount * flatteringValue;
    //    currentMoodScore += connotations.challengingCount * challengingValue;
    //    currentMoodScore += essentialsOutside * unusedEssentialPenaltyValue;

    //    StartCoroutine("SliderInterpolation");

    //    if (currentMoodScore < angerThreshold)
    //    {
    //        (isOurLeader ? gameManager.leader1Pos : gameManager.leader2Pos).GetComponentInChildren<Leader>().IsAngry = true;
    //    }
    //    else
    //    {
    //        (isOurLeader ? gameManager.leader1Pos : gameManager.leader2Pos).GetComponentInChildren<Leader>().IsAngry = false;
    //    }

    //    if (currentMoodScore < 1)
    //    {
    //        if (isOurLeader)
    //        {
    //            currentMoodScore = 0;
    //            gameManager.EndGame(GameManager.RESULTS.BAD_ENDING_1);
    //        }
    //        else
    //        {
    //            currentMoodScore = 0;
    //            gameManager.EndGame(GameManager.RESULTS.BAD_ENDING_2);
    //        }

    //    }
    //}

    public void AdjustMoodOnce(Word w, bool isInside, bool isEssential)
    {
        bool setDirty = false;
        if (isOurLeader)
        {
            if (isEssential && !isInside)
            {
                currentMoodScore += unusedEssentialPenaltyValue;
                setDirty = true;
            }
        }
        else
        {
            if (isInside)
            {
                switch (w.connotation)
                {
                    case Connotation.Challenging:
                        currentMoodScore += challengingValue;
                        break;
                    case Connotation.Insulting:
                        currentMoodScore += insultingValue;
                        break;
                    case Connotation.Flattering:
                        currentMoodScore += flatteringValue;
                        break;
                    case Connotation.Neutral:
                        currentMoodScore += neutralValue;
                        break;
                }
                setDirty = true;
            }
        }
        
        if (setDirty)
        {
            StartCoroutine("SliderInterpolation");
            StartCoroutine(MoveWordToLeader(w, (isOurLeader ? gameManager.leader1Pos : gameManager.leader2Pos)));

            if (currentMoodScore < angerThreshold)
            {
                (isOurLeader ? gameManager.leader1Pos : gameManager.leader2Pos).GetComponentInChildren<Leader>().IsAngry = true;
            }
            else
            {
                (isOurLeader ? gameManager.leader1Pos : gameManager.leader2Pos).GetComponentInChildren<Leader>().IsAngry = false;
            }

            if (currentMoodScore < 1)
            {
                if (isOurLeader)
                {
                    currentMoodScore = 0;
                    gameManager.EndGame(GameManager.RESULTS.BAD_ENDING_1);
                }
                else
                {
                    currentMoodScore = 0;
                    gameManager.EndGame(GameManager.RESULTS.BAD_ENDING_2);
                }
            }
        }
    }

    private IEnumerator MoveWordToLeader(Word word, Transform leaderBuzzer)
    {
        float i = 0;
        float rate = 1 / wordDissolveSpeed;
        Vector3 targetPosition = leaderBuzzer.position;
        Vector3 initialPosition = word.transform.position;
        Vector3 initialScale = word.transform.localScale;

        while (i < 1)
        {
            i += rate * Time.deltaTime;
            word.transform.position = Vector3.Lerp(initialPosition, targetPosition, AnimationCurve.EaseInOut(0.0F,0.0F,1.0F,1.0F).Evaluate(i));
            word.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, AnimationCurve.EaseInOut(0.0F, 0.0F, 1.0F, 1.0F).Evaluate(i));
            yield return 0;
        }
    }

    // For smooth movement of the finger from the old slider value to the new slider value.
    IEnumerator SliderInterpolation()
    {
        float tmp = moodSlider.value;
        float interpolationTime = 0;

        while (interpolationTime <= 1)
        {
            moodSlider.value = Mathf.Lerp(tmp, currentMoodScore, interpolationTime);
            interpolationTime += Time.deltaTime * interpolationSpeed;

            yield return new WaitForEndOfFrame();
        }
        moodSlider.value = currentMoodScore;
    }

    public void ResetMoodScore()
    {
        moodSlider.value = initialMoodScore;
        currentMoodScore = moodSlider.value;
    }
}