using System.Collections;
using System.Collections.Generic;
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
        if (onTransitioning) return;
        sharkBaseState?.OnUpdateState();
    }
    public void OnDraw()
    {
        sharkBaseState?.OnDrawGizmos();
    }
    public void OnTransitionState(SharkBaseState nextState)
    {
        shark.StartCoroutine(OnTransitionStateWithDelay(nextState));
    }
    private IEnumerator OnTransitionStateWithDelay(SharkBaseState nextState)
    {
        onTransitioning = true;
        sharkBaseState.OnExitState();
        sharkBaseState = nextState;
        Debug.Log(nextState);
        yield return new WaitForSeconds(0.75f);
        onTransitioning = false;
        sharkBaseState.OnEnterState();
    }
    public void InitializeState(SharkBaseState nextState)
    {
        sharkBaseState = nextState;
        sharkBaseState.OnEnterState();
    }
}
