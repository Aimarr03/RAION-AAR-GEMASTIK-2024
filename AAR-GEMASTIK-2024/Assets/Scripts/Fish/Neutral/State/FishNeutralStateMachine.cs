using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNeutralStateMachine
{
    private FishBaseState fishBaseState;
    private FishNeutralBase fish;

    public FishNeutralStateMachine(FishNeutralBase fish)
    {
        this.fish = fish;
    }
    public void OnUpdate()
    {
        fishBaseState?.OnUpdateState();
    }
    public void OnDraw()
    {
        fishBaseState?.OnDrawGizmos();
    }
    public void OnTransitionState(FishBaseState nextState)
    {
        fishBaseState.OnExitState();
        fishBaseState = nextState;
        fishBaseState.OnEnterState();        
    }
    public void InitializeState(FishBaseState nextState)
    {
        fishBaseState = nextState;
        fishBaseState.OnEnterState();
    }

    
}
