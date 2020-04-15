using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeForegroundIn : MonoBehaviour
{
    [SerializeField] GameManager gameManager = null;
    SpriteRenderer foregroundRenderer;
    // Start is called before the first frame update
    void Start()
    {
        foregroundRenderer = GetComponent<SpriteRenderer>();
        if (foregroundRenderer == null)
            Debug.LogError("foregroundRenderer is null! Add the script to a Gameobject containing the foreground sprite renderer!");
        if (gameManager == null)
            Debug.LogError("gameManager is null! Fill the field with the Gameobject containing the gameManager!");

        gameManager.onEndGame += OnEndGame;
    }
    bool startFading = false;
    private void OnEndGame(GameManager.RESULTS result)
    {   
        startFading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startFading) return;
        foregroundRenderer.color = new Color(foregroundRenderer.color.r, foregroundRenderer.color.g, foregroundRenderer.color.b, Mathf.Lerp(Mathf.Lerp(foregroundRenderer.color.a, 1, Time.deltaTime), 1, Time.deltaTime));
        if (foregroundRenderer.color.a > 0.999)
        {
            foregroundRenderer.color = new Color(foregroundRenderer.color.r, foregroundRenderer.color.g, foregroundRenderer.color.b, 1);
            startFading = false;
            onForegroundCrossfaded();
        }
    }

    public delegate void ForegroundCrossfadedDelegate();
    public ForegroundCrossfadedDelegate onForegroundCrossfaded;
}
