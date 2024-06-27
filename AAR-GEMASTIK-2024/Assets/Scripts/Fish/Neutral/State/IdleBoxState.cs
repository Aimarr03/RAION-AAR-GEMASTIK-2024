using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class IdleBoxState : FishBaseState
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 boxSize;
    private float x, y;
    private float speed;
    private bool detectedPlayer;
    private bool GoLeft;
    private float angle;
    private float radiusDetection;

    private PlayerCoreSystem coreSystem;
    public FishBaseState nextState;
    private Coroutine idleCoroutine;
    public IdleBoxState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, float radiusDetection, float angle ,float x, float y, float speed) : base(fish, fsm, playerMask)
    {
        originalPosition = fish.transform.position;
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
        Gizmos.DrawWireSphere(fish.transform.position, radiusDetection);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(originalPosition, boxSize);
        if(targetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(fish.transform.position, targetPosition);
        }
    }

    public override void OnEnterState()
    {
        targetPosition = originalPosition;
        if (idleCoroutine != null)
        {
            fish.StopCoroutine(idleCoroutine);
            OnResetRotation();
        }
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
            fish.animator.SetBool("IsMoving", true);
            while (Vector3.Distance(targetPosition, fish.transform.position) > 0.05f)
            {
                fish.transform.position = Vector3.MoveTowards(fish.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            float maxDistanceX, maxDistanceY;
            GoLeft = !GoLeft;
            if(GoLeft)
            {
                maxDistanceX = originalPosition.x - x;
                maxDistanceY = originalPosition.y -y;
            }
            else
            {
                maxDistanceX = originalPosition.x + x;
                maxDistanceY = originalPosition.y + y;
            }
            maxDistanceX = Random.Range(originalPosition.x, maxDistanceX);
            maxDistanceY = Random.Range(originalPosition.y, maxDistanceY);
            targetPosition = new Vector3 (maxDistanceX, maxDistanceY);
            
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
