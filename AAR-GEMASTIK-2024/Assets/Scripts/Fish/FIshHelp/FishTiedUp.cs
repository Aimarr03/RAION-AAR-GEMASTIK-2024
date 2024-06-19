using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTiedUp : FishBaseNeedHelp
{
    public float percentageDuration { get => currentDuration / maxDuration; }
    [SerializeField] private Transform netMask;
    [SerializeField] private float maxDuration;
    [SerializeField] private float currentDuration;
    [SerializeField] private SpriteRenderer netVisual;
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
            }
            InvokeOnGettingHelp();
        }
        
    }

}
