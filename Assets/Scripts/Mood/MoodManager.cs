using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodManager : MonoBehaviour
{
    [SerializeField]
    private bool isOurLeader;

    private int amountOfUnusedEssentials = 0;

    private float currentMoodScore;
    private Slider moodSlider;
    private float initialMoodScore;

    [SerializeField]
    private float angerThreshold;

    public float neutralValue;
    public float insultingValue;
    public float flatteringValue;
    public float challengingValue;
    public float unusedEssentialPenaltyValue;

    private GameManager gameManager;
    private Words.WordManager wordManager;

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

        moodSlider.value = currentMoodScore;

        if (currentMoodScore < angerThreshold)
        {
            if (isOurLeader)
            {
                gameManager.EndCurrentCycle(GameManager.RESULTS.BAD_ENDING_1);
            }
            else
            {
                gameManager.EndCurrentCycle(GameManager.RESULTS.BAD_ENDING_2);
            }
        }
    }

    public void ResetMoodScore()
    {
        moodSlider.value = initialMoodScore;
        amountOfUnusedEssentials = 0;
    }
}