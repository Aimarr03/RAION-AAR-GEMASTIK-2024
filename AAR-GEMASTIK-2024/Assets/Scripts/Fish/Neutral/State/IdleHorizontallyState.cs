using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleHorizontallyState : FishBaseState
{
    private float maxDistance;
    private float radiusDetection;
    private float speed;

    private Vector3 Pos1, Pos2;
    private Vector3 targetPos;
    private bool GoLeft;
    private bool detectedPlayer;
    private float angle;

    private PlayerCoreSystem coreSystem;
    public FishBaseState nextState;
    private Coroutine idleCoroutine;

    public IdleHorizontallyState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, float maxDistance, float radiusDetection, float speed, float angle) : base(fish, fsm, playerMask)
    {
        this.maxDistance = maxDistance;
        this.radiusDetection = radiusDetection;
        this.speed = speed;
        this.angle = angle;
        detectedPlayer = false;
        GoLeft = true;

        Vector3 fishPosition = fish.transform.position;
        Pos1 = new Vector3(fishPosition.x - maxDistance, fishPosition.y, fishPosition.z);
        Pos2 = new Vector3(fishPosition.x + maxDistance, fishPosition.y, fishPosition.z);
        targetPos = Pos1;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = coreSystem == null ? Color.red : Color.green;   
        Gizmos.DrawWireSphere(fish.transform.position, radiusDetection);
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
    }

    public override void OnUpdateState()
    {
        if(!detectedPlayer)OnTryToDetect();
    }

    private IEnumerator OnIdlingWithDelay()
    {
        yield return new WaitForSeconds(1.8f);
        while (true)
        {
            fish.animator.SetBool("IsMoving", true);
            while (Vector3.Distance(targetPos, fish.transform.position) > 0.05f)
            {
                fish.transform.position = Vector3.MoveTowards(fish.transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }

            GoLeft = !GoLeft;
            targetPos = GoLeft ? Pos1 : Pos2;
            fish.animator.SetBool("IsMoving", false);
            yield return new WaitForSeconds(1.75f);
            yield return OnRotatingYAxis();
        }
    }
    private IEnumerator OnRotatingYAxis()
    {
        fish.animator.SetBool("IsMoving", true);
        float targetY = GoLeft ? 0 : 180;
        Quaternion target = Quaternion.Euler(0,targetY, 0);
        while(Quaternion.Angle(fish.transform.rotation, target) > 1f)
        {
            fish.transform.rotation = Quaternion.RotateTowards(fish.transform.rotation, target, speed * 2.2f * Time.fixedDeltaTime);
            yield return null;
        }
        fish.transform.rotation = target;
    }
    private void OnTryToDetect()
    {
        Collider[] colliderList = Physics.OverlapSphere(fish.transform.position, radiusDetection, playerMask);
        if (colliderList.Length > 0)
        {
            foreach (Collider collider in colliderList)
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
