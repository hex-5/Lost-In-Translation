using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public float SecondsPerCycle { get; } = 60.0f;

    [SerializeField] public float TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue = 0.33333f;

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
        WordManager.Instance.RestartConversaitons();
        Debug.Log("Resetted everything and started a new cycle with new points.");
    }
    public void StartNextCycle()
    {
        if (!WordManager.Instance.CheckNextConversation())
        {
            WordManager.Instance.ResetWordsAndLeaders();
            Debug.Log("Letzte Conversation ist fertig!!!");
            return;
        }

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
        currentCycle.Countdown -= Time.deltaTime;
        if(currentCycle.Countdown < 0)
        {
            //countdown is over, cycle isnt stopped anywhere else in the game, so it was successful.
            currentCycle.Countdown = 0;
            onGameCycleUpdated(this, currentCycle);
            EndCurrentCycle(RESULTS.GOOD);
            return;
        }
        currentCycle.WordCountdown -= Time.deltaTime;
        if (currentCycle.WordCountdown < 0)
        {
            currentCycle.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
            SpawnWord();
        }
        if (currentCycle.Countdown > 0)
        {
            onGameCycleUpdated(this, currentCycle);
        }
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
