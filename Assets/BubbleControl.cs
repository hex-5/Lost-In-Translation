﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleControl : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        ani.Play("open");
    }
    
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.isTrigger = false;

    }
}
