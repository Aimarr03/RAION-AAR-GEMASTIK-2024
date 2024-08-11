using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCircleState : FishBaseState
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 boxSize;
    private float radiusDistance;
    private float speed;
    private bool detectedPlayer;
    private bool GoLeft;
    private float angle;
    private float radiusDetection;

    private PlayerCoreSystem coreSystem;
    public FishBaseState nextState;
    private Coroutine idleCoroutine;
    public IdleCircleState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, float radiusDetection, float angle, float radiusDistance, float speed) : base(fish, fsm, playerMask)
    {
        originalPosition = fish.transform.position;
        this.radiusDetection = radiusDetection;
        this.angle = angle;
        this.radiusDistance = radiusDistance;
        this.speed = speed;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = coreSystem == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(fish.transform.position, radiusDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originalPosition, radiusDetection);
        if (targetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(fish.transform.position, targetPosition);
        }
    }

    public override void OnEnterState()
    {
        if (idleCoroutine != null)
        {
            fish.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }
        detectedPlayer = false;
        idleCoroutine = fish.StartCoroutine(OnIdlingWithDelay());
    }

    public override void OnExitState()
    {
        if (idleCoroutine != null)
        {
            fish.StopCoroutine(idleCoroutine);
        }
        fish.animator.SetTrigger("Surprise");
    }

    public override void OnUpdateState()
    {
        if (!detectedPlayer) OnTryToDetect();
    }
    private IEnumerator OnIdlingWithDelay()
    {
        yield return new WaitForSeconds(1.8f);
        while (true)
        {
            //fish.animator.SetBool("IsMoving", true);
            while (Vector3.Distance(targetPosition, fish.transform.position) > 0.05f)
            {
                fish.transform.position = Vector3.MoveTowards(fish.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            float maxDistanceX, maxDistanceY;
            GoLeft = !GoLeft;
            if (GoLeft)
            {
                maxDistanceX = originalPosition.x - radiusDistance;
                maxDistanceY = originalPosition.y - radiusDistance;
            }
            else
            {
                maxDistanceX = originalPosition.x + radiusDistance;
                maxDistanceY = originalPosition.y + radiusDistance;
            }
            maxDistanceX = Random.Range(originalPosition.x, maxDistanceX);
            maxDistanceY = Random.Range(originalPosition.y, maxDistanceY);
            targetPosition = new Vector3(maxDistanceX, maxDistanceY);

            yield return new WaitForSeconds(2.75f);
            yield return OnRotatingYAxis();
        }
    }
    private IEnumerator OnRotatingYAxis()
    {
        //fish.animator.SetBool("IsMoving", true);
        float targetY = GoLeft ? 0 : 180;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        while (Quaternion.Angle(fish.transform.rotation, target) > 1f)
        {
            fish.transform.rotation = Quaternion.RotateTowards(fish.transform.rotation, target, speed * 3.5f * Time.fixedDeltaTime);
            yield return null;
        }
        fish.transform.rotation = target;
    }
    private void OnTryToDetect()
    {
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(fish.transform.position, radiusDetection, playerMask);
        if (colliderList.Length > 0)
        {
            foreach (Collider2D collider in colliderList)
            {
                if (collider.TryGetComponent(out PlayerCoreSystem coreSystem))
                {

                    Vector3 direction = (coreSystem.transform.position - fish.transform.position).normalized;
                    if (Vector3.Angle(-fish.transform.right, direction) < angle / 2)
                    {
                        Debug.Log("On Detected Player");
                        this.coreSystem = coreSystem;
                        detectedPlayer = true;
                        fsm.OnTransitionState(nextState);
                        if (idleCoroutine != null)
                        {
                            fish.StopCoroutine(idleCoroutine);
                            OnResetRotation();
                        }
                    }

                }
            }
        }
        else
        {
            coreSystem = null;
            if (idleCoroutine == null)
            {
                idleCoroutine = fish.StartCoroutine(OnIdlingWithDelay());
            }
        }
    }
    private void OnResetRotation()
    {
        Vector3 direction = (coreSystem.transform.position - fish.transform.position).normalized;
        float targetY = direction.x > 0 ? 180 : 0;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        fish.transform.rotation = target;
    }
}
