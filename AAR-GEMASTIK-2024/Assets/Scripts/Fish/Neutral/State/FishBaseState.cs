using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBaseState
{
    protected FishNeutralBase fish;
    protected FishNeutralStateMachine fsm;
    protected LayerMask playerMask;

    //public event Action OnTriggerTransition;
    public FishBaseState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask)
    {
        this.fish = fish;
        this.fsm = fsm;
        this.playerMask = playerMask;
    }


    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void OnUpdateState();
    public abstract void OnDrawGizmos();
}
