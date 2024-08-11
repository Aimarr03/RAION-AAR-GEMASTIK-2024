using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicState : FishBaseState
{
    public FishBaseState nextState;
    private PlayerCoreSystem coreSystem;
    private float panicSpeed;
    private float radiusCheck;
    private bool onHidingState;
    
    public PanicState(FishNeutralBase fish, FishNeutralStateMachine fsm, LayerMask playerMask, float panicSpeed, float radiusCheck) : base(fish, fsm, playerMask)
    {
        this.panicSpeed = panicSpeed;
        this.radiusCheck = radiusCheck;
    }

    public override void OnDrawGizmos()
    {
        
    }

    public override void OnEnterState()
    {
        onHidingState = false;
        OnDetectPlayer();
    }

    public override void OnExitState()
    {
        onHidingState= false;
        Color oldColorValue = fish.spriteRenderer.color;
        Color newColorValue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, 255);
        fish.spriteRenderer.color = newColorValue;
        fish.fishCollider.enabled = true;
        fish.UI.gameObject.SetActive(true);
    }

    public override void OnUpdateState()
    {
        if(onHidingState) OnHidingBehaviour();
    }
    private void OnDetectPlayer()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(fish.transform.position, 16f, playerMask);
        if(collider.Length > 0 )
        {
            collider[0].TryGetComponent(out PlayerCoreSystem coreSystem);
            this.coreSystem = coreSystem;
        }
        Vector3 direction = (coreSystem.transform.position - fish.transform.position).normalized;
        fish.StartCoroutine(OnStartHiding(direction));
    }
    private IEnumerator OnStartHiding(Vector3 direction)
    {
        yield return new WaitForSeconds(1.3f);
        float a_value = fish.spriteRenderer.color.a;
        float x_value = direction.x > 0 ? -18 : 18;
        float y_value_rotation = direction.x > 0 ? 0 : 180;
        float currentDuration = 0;
        float maxDuration = 0.75f;
        
        Vector3 newDirectionToFlee = new Vector3(x_value, direction.y, direction.z);
        
        Quaternion fishQuaternion = fish.transform.rotation;
        fish.transform.rotation = Quaternion.Euler(fishQuaternion.x, y_value_rotation, fishQuaternion.z);
        
        Color oldColorValue = fish.spriteRenderer.color;
        Color newColorvalue = new Color();
        
        while (fish.spriteRenderer.color.a > 0)
        {
            if (currentDuration > maxDuration) break;
            currentDuration += Time.deltaTime;
            fish.transform.position = Vector3.MoveTowards(fish.transform.position, newDirectionToFlee, panicSpeed * Time.fixedDeltaTime);
            float new_alpha_value = Mathf.Lerp(a_value, 0, currentDuration / maxDuration);
            newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, new_alpha_value);
            fish.spriteRenderer.color = newColorvalue;
            yield return null;
        }
        newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, 0);
        fish.spriteRenderer.color = newColorvalue;
        fish.fishCollider.enabled = false;
        fish.UI.gameObject.SetActive(false);
        onHidingState = true;
    }
    private void OnHidingBehaviour()
    {
        if(Vector3.Distance(fish.transform.position, coreSystem.transform.position) > radiusCheck)
        {
            fsm.OnTransitionState(nextState);
        }
    }
}
