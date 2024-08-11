using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SharkChaseState : SharkBaseState
{
    private float radius = 3;
    private bool isRotating = false;
    private float rotatingSpeed = 8f;
    private float linearSpeed = 2f;
    private float maxSpeed = 4f;
    private bool isOnRightDirection = false;
    private bool onCooldown;
    
    private float distanceAggro;
    private int damage;
    
    private Vector3 direction;
    private Vector3 originalPosition;
    private Transform centerCheckDistance;
    private PlayerCoreSystem playerCoreSystem;
    public SharkBaseState nextState;
    public SharkChaseState(SharkBase shark, SharkStateMachine sharkStateMachine, LayerMask playerLayerMask,
        float linearSpeed, float rotatingSpeed, float maxSpeed, float distanceAggro, Transform centerCheckDistance, int damage) : base(shark, sharkStateMachine, playerLayerMask)
    {
        originalPosition = shark.transform.position;
        this.linearSpeed = linearSpeed;
        this.rotatingSpeed = rotatingSpeed;
        this.maxSpeed = maxSpeed;
        this.distanceAggro = distanceAggro;
        this.centerCheckDistance = centerCheckDistance;
        this.damage = damage;
        onCooldown = false;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centerCheckDistance.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerCheckDistance.position, distanceAggro);
    }

    public override void OnEnterState()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(shark.transform.position, 350f, playerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerCoreSystem coreSystem))
            {
                playerCoreSystem = coreSystem;
                break;
            }
        }
        shark.animator.SetTrigger(shark.Hit);
        Debug.Log(playerCoreSystem);
    }

    public override void OnExitState()
    {
        shark.StopAllCoroutines();
        playerCoreSystem = null;
        onCooldown = false;
    }

    public override void OnUpdateState()
    {
        if (onCooldown) return;
        if (playerCoreSystem == null) return;
        CheckDistance();
        CheckPlayer();
        WithinBiteRange();
    }
    private void CheckDistance()
    {
        if (playerCoreSystem == null) return;
        if (Vector3.Distance(shark.transform.position, originalPosition) > 80f)
        {
            fsm.OnTransitionState(nextState);
        }
        else if(Vector3.Distance(shark.transform.position, playerCoreSystem.transform.position) > 60f) fsm.OnTransitionState(nextState);
    }
    private void CheckPlayer()
    {
        if (playerCoreSystem == null) return;
        direction = (playerCoreSystem.transform.position - shark.transform.position).normalized;
        //Debug.Log($"DIRECTION = {direction}");
        HorizontalMove(direction);
        VerticalMovement(direction);
    }
    private void WithinBiteRange()
    {
        //Debug.Log(Vector3.Distance(playerCoreSystem.transform.position, centerCheckDistance.position));
        //Debug.Log((Vector3.Distance(playerCoreSystem.transform.position, centerCheckDistance.position) > distanceAggro) + " Aggro Within Range");
        if (playerCoreSystem == null) return;
        if (Vector3.Distance(playerCoreSystem.transform.position, shark.transform.position) > distanceAggro || onCooldown) return;
        OnBiting();
    }
    private void HorizontalMove(Vector3 direction)
    {
        if (Vector3.Distance(playerCoreSystem.transform.position, shark.transform.position) < 1) return;
        if (isRotating)
        {
            RotatingOnYAxis();
            return;
        }
        if (direction.x == 0) return;
        float x_direction = direction.x > 0 ? 1 : -1;
        if (CheckHasToRotate(x_direction))
        {
            isRotating = true;
            return;
        }

        shark.rigidBody.velocity += linearSpeed * Time.fixedDeltaTime * x_direction * Vector2.right;
        shark.rigidBody.velocity = Vector2.ClampMagnitude(shark.rigidBody.velocity, maxSpeed);
    }
    private bool CheckHasToRotate(float x)
    {
        if (!isOnRightDirection && x < 0) return true;
        if (isOnRightDirection && x > 0) return true;
        else return false;
    }
    private void RotatingOnYAxis()
    {
        if (!isRotating) return;
        shark.rigidBody.velocity = Vector3.zero;
        float y_axis_target = isOnRightDirection ? 180 : 0;
        Quaternion targetRotate = Quaternion.Euler(0, y_axis_target, 0);
        shark.transform.rotation = Quaternion.RotateTowards(shark.transform.rotation, targetRotate, rotatingSpeed * 2.4f * Time.fixedDeltaTime);
        if (Quaternion.Angle(shark.transform.rotation, targetRotate) < 5f)
        {
            shark.transform.rotation = targetRotate;
            isRotating = false;
            isOnRightDirection = !isOnRightDirection;
            Debug.Log(isRotating);
        }
    }
    private void VerticalMovement(Vector3 direction)
    {
        if (isRotating) return;
        float z_input = direction.y;
        if (z_input != 0)
        {
            Vector2 outputVelocity = Vector2.up * (linearSpeed / 2 * Time.fixedDeltaTime * z_input);
            shark.rigidBody.velocity += outputVelocity;
        }
    }
    private void OnBiting()
    {
        shark.animator.SetTrigger("Bite");
        float angle = 75f;
        float bitingRange = distanceAggro + 1.5f;
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(shark.transform.position, bitingRange, playerMask);
        foreach (Collider2D collider in colliderList)
        {
            if (collider.TryGetComponent(out PlayerCoreSystem coreSystem))
            {

                Vector3 direction = (coreSystem.transform.position - shark.transform.position).normalized;
                if (Vector3.Angle(-shark.transform.right, direction) < angle / 2)
                {
                    playerCoreSystem.TakeDamage(damage);
                }
            }
        }
        onCooldown = true;
        CooldownAttack();
    }
    private async void CooldownAttack()
    {
        await Task.Delay(1500);
        onCooldown = false;
    }
}
