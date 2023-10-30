using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlayerController playerController;

    private BoxCollider2D _boxCollider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(playerController.Laddering);
        if (playerController.Laddering)
        {
            
            gameObject.layer = LayerMask.NameToLayer("Ground");
            _boxCollider2D.isTrigger = false;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            _boxCollider2D.isTrigger = true;
            
        }
    }
}
