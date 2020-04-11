using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleControl : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        GameObject.FindObjectOfType<GameManager>().onNewCycle += onNewCycle;
        ani = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.isTrigger = false;

        int amount = 0;
        foreach(var word in Words.WordManager.Instance.wordList)
        {
            if ((!word.GetComponentInChildren<Collider2D>().isTrigger))
                amount++;
        }
        if(amount >= Words.WordManager.Instance.wordList.Count)
        {
            ani.Play("close");
        }
    }
    public void onNewCycle(GameManager manager, bool newGame)
    {
        ani.Play("open");
    }
}
