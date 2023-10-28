using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] public Transform target;
    private PlayerController playerController;
    private bool canTeleport = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canTeleport && playerController != null && playerController.Interaction)
        {
            playerController.setTransform(target);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.transform.CompareTag("Player"))
        {
            playerController = other.transform.gameObject.GetComponent<PlayerController>();

            canTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            canTeleport = false;
        }
    }
}
