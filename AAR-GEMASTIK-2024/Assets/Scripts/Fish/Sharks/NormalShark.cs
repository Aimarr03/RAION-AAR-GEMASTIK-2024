using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NormalShark : SharkBase
{
    private SharkIdleHorizontalState idleState;
    private SharkChaseState chaseState;

    [Header("Idle State")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float radiusDetection, speed, angle;

    [Header("Chase State")]
    [SerializeField] private float linearSpeed;
    [SerializeField] private float rotatingSpeed, maxSpeed, distanceAggro;
    [SerializeField] private Transform centerCheckDistance;
    [SerializeField] private int damage;

    protected override void Awake()
    {
        base.Awake();
        idleState = new SharkIdleHorizontalState(this, stateMachine, playerMask, maxDistance, radiusDetection, speed, angle);
        chaseState = new SharkChaseState(this, stateMachine, playerMask, linearSpeed, rotatingSpeed, maxSpeed, distanceAggro, centerCheckDistance, damage);

        idleState.nextState = chaseState;
        chaseState.nextState = idleState;
        stateMachine.InitializeState(idleState);
    }
    public override void Update()
    {
        base.Update();
    }

    
    public override void AddSuddenForce(Vector3 directiom, float forcePower)
    {
        
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator GetSlowed(float duration, float multilpier)
    {
        float bufferLinearSpeed = linearSpeed;
        float bufferMaxSpeed = maxSpeed;
        linearSpeed *= 1- multilpier;
        maxSpeed *= 1 - multilpier;
        float bufferDuration = 0f;
        while (bufferDuration < duration)
        {
            bufferDuration += Time.deltaTime;
            yield return null;
        }
        linearSpeed = bufferLinearSpeed;
        maxSpeed = bufferMaxSpeed;
    }
}

