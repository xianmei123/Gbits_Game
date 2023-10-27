using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    int normal;
    float timer1;
    public float timer1Limit;
    private void Awake()
    {
        normal = this.GetComponent<PlatformEffector2D>().colliderMask;
    } 
    
    private void Update()
    {
        if(this.GetComponent<PlatformEffector2D>().colliderMask != normal)
        {
            if (timer1 >= timer1Limit)
            {
                timer1 = 0;
                this.GetComponent<PlatformEffector2D>().colliderMask = normal;
                gameObject.layer = LayerMask.NameToLayer("Ground");
            }
            timer1 += Time.deltaTime;

        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         if (other.gameObject.GetComponent<PlayerController>().MoveSpeedY < -5)
    //         {
    //             
    //         }
    //     }
    //     
    // }
}