using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInterractable : TrashBase, IInterractable
{
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        return;
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        OnTakenByPlayer();
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        playerCoreSystem = coreSystem;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        
    }
}
