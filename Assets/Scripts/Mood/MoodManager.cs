using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodManager : MonoBehaviour
{
    [SerializeField]
    private bool isOurLeader = true;

    private float currentMoodScore;
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
    private Words.WordManager wordManager;

    [SerializeField]
    private float interpolationSpeed = 1;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        wordManager = Words.WordManager.Instance;

        moodSlider = GetComponent<Slider>();
        initialMoodScore = moodSlider.value;
        currentMoodScore = initialMoodScore;
    }

    public void AdjustMood(ScoreEvaluator.ConnotationsCount connotations, int essentialsInside)
    {
        int essentialsOutside = 0;

        foreach (Words.Word w in wordManager.wordList)
        {
            if (w.isEssential)
            {
                essentialsOutside++;
            }
        }
        essentialsOutside -= essentialsInside;

        currentMoodScore += connotations.neutralCount * neutralValue;
        currentMoodScore += connotations.insultingCount * insultingValue;
        currentMoodScore += connotations.flatteringCount * flatteringValue;
        currentMoodScore += connotations.challengingCount * challengingValue;
        currentMoodScore += essentialsOutside * unusedEssentialPenaltyValue;

        StartCoroutine("SliderInterpolation");

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
                gameManager.EndCurrentCycle(GameManager.RESULTS.BAD_ENDING_1);
            }
            else
            {
                currentMoodScore = 0;
                gameManager.EndCurrentCycle(GameManager.RESULTS.BAD_ENDING_2);
            }

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