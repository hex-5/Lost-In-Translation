using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public float SecondsPerSection { get; } = 500.0f;

    [SerializeField] public float TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue = 0.33333f;

    [SerializeField] UIController ui_controller;
    [SerializeField] FadeForegroundIn fade_controller;
    private bool gameEnded = false;
    public enum RESULTS
    {
        GOOD,
        BAD_ENDING_1,
        BAD_ENDING_2
    }
    //[SerializeField] ...

    public class GameSection
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

    public delegate void NewSectionDelegate(GameManager manager, bool newGame);
    public delegate void GameSectionDelegate(GameManager manager, GameSection Section);
    public delegate void EndSectionDelegate(GameManager manager, RESULTS result);
    public delegate void EndGameDelegate(GameManager manager, RESULTS result);

    GameSection currentSection;
    public GameSectionDelegate onGameSectionUpdated;
    public EndSectionDelegate onEndSectionUpdated;
    public NewSectionDelegate onNewSection;
    public EndGameDelegate onEndGame;

    public Transform leader1Pos;
    public Transform leader2Pos;

    public void StartNewSection()
    {
        if (currentSection == null)
        {
            currentSection = new GameSection();
            currentSection.Countdown = SecondsPerSection;
            currentSection.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
            //currentSection.Points = 0;
            onGameSectionUpdated(this, currentSection);
        }
        onNewSection(this, true);
        //Todo: Reset everything for a new Section
        WordManager.Instance.RestartConversaitons();
        Debug.Log("Resetted everything and started a new Section with new points.");
    }
    public void StartNextSection()
    {
        if (!WordManager.Instance.CheckNextConversation())
        {
            WordManager.Instance.ResetWords();
            Debug.Log("Letzte Conversation ist fertig!!!");
            return;
        }

        onNewSection(this, false);
        //only reset countdown
        currentSection.Countdown = SecondsPerSection;
        currentSection.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
        //Todo: reset words
        WordManager.Instance.NextConversation();
        Debug.Log("Resetted words and started a new Section with existing points. [todo: getPointsFromSomewhere()]");
        onGameSectionUpdated(this, currentSection);
    }
    public void EndCurrentSection(RESULTS result)
    {
        onEndSectionUpdated(this, result);

        // Start leader 2 talk animation
        leader2Pos.GetComponentInChildren<Animator>().SetTrigger("Talk");
        // Start leader 2 talk sound
        SoundController.Instance.PlayRandomSound(SoundController.audio_id.ID_PUTIN_1, SoundController.audio_id.ID_PUTIN_5);
        switch (result)
        {
            case RESULTS.BAD_ENDING_1:
                onEndGame(this, RESULTS.BAD_ENDING_1);
                gameEnded = true;
                break;
            case RESULTS.BAD_ENDING_2:
                onEndGame(this, RESULTS.BAD_ENDING_1);
                gameEnded = true;
                break;
            //Todo: Script Endings and reset to start
            case RESULTS.GOOD:
                if (!WordManager.Instance.CheckNextConversation())
                {
                    onEndGame(this, RESULTS.GOOD);
                    gameEnded = true;
                }
                else
                {
                    StartNextSection();
                }
                break;
            default:
                onEndGame(this, RESULTS.GOOD);
                break;
        }
    }
    public void UpdateRunningSection()
    {
        currentSection.Countdown -= Time.deltaTime;
        if (currentSection.Countdown < 0)
        {
            //countdown is over, Section isnt stopped anywhere else in the game, so it was successful.
            currentSection.Countdown = 0;
            onGameSectionUpdated(this, currentSection);
            EndCurrentSection(RESULTS.GOOD);
            return;
        }
        currentSection.WordCountdown -= Time.deltaTime;
        if (currentSection.WordCountdown < 0)
        {
            currentSection.WordCountdown = TheAmountOfTimeInSecondsThatIsSleptBetweenEverySingleWordWhichAreSpawnedInThisIntervalNowFuckOffAndAcceptThisValue;
            SpawnWord();
        }
        if (currentSection.Countdown > 0)
        {
            onGameSectionUpdated(this, currentSection);
        }
    }

    public void SpawnWord()
    {
        if (WordManager.Instance.SpawnWord())
        {
            // Start leader 1 talk animation
            leader1Pos.GetComponentInChildren<Animator>().SetTrigger("Talk");
            SoundController.Instance.PlayRandomSound(SoundController.audio_id.ID_TRUMP_SHORT_1, SoundController.audio_id.ID_TRUMP_SHORT_7);
        }
    }

    bool uiGone = false;
    bool gameGone = false;
    // Start is called before the first frame update
    void Start()
    {
        uiGone = false;
        gameGone = false;
        if (ui_controller == null)
            Debug.LogError("Bind the ui controller to the gamemanager");
        if (fade_controller == null)
            Debug.LogError("Bind the fade controller to the gamemanager");

        ui_controller.onUICrossfaded += onUIIsGone;
        fade_controller.onForegroundCrossfaded += onGameIsGone;

        StartNewSection();
    }


    private void onGameIsGone()
    {
        uiGone = true;
    }

    private void onUIIsGone()
    {
        gameGone = true;
    }

    void CheckOnEnding()
    {
        if (uiGone && gameGone)
        {
            SoundController.Instance.StopSound();
            //Trigger ending here and go to menu after that.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameEnded) UpdateRunningSection();
    }
}
