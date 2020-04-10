using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public float SecondsPerCycle { get; } = 10.0f;
<<<<<<< HEAD
    [SerializeField] float WordsPerSecond = 0.33333f;

=======
    [SerializeField] float TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue = 0.33333f;
    [SerializeField] GameObject WordSpawner = null;
>>>>>>> aab00f0ebd811340e2753a20b6c3e2f752074210
    public enum RESULTS
    {
        GOOD,
        BAD_ENDING_1,
        BAD_ENDING_2
    }
    //[SerializeField] ...

    public class GameCycle
    {
        public float Countdown
        {
            get; set;
        }
        public float WordCountdown
        {
            get; set;
        }
    }

    public delegate void GameCycleDelegate(GameManager manager, GameCycle cycle);

    GameCycle currentCycle;
    public GameCycleDelegate onGameCycleUpdated;

    public void StartNewCycle()
    {
        if(currentCycle == null)
        {
            currentCycle = new GameCycle();
            currentCycle.Countdown = SecondsPerCycle;
            currentCycle.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
            //currentCycle.Points = 0;
            onGameCycleUpdated(this, currentCycle);
        }
        //Todo: Reset everything for a new cycle

        Debug.Log("Resetted everything and started a new cycle with new points.");
    }
    public void StartNextCycle()
    {
        //only reset countdown
        currentCycle.Countdown = SecondsPerCycle;
        currentCycle.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
        //Todo: reset words
        WordManager.Instance.NextConversation();
        Debug.Log("Resetted words and started a new cycle with existing points. [todo: getPointsFromSomewhere()]");
        onGameCycleUpdated(this, currentCycle);
    }
    public void EndCurrentCycle(RESULTS result)
    {
        Debug.Log("Cycle ended with [todo: getPointsFromSomewhere()] Points.");
        switch (result)
        {
            case RESULTS.BAD_ENDING_1:
            case RESULTS.BAD_ENDING_2:
                //Todo: Script Endings and reset to start
            case RESULTS.GOOD:
            default:
                StartNextCycle();
                break;
        }
    }
    public void UpdateRunningCycle()
    {
        currentCycle.Countdown -= Time.unscaledDeltaTime;
        if(currentCycle.Countdown < 0)
        {
            //countdown is over, cycle isnt stopped anywhere else in the game, so it was successful.
            EndCurrentCycle(RESULTS.GOOD);
        }
        currentCycle.WordCountdown -= Time.unscaledDeltaTime;
        if (currentCycle.WordCountdown < 0)
        {
            currentCycle.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
            SpawnWord();
        }

        onGameCycleUpdated(this, currentCycle);
    }

    public void SpawnWord()
    {
        Debug.Log("Spawned new word!");
        WordManager.Instance.SpawnWord();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewCycle();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRunningCycle();
    }
}
