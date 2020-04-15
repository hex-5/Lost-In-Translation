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
    GameManager gameManager;

    private bool IsInputEnabled()
    {
        return gameManager.InputEnabled;
    }

    private void OnMouseDown()
    {
        if (!IsInputEnabled())
            return;
        SoundController.Instance.PlaySound(SoundController.audio_id.ID_SFX_PICK_WORD, false);
        transform.parent = null;
        offset = cam.ScreenToWorldPoint(Input.mousePosition) - transform.localPosition;
        dragging = true;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().dragging = true;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().currentCollider = transform.GetChild(0).GetComponent<Collider2D>();
    }

    private void OnMouseUp()
    {
        if (IsInputEnabled())
            SoundController.Instance.PlaySound(SoundController.audio_id.ID_SFX_DROP_WORD, false);
        dragging = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject.Find("all_seeing_eye").GetComponent<ShineBrightLikeADiamond>().dragging = false;
    }
    private void OnMouseDrag()
    {
        if (!IsInputEnabled())
        {
            if(dragging)
            {
                OnMouseUp();
            }
        }

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
        if (!IsInputEnabled())
            return;
        SoundController.Instance.PlaySound(SoundController.audio_id.ID_SFX_ROTATE_WORD, false);
        transform.localRotation = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z - rotationDegrees, Vector3.forward);
    }
    private void RotateRight()
    {
        if (!IsInputEnabled())
            return;
        SoundController.Instance.PlaySound(SoundController.audio_id.ID_SFX_ROTATE_WORD, false);
        transform.localRotation = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z + rotationDegrees, Vector3.forward);
    }

    private Camera cam;
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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