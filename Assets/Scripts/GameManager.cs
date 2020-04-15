using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Words;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE   
    {
        INIT,
        START_SECTION,  // Only for Understanding
        UPDATE_SECTION,
        END_SECTION,    // Only for Understanding
        EVALUATE_SECTION,   // Only for Understanding
        END_GAME,
    }
    GAMESTATE currentGameState = GAMESTATE.INIT;

    [field: SerializeField] public float SecondsPerSection { get; } = 14.0f;

    [SerializeField] public float TimeBetweenWords = 0.33333f;

    [SerializeField] UIController ui_controller = null;
    [SerializeField] FadeForegroundIn fade_controller = null;
    [SerializeField] ScoreEvaluator scoreEvaluator = null;
    
    public enum RESULTS
    {
        GOOD,
        BAD_ENDING_1,
        BAD_ENDING_2
    }
    RESULTS lastResult = RESULTS.GOOD;
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

    public delegate void NewGameDelegate();
    public delegate void NewSectionDelegate();
    public delegate void UpdateSectionDelegate(GameSection Section);
    public delegate void StartEvaluationDelegate(RESULTS result);
    public delegate void EndSectionDelegate(RESULTS result);
    public delegate void EndGameDelegate(RESULTS result);

    GameSection currentSection;
    public UpdateSectionDelegate onUpdateSection;
    public EndSectionDelegate onEndSectionUpdated;
    public NewSectionDelegate onNewSection;
    public NewGameDelegate onNewGame;
    public EndGameDelegate onEndGame;
    public StartEvaluationDelegate onStartEvaluation;

    public Transform leader1Pos;
    public Transform leader2Pos;

    public GameObject firedEndAnimation;
    public GameObject nukeEndAnimation;
    public GameObject goodEndAnimation;
    public GameObject backToMenuButton;

    public bool InputEnabled = true;

    public void StartNewSection()
    {
        if (currentGameState == GAMESTATE.INIT)
        {
            currentSection = new GameSection();
            currentSection.Countdown = SecondsPerSection;
            currentSection.WordCountdown = TimeBetweenWords;
        }
        onNewGame?.Invoke();

        //Todo: Reset everything for a new Section
        WordManager.Instance.ResetInstance();

        InputEnabled = true;
        // Everything is Initialized -> Start Update
        currentGameState = GAMESTATE.UPDATE_SECTION;
    }
    public void StartNextSection()
    {
        currentGameState = GAMESTATE.START_SECTION;
        onNewSection?.Invoke();
        WordManager.Instance.NextConversation();
        //only reset countdown
        currentSection.Countdown = SecondsPerSection;
        currentSection.WordCountdown = TimeBetweenWords;

        InputEnabled = true;
        // New Section is set-up -> Start Update
        currentGameState = GAMESTATE.UPDATE_SECTION;
    }
    public void EndGame(RESULTS result)
    {
        if (scoreEvaluator.IsInvoking())
            scoreEvaluator.StopAllCoroutines();

        currentGameState = GAMESTATE.END_GAME;
        InputEnabled = false;

        lastResult = result;
        switch (result)
        {
            case RESULTS.BAD_ENDING_1:
                onEndGame?.Invoke(RESULTS.BAD_ENDING_1);
                break;
            case RESULTS.BAD_ENDING_2:
                onEndGame?.Invoke(RESULTS.BAD_ENDING_2);
                break;
            case RESULTS.GOOD:
                onEndGame?.Invoke(RESULTS.GOOD);
                break;
            default:
                Debug.LogError("This shouldnt happen.");
                break;
        }
    }
    public void EvaluateCurrentSection(RESULTS result)
    {
        currentGameState = GAMESTATE.EVALUATE_SECTION;
        onStartEvaluation?.Invoke(result);
        // Start leader 2 talk animation
        leader2Pos.GetComponentInChildren<Animator>().SetTrigger("Talk");
        // Start leader 2 talk sound
        SoundController.Instance.PlayRandomSound(SoundController.audio_id.ID_PUTIN_1, SoundController.audio_id.ID_PUTIN_5);
        
    }

    public void EndCurrentSection(RESULTS result)
    {
        if (currentGameState == GAMESTATE.END_GAME)
            return;
        currentGameState = GAMESTATE.END_SECTION;

        onEndSectionUpdated?.Invoke(result);
        StartNextSection();
    }
    
    public void UpdateRunningSection()
    {
        if (currentSection.Countdown < 0)
        {
            InputEnabled = false;

            //countdown is over, Section isnt stopped anywhere else in the game, so it was successful.
            //onGameSectionUpdated(this, currentSection);
            if (!WordManager.Instance.CheckNextConversation())
            {
                currentGameState = GAMESTATE.END_GAME;
                EndGame(RESULTS.GOOD);
                return;
            }
            EvaluateCurrentSection(RESULTS.GOOD);
            return;
        }
        currentSection.Countdown -= Time.deltaTime;
        currentSection.WordCountdown -= Time.deltaTime;
        if (currentSection.WordCountdown < 0)
        {
            currentSection.WordCountdown = TimeBetweenWords;
            SpawnWord();
        }
        if (currentSection.Countdown > 0)
        {
            onUpdateSection?.Invoke(currentSection);
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

    void Start()
    {
        uiGone = false;
        gameGone = false;
        if (ui_controller == null)
            Debug.LogError("Bind the ui controller to the gamemanager");
        if (fade_controller == null)
            Debug.LogError("Bind the fade controller to the gamemanager");
        if (scoreEvaluator == null)
            Debug.LogError("Bind the score evaluator to the gamemanager");


        ui_controller.onUICrossfaded += OnUIIsGone;
        fade_controller.onForegroundCrossfaded += OnGameIsGone;
        scoreEvaluator.onEvaluationFinshed += EndCurrentSection;

        currentGameState = GAMESTATE.INIT;
        
        StartNewSection();
    }


    private void OnGameIsGone()
    {
        uiGone = true;
        CheckOnEnding();
    }

    private void OnUIIsGone()
    {
        gameGone = true;
        CheckOnEnding();
    }

    void CheckOnEnding()
    {
        if (uiGone && gameGone)
        {
            if (backToMenuButton != null)
            {
                backToMenuButton.SetActive(true);

                backToMenuButton.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    SceneManager.LoadScene(0);
                });
            }
            else
            {
                Debug.LogWarning("PLEASE ASSIGN THE BACK BUTTON IN GAMEMANAGER! ~Karen");
            }
            SoundController.Instance.StopSound();
            //Trigger ending here and go to menu after that.
            switch (lastResult)
            {
                case RESULTS.BAD_ENDING_1:
                    firedEndAnimation.SetActive(true);
                    break;
                case RESULTS.BAD_ENDING_2:
                    nukeEndAnimation.SetActive(true);
                    break;
                case RESULTS.GOOD:
                    goodEndAnimation.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GAMESTATE.UPDATE_SECTION) UpdateRunningSection();
    }
}
