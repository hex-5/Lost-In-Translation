using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteSkin))]
public class EyeFollowCursor : MonoBehaviour
{

    public Camera cam;
    public int eyeBoneIndex = 1;
    public float eyeMaxOffset = 1;

    private SpriteSkin _skin;
    private Vector3 _eyeLocalRestPosition;

    void Start()
    {
        _skin = GetComponent<SpriteSkin>();
        _eyeLocalRestPosition = _skin.boneTransforms[eyeBoneIndex].localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Transform eyeTransform = _skin.boneTransforms[eyeBoneIndex];
        Vector3 cursorLocalPosition = eyeTransform.InverseTransformPoint(cam.ScreenToWorldPoint(Input.mousePosition));
        eyeTransform.localPosition = _eyeLocalRestPosition + Vector3.ClampMagnitude(cursorLocalPosition*3, eyeMaxOffset);
    }
}
