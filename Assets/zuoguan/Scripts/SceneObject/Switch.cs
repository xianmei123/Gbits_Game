using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] public GameObject Door;

    private bool CanInteraction = true;

    private Animator _animator;

    private string stateName;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (name.Contains("Green"))
        {
            stateName = "Switch1";
        }
        else if (name.Contains("Red"))
        {
            stateName = "Switch2";
        }
        else if (name.Contains("Blue"))
        {
            stateName = "Switch3";
        }
        else if (name.Contains("Yellow"))
        {
            stateName = "Switch4";
        }
        else if (name.Contains("Grey"))
        {
            stateName = "Switch5";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanInteraction && other.transform.CompareTag("Player"))
        {
            Debug.Log(stateName);
            _animator.Play(stateName);
            Door door = Door.GetComponent<Door>();
            door.Open();
            CanInteraction = false;
        }
        
    }
}
