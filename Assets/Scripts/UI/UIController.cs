using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIController : MonoBehaviour
{

    public ProgressBar ProgressBarObject;
    public UnityEngine.UI.Text TimeRemainingTextObject;

    public Color TimeProgressBadColor = Color.red;
    public Color TimeProgressGoodColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.onGameCycleUpdated += OnGameCycleUpdated;
        }
        else
        {
            Debug.LogWarning("UIController: Could not find GameManager.");
        }
    }

    private void OnGameCycleUpdated(GameManager manager, GameManager.GameCycle cycle)
    {
        float progress = 1 - (cycle.Countdown / manager.SecondsPerCycle);
        ProgressBarObject.Progress = progress;
        ProgressBarObject.ProgressBarColor = Color.Lerp(TimeProgressGoodColor, TimeProgressBadColor, progress*progress);
        TimeRemainingTextObject.text = $"{cycle.Countdown % 60:0.00}s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
