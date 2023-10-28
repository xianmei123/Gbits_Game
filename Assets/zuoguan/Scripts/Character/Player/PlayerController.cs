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

    private BoxCollider2D playerCollider;
    public bool canInteraction = false;

    private bool isAttacking = false;

    public AudioSource VoicePlayer { get; private set; }

    public bool CanAirJump { get; set; }
    public bool Victory { get; private set; }


    public bool canSprint = true;

    public bool canFlySprint = true;


    private Vector3 InitialPos;

    public bool CanClimb => ClimbDetector.CanClimb;
    // public bool IsGrounded => groundDetector.IsGrounded;
    
    public bool IsGrounded = true;
    
    public bool Grounded => groundDetector.IsGrounded;
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
    [SerializeField]
    private LayerMask LadderLayer;
    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private Vector2 colliderSize;
    private bool isOnSlope;
    [SerializeField]
    private float slopeCheckDistance = 0.5f;
    private float maxSlopeAngle = 70;
    private bool canWalkOnSlope;
    private Vector2 slopeNormalPerp;
    
    
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius;

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
        playerCollider = GetComponent<BoxCollider2D>();
        gameObject.layer = LayerMask.NameToLayer("Player");


        // Debug.Log(SkillData.Instance.SprintSkill + " " + SkillData.Instance.FlySprintSkill + " " + SkillData.Instance.ClimbSkill + " " + SkillData.Instance.AttackSkill);
        skills.Add("Jump", false);
        skills.Add("Climb", false);
        skills.Add("Sprint", false);
        skills.Add("Climb_Ladder", false);
        colliderSize = playerCollider.size;
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
        
        // Debug.Log(HaveSkill("Sprint") + " " + HaveSkill("Climb") + " " + HaveSkill("Jump") + HaveSkill("Climb_Ladder"));


        if (input.Interaction && skillType > 0)
        {
            canInteraction = true;
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
                case 4:
                    GetSkill("Climb_Ladder");
                  
                    break;
            }
            
        }
    }


    private void FixedUpdate()
    {
        // if (InMovingPlatform)
        // {
        //     transform.Translate(distance);
        // }
        CheckGround();
        SlopeCheck();
    }

    private void CheckGround()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position + new Vector3(transform.localScale.x * 0.35f, 0f, 0), groundCheckRadius, GroundLayer);
       
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
        // Debug.Log(IsGrounded + " " + isOnSlope + " " + canWalkOnSlope + " " + HaveSkill("Climb_Ladder"));
        if (IsGrounded && isOnSlope && canWalkOnSlope && HaveSkill("Climb_Ladder"))
        {
            // Debug.Log("yes");
            SetVelocity(new Vector3(speed * slopeNormalPerp.x * -input.AxisX, speed * slopeNormalPerp.y * -input.AxisX));
        }
        else if (Grounded)
        {
            SetVelocityX(speed * input.AxisX);
        }

        // SetVelocityX(speed * input.AxisX);
    }
    
    public void JumpMove(float speed)
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
        else if (other.transform.CompareTag("Interaction_Skill_ClimbLadder"))
        {
           
            skillType = 4;
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
    
    public void IsInLadder(float speed)
    {
        
    }

    public void MoveInLadder(float speed)
    {
        
    }
    
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, LadderLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, LadderLayer);
        Debug.DrawLine(checkPos, (Vector3)checkPos +  (Vector3)transform.right * slopeCheckDistance, Color.blue);
        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
        // Debug.Log("h:" + slopeSideAngle + isOnSlope);
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, LadderLayer);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && !input.Move)
        {
            rigidBody.sharedMaterial = fullFriction;
            playerCollider.sharedMaterial = fullFriction;
        }
        else
        {
            rigidBody.sharedMaterial = noFriction;
            playerCollider.sharedMaterial = noFriction;
        }
        
        // Debug.Log("v:" + slopeDownAngle + isOnSlope);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position + new Vector3(transform.localScale.x * 0.35f, 0f, 0), groundCheckRadius);
    }

}