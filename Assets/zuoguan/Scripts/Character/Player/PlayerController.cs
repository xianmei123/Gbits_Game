using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore;

public class PlayerController : MonoBehaviour
{
   
    [SerializeField] private float SprintCD = 5f;
    [SerializeField] private bool invincible = false;
  
    [SerializeField] public List<Vector3> pos;
    
    private float sprintTime = 0.0f;

    private PlayerGroundDetector groundDetector;
    private PlayerClimbDetector ClimbDetector;
    private PlayerInput input;

    private Rigidbody2D rigidBody;

    private Collider2D otherCollider;

    private CapsuleCollider2D playerCollider;
    private bool canInteraction = false;

    private bool isAttacking = false;

    public AudioSource VoicePlayer { get; private set; }

    public bool CanAirJump { get; set; }
    public bool Victory { get; private set; }


    public bool canSprint = true;

    public bool canFlySprint = true;


    private Vector3 InitialPos;

    public bool CanClimb => ClimbDetector.CanClimb;
    public bool IsGrounded => groundDetector.IsGrounded;
    public bool IsFalling => rigidBody.velocity.y < 0f && !IsGrounded;

    public float MoveSpeedX => Mathf.Abs(rigidBody.velocity.x);
    public float MoveSpeedY => rigidBody.velocity.y;

    private GameObject InteractionObject;

    // private MovingPlatform movingPlatform;
    public bool InMovingPlatform = false;

    public Dictionary<string, bool> skills = new Dictionary<string, bool>();
    

    public bool AttackSkill { get; set; }

    private int skillType = 0;

    private bool IsDeath = false;

    public Vector2 distance;

    

    private void Awake()
    {
        groundDetector = GetComponentInChildren<PlayerGroundDetector>();
        ClimbDetector = GetComponentInChildren<PlayerClimbDetector>();
        input = GetComponent<PlayerInput>();
        if (input == null)
        {
            Debug.Log("false");
        }

        rigidBody = GetComponent<Rigidbody2D>();
        VoicePlayer = GetComponentInChildren<AudioSource>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        gameObject.layer = LayerMask.NameToLayer("Player");


        // Debug.Log(SkillData.Instance.SprintSkill + " " + SkillData.Instance.FlySprintSkill + " " + SkillData.Instance.ClimbSkill + " " + SkillData.Instance.AttackSkill);
        skills.Add("Jump", false);
        skills.Add("Climb", false);
        skills.Add("Sprint", false);
        
    }


    private void Start()
    {
        
        input.EnableGameplayInputs();
        
        
        InitialPos = transform.position;
    }

    private void Update()
    {
        if (sprintTime < SprintCD)
        {
            sprintTime += Time.deltaTime;
        }
        else
        {
            canSprint = true;
        }
        
        // Debug.Log(HaveSkill("Sprint") + " " + HaveSkill("Climb") + " " + HaveSkill("Jump"));


        if (input.Interaction && skillType > 0)
        {
            switch (skillType)
            {
                case 1:
                    GetSkill("Sprint");
                    
                    break;
                case 2:
                    GetSkill("Climb");
                  
                    break;
                case 3:
                    GetSkill("Jump");
                  
                    break;
            }
            
        }
    }


    private void FixedUpdate()
    {
        if (InMovingPlatform)
        {
            transform.Translate(distance);
        }
    }

    public void GetSkill(string skillName)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            var item = skills.ElementAt(i);
            skills[item.Key] = false;
        }
        
        skills[skillName] = true;
    }

    public void EnterSprintCD()
    {
        canSprint = false;
        sprintTime = 0.0f;
    }
    

    public void Move(float speed)
    {
        if (input.Move)
        {
            transform.localScale = new Vector3(input.AxisX, 1f, 1f);
        }

        SetVelocityX(speed * input.AxisX);
    }
    
    public void MoveUpDown(float speed)
    {
      

        SetVelocityY(speed * input.AxisY);
    }


    public void SetVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y);
    }

    public void SetVelocityY(float velocityY)
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocityY);
    }

    public void SetUseGravity(float value)
    {
        // rigidBody.useGravity = value;
        rigidBody.gravityScale = value;
    }

    public Vector3 GetVelocity()
    {
        return rigidBody.velocity;
    }

    public Vector2 GetPlayerSize()
    {
        return playerCollider.size;
    }

    public void SetPlayerSize(Vector2 size)
    {
        playerCollider.size = size;
    }

    public Vector2 GetPlayerOffset()
    {
        return playerCollider.offset;
    }

    public void SetPlayerOffset(Vector2 offset)
    {
        playerCollider.offset = offset;
    }

    public void SetInvincible()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
    }

    public void SetPlayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void InPlatform()
    {
        groundDetector.InPlatform();
    }



    public void Dead()
    {

        IsDeath = true;
    }

    public bool PlayerDeath()
    {
        return IsDeath;
    }

    public void Restart()
    {
        IsDeath = false;
        transform.position = InitialPos;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            otherCollider = other;

        }

        if (other.transform.CompareTag("Interaction_Skill_Sprint"))
        {
            
            skillType = 1;
        }
        else if (other.transform.CompareTag("Interaction_Skill_Climb"))
        {
          
            skillType = 2;
        }
        else if (other.transform.CompareTag("Interaction_Skill_Jump"))
        {
           
            skillType = 3;
        }
  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag.Contains("Interaction_Skill"))
        {
            
            skillType = 0;
        }
    }


 

    public bool HaveSkill(string skillName)
    {
        return skills[skillName];
    }
}