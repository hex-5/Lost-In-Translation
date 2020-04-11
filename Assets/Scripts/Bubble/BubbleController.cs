using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class BubbleController : MonoBehaviour
{

    public GameObject managers;
    public Leader speakerLeader;
    private GameManager _gameManager;
    [SerializeField]
    public Bubble[] IsAngryToBubbleMap;

    void Start()
    {
        _gameManager = managers.GetComponent<GameManager>();
        _gameManager.onNewCycle += OnNewCycle;
    }

    private void OnNewCycle(GameManager manager, bool newgame)
    {
        foreach (Bubble bubble in IsAngryToBubbleMap)
        {
            bubble.gameObject.SetActive(false);
        }
        speakerLeader = _gameManager.leader1Pos.gameObject.GetComponentInChildren<Leader>();
        IsAngryToBubbleMap[speakerLeader.IsAngry?1:0].Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
