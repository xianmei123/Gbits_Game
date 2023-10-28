using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/ClimbLadder", fileName = "PlayerState_ClimbLadder")]
public class PlayerState_ClimbLadder : PlayerState
{

    private float speed = 3.0f;
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
       
        if (input.Move && input.AxisX * player.transform.localScale.x < 0)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
        
        else if (input.MoveUpDown)
        {
            stateMachine.SwitchState(typeof(PlayerState_ClimbMove));
        }
        
        
        
    }

    public override void PhysicUpdate()
    {
        player.MoveInLadder(speed);
       
    }

    public override void Exit()
    {
        player.SetUseGravity(1.0f);
    }
}