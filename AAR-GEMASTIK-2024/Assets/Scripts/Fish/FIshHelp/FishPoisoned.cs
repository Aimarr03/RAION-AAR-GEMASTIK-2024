using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPoisoned : FishBaseNeedHelp, IDelivarable
{
    private bool IsDelivered = false;
    public float weight;
    private bool isBeingHeld = false;
    public override void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = false;
        playerInterractionSystem.SetIsHolding(false);
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnDecreaseValue(weight);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ExpedictionManager expedictionManager))
            {
                expedictionManager.OnGainCaughtFish(this);
            }
        }
    }

    public override void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (IsDelivered) return;
        isBeingHeld = true;
        playerInterractionSystem.SetIsHolding(true);
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnIncreaseValue(weight);
    }

    public override void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (IsDelivered) return;
        playerCoreSystem = coreSystem;
        if(!isBeingHeld) InvokeOnBeingNoticed();
    }
    private void Update()
    {
        if (IsDelivered) return;
        if (isBeingHeld) OnBeingHeld();    
    }
    private void OnBeingHeld()
    {
        if (IsDelivered) return;
        if (playerCoreSystem == null) return;
        transform.position = playerCoreSystem.transform.position;
    }

    public DelivarableType GetDelivarableType() => DelivarableType.Poisoned;

    public int GetBounty() => bounty;

    public void OnDeloading()
    {
        GetComponent<Collider>().enabled = false;
        int childrenCount = transform.childCount;
        for(int index = 0; index < childrenCount; index++)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }
    }

    public void OnDelivered()
    {
        OnDeloading();
        IsDelivered = true;
        InvokeOnGettingHelp();
        InvokeBroadcastGettingHelpDone();
        hasBeenHelped = true;
    }
}
