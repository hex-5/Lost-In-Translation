using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool dragging;
    Vector3 offset;
    Rigidbody2D rigidBody;
    private void OnMouseDown()
    {
        offset = cam.ScreenToWorldPoint(Input.mousePosition) - transform.localPosition;
        dragging = true;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnMouseUp()
    {
        dragging = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void OnMouseDrag()
    {
        if (dragging && Input.GetMouseButtonDown(1))
        {
            Rotate();
        }
    }
    private void OnMouseOver()
    {
    }

    private void Rotate()
    {
        transform.localRotation = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z - 30.0f, Vector3.forward);
    }

    private Camera cam;
    public void Start()
    {
        cam = Camera.main;
        rigidBody = this.GetComponent<Rigidbody2D>();
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