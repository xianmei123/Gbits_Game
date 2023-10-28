using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    [SerializeField] float deceleration = 5f;

    public override void Enter()
    {
        base.Enter();
        
        currentSpeed = player.MoveSpeedX;
    }

    public override void LogicUpdate()
    { 
        if (player.PlayerDeath())
        {
            stateMachine.SwitchState(typeof(PlayerState_Death));
            return;
        }
        if (input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        else if (input.Jump && player.HaveSkill("Jump"))
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        else if (input.Sprint && player.HaveSkill("Sprint"))
        {
            if (player.canSprint)
            {
                stateMachine.SwitchState(typeof(PlayerState_Sprint));
            }
            
        }
        else if (player.CanClimb && player.HaveSkill("Climb"))
        {
            stateMachine.SwitchState(typeof(PlayerState_Climb));
            return;
        }
        else if (player.canInteraction)
        {
            stateMachine.SwitchState(typeof(PlayerState_Absorb));
        }
        // else if (input.Squat)
        // {
        //     stateMachine.SwitchState(typeof(PlayerState_Squat));
        // }
        // else if (input.Attack && player.AttackSkill)
        // {
        //     stateMachine.SwitchState(typeof(PlayerState_Attack));
        // }
        

        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
    }

    public override void PhysicUpdate()
    {
      
        player.SetVelocityX(currentSpeed * player.transform.localScale.x);
        // if (player.MoveSpeedY > 0)
        // {
        //     player.SetVelocityY(0);
        // }
        
    }
}