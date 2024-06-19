using System.Collections;
using UnityEngine;


public class Panic02State : FishBaseState
{
    private float speed;
    private float radiusCheck;
    private bool OnHidingState;
    private Vector3 originalPosition;
    private PlayerCoreSystem coreSystem;
    public FishBaseState nextState;
    public Panic02State(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, float speed, float radiusCheck) : base(fish, fsm, playerMask)
    {
        originalPosition = fish.transform.position;
        this.speed = speed;
        this.radiusCheck = radiusCheck;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        OnHidingState = false;
        Collider[] colliders = Physics.OverlapSphere(fish.transform.position, 20f, playerMask);
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out PlayerCoreSystem coreSystem))
            {
                this.coreSystem = coreSystem;
                Vector3 direction = (coreSystem.transform.position - fish.transform.position).normalized;
                fish.StartCoroutine(StartToHide(direction));
            }
        }
    }

    private IEnumerator StartToHide(Vector3 direction)
    {
        yield return new WaitForSeconds(1.3f);
        float rotation_on_y_axis = direction.x > 0 ? 0 : 180;
        float a_value = fish.spriteRenderer.color.a;
        fish.transform.rotation = Quaternion.Euler(fish.transform.rotation.x, rotation_on_y_axis, fish.transform.rotation.z);
        Color oldColorValue = fish.spriteRenderer.color;
        Color newColorvalue = new Color();

        float currentDuration = 0;
        float maxDuration = 0.33f;
        while (fish.spriteRenderer.color.a > 0.75 && Vector3.Distance(fish.transform.position, originalPosition) > 0.05f)
        {
            if (currentDuration > maxDuration) break;
            currentDuration += Time.deltaTime;
            fish.transform.position = Vector3.MoveTowards(fish.transform.position, originalPosition, speed * Time.fixedDeltaTime);
            float new_alpha_value = Mathf.Lerp(a_value, 0, currentDuration / maxDuration);
            newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, new_alpha_value);
            fish.spriteRenderer.color = newColorvalue;
            yield return null;
        }
        newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, 0);
        fish.spriteRenderer.color = newColorvalue;
        fish.sphereCollider.enabled = false;
        fish.UI.gameObject.SetActive(false);
        OnHidingState = true;
    }

    public override void OnExitState()
    {
        OnHidingState = false;
        Color oldColorValue = fish.spriteRenderer.color;
        Color newColorValue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, 255);
        fish.spriteRenderer.color = newColorValue;
        fish.sphereCollider.enabled = true;
        fish.UI.gameObject.SetActive(true);
    }

    public override void OnUpdateState()
    {
        if (OnHidingState)
        {
            OnHidingStateBehaviour();
        }
    }
    private void OnHidingStateBehaviour()
    {
        if (OnHidingState)
        {
            if (Vector3.Distance(fish.transform.position, coreSystem.transform.position) > radiusCheck) fsm.OnTransitionState(nextState);
        }
    }
}
