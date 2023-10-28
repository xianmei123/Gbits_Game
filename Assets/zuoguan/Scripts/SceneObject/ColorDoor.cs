using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    private PlayerController playerController;
    private Animator _animator;
    private bool IsPlaying = false;
    private int type = 0;
    private BoxCollider2D _boxCollider2D;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        if (name.Contains("Green"))
        {
            type = 1;
        }
        else if (name.Contains("Red"))
        {
            type = 2;
        }
        else if (name.Contains("Blue"))
        {
            type = 3;
        }
        else if (name.Contains("Yellow"))
        {
            type = 4;
        }
        else if (name.Contains("Grey"))
        {
            type = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (info.normalizedTime >= 1.0f)
            {
                IsPlaying = false;
                _boxCollider2D.excludeLayers += (1 << LayerMask.NameToLayer("Player"));
            }
                 
        }
        
      
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerController = other.transform.gameObject.GetComponent<PlayerController>();
          
            if (playerController.type == type)
            {
                _animator.Play("Open");
                IsPlaying = true;
            }
        }
    }

   
}
