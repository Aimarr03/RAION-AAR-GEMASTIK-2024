using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkIdleBoxState : SharkBaseState
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 boxSize;
    private float x, y;
    private float speed;
    private bool detectedPlayer;
    private bool GoLeft;
    private bool canDetect;
    private float angle;
    private float radiusDetection;

    private PlayerCoreSystem coreSystem;
    public SharkBaseState nextState;
    private Coroutine idleCoroutine;
    public SharkIdleBoxState(SharkBase shark, SharkStateMachine fsm, LayerMask playerMask, float radiusDetection, float angle, float x, float y, float speed) : base(shark, fsm, playerMask)
    {
        originalPosition = shark.transform.position;
        this.radiusDetection = radiusDetection;
        this.angle = angle;
        this.x = x;
        this.y = y;
        this.speed = speed;
        boxSize = new Vector3(x, y, 1);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = coreSystem == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(shark.transform.position, radiusDetection);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(originalPosition, boxSize);
        if (targetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shark.transform.position, targetPosition);
        }
    }

    public override void OnEnterState()
    {
        canDetect = false;
        targetPosition = originalPosition;
        if(Vector3.Distance(shark.transform.position, targetPosition) > x)
        {
            detectedPlayer = false;
            shark.StartCoroutine(OnReturningHome());
            return;
        }
        if (idleCoroutine != null)
        {
            shark.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }
        detectedPlayer = false;
        canDetect = true;
        idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
        shark.onTakeDamage += Shark_onTakeDamage;
    }

    private void Shark_onTakeDamage(bool arg1, float arg2)
    {
        fsm.OnTransitionState(nextState);
    }

    public override void OnExitState()
    {
        if (idleCoroutine != null)
        {
            shark.StopCoroutine(idleCoroutine);
        }
        shark.animator.SetTrigger(shark.Surprise);
        shark.OnInvokeEncounter();
        shark.onTakeDamage -= Shark_onTakeDamage;
    }

    public override void OnUpdateState()
    {
        if (!detectedPlayer && canDetect) OnTryToDetect();
    }
    private IEnumerator OnIdlingWithDelay()
    {
        yield return new WaitForSeconds(1.8f);
        while (true)
        {
            shark.animator.SetBool("IsMoving", true);
            while (Vector3.Distance(targetPosition, shark.transform.position) > 0.05f)
            {
                shark.transform.position = Vector3.MoveTowards(shark.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            float maxDistanceX, maxDistanceY;
            GoLeft = !GoLeft;
            if (GoLeft)
            {
                maxDistanceX = originalPosition.x - x;
                maxDistanceY = originalPosition.y - y;
            }
            else
            {
                maxDistanceX = originalPosition.x + x;
                maxDistanceY = originalPosition.y + y;
            }
            maxDistanceX = UnityEngine.Random.Range(originalPosition.x, maxDistanceX);
            maxDistanceY = UnityEngine.Random.Range(originalPosition.y, maxDistanceY);
            targetPosition = new Vector3(maxDistanceX, maxDistanceY);

            yield return new WaitForSeconds(2.75f);
            yield return OnRotatingYAxis();
        }
    }
    private IEnumerator OnRotatingYAxis()
    {
        shark.animator.SetBool("IsMoving", true);
        float targetY = GoLeft ? 0 : 180;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        while (Quaternion.Angle(shark.transform.rotation, target) > 1f)
        {
            shark.transform.rotation = Quaternion.RotateTowards(shark.transform.rotation, target, speed * 2.2f * Time.fixedDeltaTime);
            yield return null;
        }
        shark.transform.rotation = target;
    }
    private void OnTryToDetect()
    {
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(shark.transform.position, radiusDetection, playerMask);
        if (colliderList.Length > 0)
        {
            foreach (Collider2D collider in colliderList)
            {
                if (collider.TryGetComponent(out PlayerCoreSystem coreSystem))
                {

                    Vector3 direction = (coreSystem.transform.position - shark.transform.position).normalized;
                    if (Vector3.Angle(-shark.transform.right, direction) < angle / 2)
                    {
                        Debug.Log("On Detected Player");
                        this.coreSystem = coreSystem;
                        detectedPlayer = true;
                        fsm.OnTransitionState(nextState);
                        if (idleCoroutine != null)
                        {
                            shark.StopCoroutine(idleCoroutine);
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
                idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
            }
        }
    }
    private void OnResetRotation()
    {
        if (coreSystem == null) return;
        Vector3 direction = (coreSystem.transform.position - shark.transform.position).normalized;
        float targetY = direction.x > 0 ? 180 : 0;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        shark.transform.rotation = target;
    }
    private IEnumerator OnReturningHome()
    {
        Debug.Log("Returning Home");
        while(Vector3.Distance(shark.transform.position, targetPosition) > 0.5f)
        {
            shark.transform.position = Vector3.MoveTowards(shark.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        canDetect = true;
        if (idleCoroutine != null)
        {
            shark.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }
        detectedPlayer = false;
        Debug.Log($"Can Detect {!detectedPlayer && canDetect}");
        idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
    }
}
