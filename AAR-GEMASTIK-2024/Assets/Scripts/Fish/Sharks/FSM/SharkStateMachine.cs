using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SharkStateMachine
{
    private SharkBaseState sharkBaseState;
    private SharkBase shark;
    private bool onTransitioning;

    public SharkStateMachine(SharkBase shark)
    {
        this.shark = shark;
    }
    public void OnUpdate()
    {
        if (shark.GetIsKnockout()) return;
        if (onTransitioning) return;
        sharkBaseState?.OnUpdateState();
    }
    public void OnDraw()
    {
        sharkBaseState?.OnDrawGizmos();
    }
    public void OnTransitionState(SharkBaseState nextState)
    {
        if(onTransitioning) return;
        OnTransitionStateWithDelay(nextState);
        //shark.StartCoroutine(OnTransitionStateWithDelay(nextState));
    }
    private async void OnTransitionStateWithDelay(SharkBaseState nextState)
    {
        onTransitioning = true;
        sharkBaseState.OnExitState();
        sharkBaseState = nextState;
        Debug.Log(sharkBaseState);
        await Task.Delay(750);
        onTransitioning = false;
        sharkBaseState.OnEnterState();
    }
    public void InitializeState(SharkBaseState nextState)
    {
        sharkBaseState = nextState;
        sharkBaseState.OnEnterState();
    }
}
