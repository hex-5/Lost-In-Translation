using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    public GameManager gameManager;
    public Leader speakerLeader;

    [SerializeField]
    public Bubble[] IsAngryToBubbleMap;

    void Start()
    {
        gameManager.onNewCycle += OnNewCycle;
    }

    private void OnNewCycle(GameManager manager, bool newgame)
    {
        IsAngryToBubbleMap[speakerLeader.IsAngry?0:1].Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
