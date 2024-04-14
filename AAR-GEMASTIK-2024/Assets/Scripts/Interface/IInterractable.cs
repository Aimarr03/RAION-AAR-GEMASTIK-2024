using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterractable 
{
    public void Interracted(PlayerInterractionSystem playerInterractionSystem);

    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem);
    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem);
}
