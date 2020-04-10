using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public ProgressBar ProgressBarObject;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().onGameCycleUpdated += OnGameCycleUpdated;
    }

    private void OnGameCycleUpdated(GameManager manager, GameManager.GameCycle cycle)
    {
        ProgressBarObject.Progress = 1 - (cycle.Countdown / manager.SecondsPerCycle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
