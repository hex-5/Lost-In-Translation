using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodManager : MonoBehaviour
{
    [SerializeField]
    private bool isOurSupervisor;

    private float currentMoodScore;
    private Slider moodSlider;
    private float initialMoodScore;

    [SerializeField]
    private float angerThreshold;

    public float neutralValue;
    public float insultingValue;
    public float flatteringValue;
    public float challengingValue;

    private void Start()
    {
        moodSlider = GetComponent<Slider>();
        initialMoodScore = moodSlider.value;
    }

    public void AdjustMood(ScoreEvaluator.ConnotationsCount connotations)
    {
        currentMoodScore += connotations.neutralCount * neutralValue;
        currentMoodScore += connotations.insultingCount * insultingValue;
        currentMoodScore += connotations.flatteringCount * flatteringValue;
        currentMoodScore += connotations.challengingCount * challengingValue;

        moodSlider.value = currentMoodScore;

        if (currentMoodScore < angerThreshold)
        {
            if (isOurSupervisor)
            {
                // GameManager EndCurrentCycle Bad Ending 1
            }
            else
            {
                // GameManager EndCurrentCycle Bad Ending 2
            }
        }
    }

    public void ResetMoodScore()
    {
        moodSlider.value = initialMoodScore;
    }
}