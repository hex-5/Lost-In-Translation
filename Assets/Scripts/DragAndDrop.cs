using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool dragging;
    Vector3 offset;
    private void OnMouseDown()
    {
        offset = cam.ScreenToWorldPoint(Input.mousePosition) - transform.localPosition;
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
    private void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition) - offset;// - transform.position;
            mousePos.z = 0;
            transform.localPosition = mousePos;

            if (Input.GetMouseButtonDown(1))
            {
                Rotate();
            }
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
    }
    private void Update()
    {
    }

}