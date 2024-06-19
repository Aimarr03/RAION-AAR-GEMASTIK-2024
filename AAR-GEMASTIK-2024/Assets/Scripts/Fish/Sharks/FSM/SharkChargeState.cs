using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkChargeState : SharkBaseState
{
    private Transform attackCenter;
    private float speed;
    private int damage;
    private Vector3 targetPosition;
    private Vector3 targetWithOffsetPosition;
    private Vector2 size = new Vector2(6, 3);
    private bool isHitting = false;
    private bool isOnRightDirection = false;
    private PlayerCoreSystem playerCoreSystem;

    public SharkBaseState OnStopChargingState;
    public SharkBaseState OnAlternativeAttackState;

    private Coroutine OnChargingCoroutine;

    public SharkChargeState(SharkBase shark, SharkStateMachine fsm, LayerMask playerMask, float speed, int damage, Transform attackCenter) : base(shark, fsm, playerMask)
    {
        isHitting = false;
        this.speed = speed;
        this.damage = damage;
        this.attackCenter = attackCenter;
    }

    public override void OnEnterState()
    {
        shark.animator.SetBool("ChargeBite", true);
        isHitting = false;
        Collider[] colliders = Physics.OverlapSphere(shark.transform.position, 30, playerMask);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
            {
                this.playerCoreSystem = playerCoreSystem;
                targetPosition = this.playerCoreSystem.transform.position;
            }
        }
        OnChargingCoroutine = shark.StartCoroutine(OnCharging());
    }

    public override void OnExitState()
    {
        shark.animator.SetBool("ChargeBite", false);
        playerCoreSystem = null;
        if (OnChargingCoroutine != null)
        {
            shark.StopCoroutine(OnChargingCoroutine);
            OnChargingCoroutine = null;
        }
    }

    public override void OnUpdateState()
    {
        if (isHitting) OnHit();
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(shark.transform.position, targetWithOffsetPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shark.transform.position, size);
    }
    private void OnHit()
    {
        Collider[] colliders = Physics.OverlapBox(attackCenter.position, size, Quaternion.identity, playerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamagable idamagable)) idamagable.TakeDamage(damage);
        }
    }
    private IEnumerator OnCharging()
    {
        float x_direction = (targetPosition - shark.transform.position).x;
        if(x_direction < 0 && isOnRightDirection || x_direction > 0 && !isOnRightDirection)
        {
            yield return OnRotatingYAxis(x_direction);
        }
        yield return new WaitForSeconds(0.6f);
        isHitting = true;
        Vector3 offsetPosition = new Vector3(2, 2, 0);
        Vector3 targetWithOffsetPosition = x_direction < 0 ? targetPosition - offsetPosition : targetPosition + offsetPosition;
        this.targetWithOffsetPosition = targetWithOffsetPosition;
        while (Vector3.Distance(shark.transform.position, targetWithOffsetPosition) > 1f)
        {
            shark.transform.position = Vector3.MoveTowards(shark.transform.position, targetWithOffsetPosition, speed * Time.deltaTime);
            yield return null;
        }
        shark.StartCoroutine(OnCooldown());
    }
    private IEnumerator OnRotatingYAxis(float x_direction)
    {
        float y_angle = x_direction < 0 ? 0 : 180;
        Quaternion targetAngle = Quaternion.Euler(shark.transform.rotation.x, y_angle, shark.transform.rotation.z);
        while(Quaternion.Angle(shark.transform.rotation, targetAngle) > 5f)
        {
            shark.transform.rotation = Quaternion.RotateTowards(shark.transform.rotation, targetAngle, speed * Time.fixedDeltaTime);
            yield return null;
        }
        shark.transform.rotation = targetAngle;
        isOnRightDirection = !isOnRightDirection;
    }
    private IEnumerator OnCooldown()
    {
        if (OnChargingCoroutine != null)
        {
            shark.StopCoroutine(OnChargingCoroutine);
            OnChargingCoroutine = null;
        }
        isHitting = false;
        targetPosition = playerCoreSystem.transform.position;
        float distance = Vector3.Distance(shark.transform.position, targetPosition);
        yield return new WaitForSeconds(1.2f);
        OnChargingCoroutine = shark.StartCoroutine(OnCharging());
        Debug.Log(distance);
        if (distance < 12f)
        {
            Debug.Log("Engage Chasing");
            fsm.OnTransitionState(OnAlternativeAttackState);
        }
        else if (distance < 24f)
        {
            Debug.Log("Engage Re-Charging");
            OnChargingCoroutine = shark.StartCoroutine(OnCharging());
        }
        else
        {
            Debug.Log("Engage Stop");
            fsm.OnTransitionState(OnStopChargingState);
        }
    }
}
