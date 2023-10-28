using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : ScriptableObject, IState
{
    [SerializeField] string stateName;
    [SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f;

    float stateStartTime;

    // List<int> stateHashs;
    //
    // int stateHash;
    Dictionary<string, List<int>> stateHashs = new Dictionary<string, List<int>>();
    
    protected float currentSpeed;

    protected Animator animator;

    protected PlayerController player;
    protected PlayerInput input;
    protected PlayerStateMachine stateMachine;

    // protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    protected bool IsAnimationFinished => StateDuration >= 0.5;

    protected float StateDuration => Time.time - stateStartTime;

    void OnEnable()
    {
        if (stateHashs.ContainsKey(stateName))
        {
            return;
        }
        List<int> stateHashList = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            stateHashList.Add(Animator.StringToHash(stateName + i));
            // Debug.Log(stateName + i + " " + Animator.StringToHash(stateName + i));
        }

        stateHashs[stateName] = stateHashList;
        // Debug.Log("size" + stateHashs.Count);
        // foreach (var key in stateHashs.Keys)
        // {
        //     Debug.Log(key + " keysize " + stateHashs[key].Count);
        // }
    }

    public void Initialize(Animator animator, PlayerController player, PlayerInput input, PlayerStateMachine stateMachine)
    {
        this.animator = animator;
        this.player = player;
        this.input = input;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        // foreach (var key in stateHashs.Keys)
        // {
        //     Debug.Log(key + " keysize " + stateHashs[key].Count);
        // }
        // Debug.Log(stateHashs[stateName][player.type - 1] + " " + player.type + " " + stateHashs.Count);
        animator.CrossFade(stateHashs[stateName][player.type - 1], transitionDuration);
        stateStartTime = Time.time;
    }

    public virtual void Exit()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
        
    }
}