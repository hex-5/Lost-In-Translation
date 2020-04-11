using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer), typeof(SpriteSkin))]
public class Bubble : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float scaleMultiplierMin = 1.5f;
    [SerializeField] private float scaleMultiplierMax = 3.0f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.isTrigger = false;
        collision.transform.localScale = collision.transform.localScale * Random.Range(scaleMultiplierMin, scaleMultiplierMax);
        int amount = 0;
        foreach(var word in Words.WordManager.Instance.wordList)
        {
            if ((!word.GetComponentInChildren<Collider2D>().isTrigger))
                amount++;
        }
        if(amount >= Words.WordManager.Instance.wordList.Count)
        {
            animator.SetBool("isOpen", false);
            
        }
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
