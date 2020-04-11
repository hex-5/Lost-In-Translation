using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineBrightLikeADiamond : MonoBehaviour
{
    [Tooltip("Edge collider sitting on a different GameObject, e.g. on a child.")]
    public EdgeCollider2D edgeCollider;

    [SerializeField]
    [ColorUsage(true, true)]
    Color good;
    [SerializeField]
    [ColorUsage(true, true)]
    Color bad;
    [SerializeField]
    GameManager gameManager;

    public List<Collider2D> objectsOnBorder = null;
    private bool insideTriangle = false;
    private Collider2D lastCollider = null;

    private void Start()
    {
        objectsOnBorder = new List<Collider2D>();

        //if (gameManager != null)
        //{
        //    gameManager.onNewCycle += RegisterAllWords;
        //}
    }

    //private void RegisterAllWords(GameManager manager, bool newGame)
    //{

    //}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lastCollider != null)
            {
                objectsOnBorder.Remove(lastCollider);
                if (CheckWordOnLinePure(lastCollider))
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                    lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", bad);
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
        }

        if (Input.GetMouseButton(0))
        {
            if (lastCollider != null)
            {
                if (CheckWordOnLinePure(lastCollider) || objectsOnBorder.Count > 0)
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                    lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", bad);
                }
                else
                {
                    if (insideTriangle)
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                        GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", good);
                        lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", new Color(0, 0, 0, 0));
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
                        lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", new Color(0, 0, 0, 0));
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (lastCollider != null)
            {
                if (CheckWordOnLine(lastCollider))
                {
                    GetComponent<SpriteRenderer>().material.SetInt("_Active", 1);
                    GetComponent<SpriteRenderer>().material.SetColor("_ShineColor", bad);
                    lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", bad);
                }
                else
                {
                    if (objectsOnBorder.Count == 0)
                    {
                        GetComponent<SpriteRenderer>().material.SetInt("_Active", 0);
                    }
                    lastCollider.GetComponent<SpriteRenderer>().material.SetColor("_TintRGBA_Color_1", new Color(0, 0, 0, 0));
                }
            }
        }
    }

    public bool CheckWordOnLinePure(Collider2D collider)
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

    public bool CheckWordOnLine(Collider2D collider)
    {
        if (lastCollider != null)
        {
            if (Physics2D.IsTouching(collider, edgeCollider))
            {
                if (!objectsOnBorder.Contains(collider))
                {
                    objectsOnBorder.Add(collider);
                }
                return true;
            }
            else
            {
                if (objectsOnBorder.Contains(collider))
                {
                    objectsOnBorder.Remove(collider);
                }
                return false;
            }
        }
        else return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            insideTriangle = true;
            lastCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            insideTriangle = false;
            lastCollider = collision;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Word"))
        {
            insideTriangle = true;
            //lastCollider = collision;
        }
    }
}
