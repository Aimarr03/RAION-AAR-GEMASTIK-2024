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
    private bool isResting = false;
    private bool goingHome=false;

    public SharkIdleHorizontalState(SharkBase shark, SharkStateMachine fsm, LayerMask playerMask, float maxDistance, float radiusDetection, float speed, float angle) : base(shark, fsm, playerMask)
    {
        this.maxDistance = maxDistance;
        this.radiusDetection = radiusDetection;
        this.speed = speed;
        this.angle = angle;
        detectedPlayer = false;
        GoLeft = true;
        isResting = false;

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
        /*if (idleCoroutine != null)
        {
            shark.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }*/
        detectedPlayer = false;
        Debug.Log("Distance to Original Position " + Vector3.Distance(shark.transform.position, originalPos));
        goingHome = Vector3.Distance(shark.transform.position, originalPos) > maxDistance;
        Debug.Log("Going Home ? " + goingHome);
        /*if (Vector3.Distance(shark.transform.position, originalPos) > maxDistance)
        {
            //shark.StartCoroutine(OnGoHome());
            shark.OnInvokeStopEncounter();
        }*/
        /*else
        {
            idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
        }*/
        StartResting(0.3f);
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
        shark.rigidBody.velocity = Vector2.zero;
        shark.OnInvokeEncounter();
        shark.onTakeDamage -= Shark_onTakeDamage;
    }

    public override void OnUpdateState()
    {
        //Debug.Log($"ISRESTING {isResting}, DETECTED PLATER {detectedPlayer}, GOING HOME {goingHome}");
        if (isResting) return;
        if (!detectedPlayer) OnTryToDetect();
        if (!goingHome) OnIdlingWithDelay();
        else OnGoingHome();
    }

    private void OnIdlingWithDelay()
    {
        Debug.Log(Vector3.Distance(targetPos, shark.transform.position));
        Vector2 direction = GoLeft ? Vector2.left : Vector2.right;
        bool hit = Physics.Raycast(shark.transform.position, direction, 4f);
        Debug.Log("Hit Something " + hit);
        if(Vector3.Distance(targetPos, shark.transform.position) > 0.05f && !hit)
        {
            shark.transform.position = Vector3.MoveTowards(shark.transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            GoLeft = !GoLeft;
            targetPos = GoLeft ? Pos1 : Pos2;
            //shark.animator.SetBool("IsMoving", false);
            StartResting(0.35f);
            OnRotatingYAxis();
        }
    }
    private void OnGoingHome()
    {
        if(Vector3.Distance(shark.transform.position, originalPos) > 0.2f)
        {
            shark.transform.position = Vector3.MoveTowards(shark.transform.position, originalPos, speed * Time.deltaTime);
        }
        else
        {
            goingHome = false;
        }
    }
    private void OnRotatingYAxis()
    {
        //shark.animator.SetBool("IsMoving", true);
        float targetY = GoLeft ? 0 : 180;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        /*while (Quaternion.Angle(shark.transform.rotation, target) > 1f)
        {
            shark.transform.rotation = Quaternion.RotateTowards(shark.transform.rotation, target, speed * 2.2f * Time.fixedDeltaTime);
            
        }*/
        shark.transform.rotation = target;
        StartResting(0.25f);
    }
    /*private IEnumerator OnGoHome()
    {
        Debug.Log((shark.transform.position - originalPos).normalized.x);
        GoLeft = (shark.transform.position - originalPos).normalized.x > 0 ? true : false;
        //yield return OnRotatingYAxis();
        targetPos = originalPos;
        shark.StopAllCoroutines();
        //idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
        //Debug.Log(targetPos);
    }*/
    private void OnTryToDetect()
    {
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(shark.transform.position, radiusDetection, playerMask);
        if (colliderList.Length > 0)
        {
            foreach(Collider2D collider in colliderList)
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
                        /*if (idleCoroutine != null)
                        {
                            shark.StopCoroutine(idleCoroutine);
                            OnResetRotation();
                        }*/
                    }

                }
            }
        }
        else
        {
            coreSystem = null;
            /*if (idleCoroutine == null)
            {
                idleCoroutine = shark.StartCoroutine(OnIdlingWithDelay());
            }*/
        }
    }
    private void OnResetRotation()
    {
        Vector3 direction = (coreSystem.transform.position - shark.transform.position).normalized;
        float targetY = direction.x > 0 ? 180 : 0;
        Quaternion target = Quaternion.Euler(0, targetY, 0);
        shark.transform.rotation = target;
    }
    private IEnumerator StartResting(float delay)
    {
        isResting = true;
        yield return new WaitForSeconds(delay);
        isResting = false;
    }
}
