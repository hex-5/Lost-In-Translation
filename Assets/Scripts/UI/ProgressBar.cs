using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ProgressBar : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right
    }

    public Direction ProgressDirection;

    public GameObject ProgressImage;

    [SerializeField, Range(0,1)]
    private float _progress;

    private Color _progressBarColor;

    public float Progress
    {
        get => _progress;
        set
        {
            _progress = Mathf.Clamp01(value);
            if (ProgressDirection == Direction.Right)
            {
                ProgressImage.GetComponent<RectTransform>().anchorMin = new Vector2(_progress,0);
                ProgressImage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            }
            else
            {
                ProgressImage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                ProgressImage.GetComponent<RectTransform>().anchorMax = new Vector2(_progress, 1);
            }
        }
    }

    public Color ProgressBarColor
    {
        get => ProgressImage.GetComponent<Image>().color; 
        set { ProgressImage.GetComponent<Image>().color = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Progress = _progress;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
