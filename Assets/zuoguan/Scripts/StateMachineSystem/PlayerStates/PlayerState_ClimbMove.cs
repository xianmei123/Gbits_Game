using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/ClimbMove", fileName = "PlayerState_ClimbMove")]
public class PlayerState_ClimbMove : PlayerState
{
    [SerializeField] float climbSpeed = 5f;
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(new Vector3(0.0f, 0.0f, 0.0f));
        
        player.SetUseGravity(0.0f);
        
    }

    public override void LogicUpdate()
    {
        if (player.PlayerDeath())
        {
            stateMachine.SwitchState(typeof(PlayerState_Death));
            return;
        }
        // if (input.StopJump || player.IsFalling)
        // {
        //     stateMachine.SwitchState(typeof(PlayerState_Fall));
        // }
        //
        // if (input.Sprint)
        // {
        //     if (player.canFlySprint)
        //     {
        //         stateMachine.SwitchState(typeof(PlayerState_FlySprint));
        //     }
        // }
        if (input.Move && input.AxisX * player.transform.localScale.x < 0)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
        
        if (input.Jump && player.HaveSkill("Jump"))
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }
        
    }

    public override void PhysicUpdate()
    {
        player.SetVelocityX(0);
        player.MoveUpDown(climbSpeed);
    }

    public override void Exit()
    {
        player.SetUseGravity(1.0f);
    }
}