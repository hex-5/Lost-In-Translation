using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragAndDrop : MonoBehaviour
{
    [SerializeField] float rotationDegrees = 22.5f;
    bool dragging;
    Vector3 offset;
    Rigidbody2D rigidBody;
    
    private void OnMouseDown()
    {
        offset = cam.ScreenToWorldPoint(Input.mousePosition) - transform.localPosition;
        dragging = true;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().dragging = true;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().currentCollider = transform.GetChild(0).GetComponent<Collider2D>();
    }

    private void OnMouseUp()
    {
        dragging = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().dragging = false;
    }
    private void OnMouseDrag()
    {
        if (dragging)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
                RotateRight();
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(1))
                RotateLeft();
        }
    }
    private void OnMouseOver()
    {
    }

    private void RotateLeft()
    {
        transform.localRotation = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z - rotationDegrees, Vector3.forward);
    }
    private void RotateRight()
    {
        transform.localRotation = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z + rotationDegrees, Vector3.forward);
    }

    private Camera cam;
    public void Start()
    {
        cam = Camera.main;
        rigidBody = this.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FixedUpdate()
    {
        if (!dragging)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y)) - transform.position - offset;
        rigidBody.MovePosition(rigidBody.position + new Vector2(move.x, move.y));
    }

}