using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIController : MonoBehaviour
{

    public ProgressBar ProgressBarObject;
    public UnityEngine.UI.Text TimeRemainingTextObject;

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
        ProgressBarObject.Progress = 1 - (cycle.Countdown / manager.SecondsPerCycle);
        TimeRemainingTextObject.text = $"{cycle.Countdown % 60:00.0}s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
