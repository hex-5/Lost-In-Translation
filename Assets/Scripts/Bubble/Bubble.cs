using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer), typeof(SpriteSkin))]
public class Bubble : MonoBehaviour
{
    private Animator animator;
    private Transform mostDeepChild;
    private GameManager gameManager;

    private Transform GetDeepestChild(Transform parent)
    {
        if (parent.childCount > 0)
            return (GetDeepestChild(parent.GetChild(0)));
        else
            return parent;
    }

    void Start()
    {
        mostDeepChild = GetDeepestChild(this.transform);
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.onUpdateSection += CheckForWordsInside;
    }

    private void CheckForWordsInside(GameManager.GameSection Section)
    {
        if(mostDeepChild.childCount == 0)
            animator.SetBool("isOpen", false);
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
}
