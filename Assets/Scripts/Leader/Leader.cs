using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Leader : MonoBehaviour
{
    public bool IsAngry
    {
        get => _isAngry;
        set
        {
            _isAngry = value;
            _animator.SetBool("isAngry",value);
        }
    }

    private Animator _animator;
    private bool _isAngry;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
