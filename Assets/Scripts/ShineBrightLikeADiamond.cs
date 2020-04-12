using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineBrightLikeADiamond : MonoBehaviour
{
    [Tooltip("Edge collider sitting on a different GameObject, e.g. on a child.")]
    public EdgeCollider2D edgeCollider;

    [SerializeField]
    [ColorUsage(true, true)]
    Color good = new Color(0,0,0,0);
    [SerializeField]
    [ColorUsage(true, true)]
    Color bad = new Color(0, 0, 0, 0);

    public bool dragging = false;
    public Collider2D currentCollider = null;

    private bool insideTriangle = false;
    private Collider2D lastCollider = null;
    private List<Collider2D> wordsInside = null;

    private void Start()
    {
        wordsInside = new List<Collider2D>();
    }

    private void Update()
    {
        if (dragging == false)
        {
            GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
        }
        else
        {
            if (!wordsInside.Contains(currentCollider))
                insideTriangle = false;
            else
                insideTriangle = true;

            if (Input.GetMouseButtonDown(0))
            {
                if (CheckWordOnLine(lastCollider))
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                }
                else
                {
                    if (insideTriangle)
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                        GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", good);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (CheckWordOnLine(lastCollider))
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                }
                else
                {
                    if (insideTriangle)
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                        GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", good);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (CheckWordOnLine(lastCollider))
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                }
                else
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
                }
            }
        }
    }

    public bool CheckWordOnLine(Collider2D collider)
    {
        if (lastCollider != null)
        {
            if (Physics2D.IsTouching(collider, edgeCollider))
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            if (!wordsInside.Contains(currentCollider))
                wordsInside.Add(collision);
            insideTriangle = true;
            lastCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            if (wordsInside.Contains(currentCollider))
                wordsInside.Remove(collision);
            insideTriangle = false;
            lastCollider = collision;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            lastCollider = collision;
        }
    }
}
