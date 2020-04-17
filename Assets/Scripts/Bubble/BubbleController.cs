using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class BubbleController : MonoBehaviour
{

    private Leader speakerLeader;
    public GameManager _gameManager;
    [SerializeField]
    public Bubble[] IsAngryToBubbleMap;
    public Bubble ActiveBubble;

    private void Awake()
    {
        ActiveBubble = IsAngryToBubbleMap[0];
    }

    void Start()
    {
        _gameManager.onNewGame += OnNewSection;
        _gameManager.onNewSection += OnNewSection;
        foreach (Bubble bubble in IsAngryToBubbleMap)
        {
            bubble.gameObject.SetActive(false);
        }
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
}
