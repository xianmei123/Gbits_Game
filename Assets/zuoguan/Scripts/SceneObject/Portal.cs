using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] public AudioClip teleplotAudio;
    [SerializeField] public Transform target;
    private PlayerController playerController;
    private bool canTeleport = false;
    private Animator _animator;
    private bool IsPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
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
                playerController.setTransform(target);
            }
                 
        }
        
        if (canTeleport && playerController != null && playerController.Interaction)
        {
            SoundEffectPlayer.AudioSource.clip = teleplotAudio;
            SoundEffectPlayer.AudioSource.pitch = 3;
            SoundEffectPlayer.AudioSource.time = 1;
            SoundEffectPlayer.AudioSource.volume = 0.5f;
            SoundEffectPlayer.AudioSource.Play();
            // SoundEffectPlayer.AudioSource.PlayOneShot(teleplotAudio);
            _animator.Play("teleport", 0, 0f);
            IsPlaying = true;

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