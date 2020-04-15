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
    public Bubble ActiveBubble;

    void Start()
    {
        ActiveBubble = IsAngryToBubbleMap[0];
        _gameManager = managers.GetComponent<GameManager>();
        _gameManager.onNewGame += OnNewSection;
        _gameManager.onNewSection += OnNewSection;
    }

    private void OnNewSection()
    {
        foreach (Bubble bubble in IsAngryToBubbleMap)
        {
            bubble.gameObject.SetActive(false);
        }
        speakerLeader = _gameManager.leader1Pos.gameObject.GetComponentInChildren<Leader>();
        IsAngryToBubbleMap[speakerLeader.IsAngry?1:0].Open();
        ActiveBubble = IsAngryToBubbleMap[speakerLeader.IsAngry ? 1 : 0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
