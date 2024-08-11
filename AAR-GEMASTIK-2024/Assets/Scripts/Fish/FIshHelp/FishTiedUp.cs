using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FishTiedUp : FishBaseNeedHelp
{
    public float percentageDuration { get => currentDuration / maxDuration; }
    [SerializeField] private Transform netMask;
    [SerializeField] private float maxDuration;
    [SerializeField] private float currentDuration;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private SpriteRenderer netVisual;
    [SerializeField] private Animator animator;
    private bool isDoneHelped;
    private void Awake()
    {
        currentDuration = 0;
    }
    public override void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }

    public override void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }

    public override void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (isDoneHelped) return;
        playerCoreSystem = coreSystem;
        if (playerCoreSystem == null)
        {
            currentDuration = 0;
            InvokeOnBeingNoticed();
        }
    }

    void Update()
    {
        if(playerCoreSystem != null && currentDuration != maxDuration && !isDoneHelped)
        {
            currentDuration += Time.deltaTime;
            currentDuration = Mathf.Clamp(currentDuration, 0, maxDuration);
            if (currentDuration >= maxDuration)
            {
                currentDuration = maxDuration;
                isDoneHelped = true;
                netMask.gameObject.SetActive(false);
                InvokeBroadcastGettingHelpDone();
                hasBeenHelped = true;
                animator.SetTrigger("Happy");
                OnGetHelped();
            }
            InvokeOnGettingHelp();
        }
    }
    private async void OnGetHelped()
    {
        await Task.Delay(400);
        Vector3 targetPosition = transform.position + new Vector3(0,10,0);
        Color oldColorValue = visual.color;
        float a_value = oldColorValue.a;
        Color newColorvalue = new Color();
        float currentDuration = 0;
        float maxDuration = 1.3f;
        while (visual.color.a > 0)
        {
            if (currentDuration > maxDuration) break;
            currentDuration += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3.3f * Time.fixedDeltaTime);
            float new_alpha_value = Mathf.Lerp(a_value, 0, currentDuration / maxDuration);
            newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, new_alpha_value);
            visual.color = newColorvalue;
            await Task.Yield();
        }
        newColorvalue = new Color(oldColorValue.r, oldColorValue.g, oldColorValue.b, 0);
        visual.color = newColorvalue;
        GetComponent<Collider2D>().enabled = false;
    }

}
