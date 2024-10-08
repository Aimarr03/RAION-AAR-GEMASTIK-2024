using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPoisoned : FishBaseNeedHelp, IDelivarable
{
    private bool IsDelivered = false;
    public float weight;
    private bool isBeingHeld = false;
    [SerializeField] private RectTransform Ui_Guide;
    public override void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = false;
        playerInterractionSystem.SetIsHolding(false);
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnDecreaseValue(weight);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4f);
        Ui_Guide.gameObject.SetActive(true);
        foreach (Collider2D collider in colliders)
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
        Ui_Guide.gameObject.SetActive(false);
        playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnIncreaseValue(weight);
    }

    public override void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (IsDelivered) return;
        if(playerCoreSystem != null)
        {
            if (playerCoreSystem.interractionSystem.IsHolding()) return;
        }
        Ui_Guide.gameObject.SetActive(coreSystem != null);
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
        transform.position = playerCoreSystem.interractionSystem.GetHolderPosition;
    }

    public DelivarableType GetDelivarableType() => DelivarableType.Poisoned;

    public int GetBounty() => bounty;

    public void OnDeloading()
    {
        GetComponent<Collider2D>().enabled = false;
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
