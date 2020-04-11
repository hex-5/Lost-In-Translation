using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer), typeof(SpriteSkin))]
public class Bubble : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int amount = CountObjectsInside();
        if(amount == 0)
        {
            animator.SetBool("isOpen", false);
        }
    }

    private int CountObjectsInside()
    {
        int amount = 0;
        foreach (var word in Words.WordManager.Instance.wordList)
        {
            if (word.gameObject.layer == LayerMask.NameToLayer("WordInBubble"))
                amount++;
        }

        return amount;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        animator.SetBool("isOpen", true);
    }

    public void onCloseFinished()
    {
        gameObject.SetActive(false);
    }

    void Update(float DeltaSeconds)
    {
        
    }


}
