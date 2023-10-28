using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] public String LevelName;
    private Animator _animator;
    private bool IsPlaying = false;
    private float curTime = 0f;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        
        if (IsPlaying)
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (Time.time - curTime > 1f)
            {
                IsPlaying = false;
                SceneManager.LoadScene(LevelName);
            }
            
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("下一关");
        if (other.gameObject.CompareTag("Player"))
        {
            
            _animator.Play("nextLevel", 0, 0f);
            IsPlaying = true;
            curTime = Time.time;
        }
    }
}
