using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float moveSpeed = 5f;

    public override void LogicUpdate()
    {
        // Debug.Log(player.MoveSpeedY);
        if (player.PlayerDeath())
        {
            stateMachine.SwitchState(typeof(PlayerState_Death));
            return;
        }
        if (player.IsGrounded)
        {
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }

        else if (player.CanClimb && player.HaveSkill("Climb"))
        {
            stateMachine.SwitchState(typeof(PlayerState_Climb));
            return;
        }
        
        else if (input.Sprint && player.HaveSkill("Sprint"))
        {
            if (player.canSprint)
            {
                stateMachine.SwitchState(typeof(PlayerState_FlySprint));
            }
        }
        else if (input.Jump && player.HaveSkill("Jump"))
        {
            input.SetJumpInputBufferTimer();
        }
    }

    public override void PhysicUpdate()
    {
        player.JumpMove(moveSpeed);
        player.SetVelocityY(speedCurve.Evaluate(StateDuration));
    }
}