using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkIdleHorizontalState : SharkBaseState
{
    private float maxDistance;
    private float radiusDetection;
    private float speed;

    private Vector3 Pos1, Pos2;
    private Vector3 targetPos;
    private Vector3 originalPos;
    private bool GoLeft;
    private bool detectedPlayer;
    private float angle;

    private PlayerCoreSystem coreSystem;
    public SharkBaseState nextState;
    private Coroutine idleCoroutine;

    public SharkIdleHorizontalState(SharkBase shark, SharkStateMachine fsm, LayerMask playerMask, float maxDistance, float radiusDetection, float speed, float angle) : base(shark, fsm, playerMask)
    {
        this.maxDistance = maxDistance;
        this.radiusDetection = radiusDetection;
        this.speed = speed;
        this.angle = angle;
        detectedPlayer = false;
        GoLeft = true;

        Vector3 fishPosition = shark.transform.position;
        originalPos = fishPosition;
        Pos1 = new Vector3(fishPosition.x - maxDistance, fishPosition.y, fishPosition.z);
        Pos2 = new Vector3(fishPosition.x + maxDistance, fishPosition.y, fishPosition.z);
        targetPos = Pos1;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = coreSystem == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(shark.transform.position, radiusDetection);
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Idle Horizontal State for " + shark.gameObject.name);
        if (idleCoroutine != null)
        {
            shark.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }
        detectedPlayer = false;
        Debug.Log("Distance to Original Position " + Vector3.Distance(shark.transform.position, originalPos));
        if(Vector3.Distance(shark.transform.position, originalPos) > maxDistance)
        {
            shark.StartCoroutine(OnGoHome());
            shark.OnInvokeStopEncounter();
        }
        else
        {
            idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
        }
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
        if (!detectedPlayer) OnTryToDetect();
    }

    private IEnumerator OnIdlingWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            //shark.animator.SetBool("IsMoving", true);
            Debug.Log(Vector3.Distance(targetPos, shark.transform.position));
            while (Vector3.Distance(targetPos, shark.transform.position) > 0.05f)
            {
                shark.transform.position = Vector3.MoveTowards(shark.transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }

            GoLeft = !GoLeft;
            targetPos = GoLeft ? Pos1 : Pos2;
            //shark.animator.SetBool("IsMoving", false);
            yield return new WaitForSeconds(1f);
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
    private IEnumerator OnGoHome()
    {
        Debug.Log((shark.transform.position - originalPos).normalized.x);
        GoLeft = (shark.transform.position - originalPos).normalized.x > 0 ? true : false;
        yield return OnRotatingYAxis();
        targetPos = originalPos;
        shark.StopAllCoroutines();
        idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
        Debug.Log(targetPos);
    }
    private void OnTryToDetect()
    {
        Collider[] colliderList = Physics.OverlapSphere(shark.transform.position, radiusDetection, playerMask);
        if (colliderList.Length > 0)
        {
            foreach (Collider collider in colliderList)
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
        Vector3 direction = (coreSystem.transform.position - shark.transform.position).normalized;
        float targetY = direction.x > 0 ? 180 : 0;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        shark.transform.rotation = target;
    }
}
