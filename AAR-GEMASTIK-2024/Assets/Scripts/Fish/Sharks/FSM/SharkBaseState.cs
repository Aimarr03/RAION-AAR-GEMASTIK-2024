using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SharkBaseState
{
    protected SharkBase shark;
    protected SharkStateMachine fsm;
    protected LayerMask playerMask;

    public event Action OnTriggerTransition;
    public SharkBaseState(SharkBase shark, SharkStateMachine fsm, LayerMask playerMask)
    {
        this.shark = shark;
        this.fsm = fsm;
        this.playerMask = playerMask;
    }
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void OnUpdateState();
    public abstract void OnDrawGizmos();
}
