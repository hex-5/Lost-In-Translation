using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIController : MonoBehaviour
{

    public ProgressBar ProgressBarObject;
    public UnityEngine.UI.Text TimeRemainingTextObject;

    public Color TimeProgressBadColor = Color.red;
    public Color TimeProgressGoodColor = Color.white;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.onUpdateSection += OnGameSectionUpdated;
            //Todo: add to gameEnd delegate
            gameManager.onEndGame += OnEndGame;
        }
        else
        {
            Debug.LogWarning("UIController: Could not find GameManager.");
        }
    }

    private void OnEndGame(GameManager.RESULTS result)
    {
        StartDestroyYourself();
    }

    private void OnGameSectionUpdated(GameManager.GameSection Section)
    {
        float progress = 1 - (Section.Countdown / gameManager.SecondsPerSection);
        ProgressBarObject.Progress = progress;
        ProgressBarObject.ProgressBarColor = Color.Lerp(TimeProgressGoodColor, TimeProgressBadColor, progress*progress);
        TimeRemainingTextObject.text = $"{Section.Countdown % 60:0.00}s";
    }

    // Update is called once per frame
    void Update()
    {
        if (!startDestroying) return;
        UIGroup.alpha = Mathf.Lerp(Mathf.Lerp(UIGroup.alpha, 0, Time.deltaTime), 0, Time.deltaTime);
        if(UIGroup.alpha < 0.005f)
        {
            this.gameObject.SetActive(false);
            startDestroying = false;
            if (onUICrossfaded.Target != null) onUICrossfaded();
        }
    }

    CanvasGroup UIGroup;
    bool startDestroying = false;
    private void StartDestroyYourself()
    {
        UIGroup = this.GetComponent<CanvasGroup>();
        startDestroying = true;
    }



    public delegate void UICrossfadedDelegate();
    public UICrossfadedDelegate onUICrossfaded;
}
