using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Absorb", fileName = "PlayerState_Absorb")]
public class PlayerState_Absorb : PlayerState
{

    [SerializeField] public AudioClip AbsorbClip;
  
    public override void Enter()
    {
        base.Enter();
        SoundEffectPlayer.AudioSource.clip = AbsorbClip;
        SoundEffectPlayer.AudioSource.pitch = 3;
        SoundEffectPlayer.AudioSource.time = 2;
        SoundEffectPlayer.AudioSource.volume = 0.5f;
        SoundEffectPlayer.AudioSource.Play();
        // SoundEffectPlayer.AudioSource.PlayOneShot(AbsorbClip);
        player.SetVelocity(new Vector3(0.0f, 0.0f, 0.0f));

        player.canInteraction = false;

    }

    public override void LogicUpdate()
    {
        if (player.PlayerDeath())
        {
            stateMachine.SwitchState(typeof(PlayerState_Death));
            return;
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        
    }

    public override void PhysicUpdate()
    {
        player.SetVelocityX(0);
       
    }

    public override void Exit()
    {
     
    }
}