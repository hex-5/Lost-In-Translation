using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool dragging;

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void Update()
    {
        if (dragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePos);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.DrawLine(transform.position, mousePos, Color.red, 5.0f);

            if (Input.GetMouseButtonDown(1))
            {
                Rotate();
            }
        }
    }

    private void Rotate()
    {
        transform.GetChild(0).Rotate(Vector3.forward, -30.0f);
    }
}