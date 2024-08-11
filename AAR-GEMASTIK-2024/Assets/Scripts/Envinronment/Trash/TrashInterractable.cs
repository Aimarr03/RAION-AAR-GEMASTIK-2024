using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInterractable : TrashBase, IInterractable
{
    [SerializeField] private AudioClip OnTakenAudio;
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        return;
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        OnTakenByPlayer();
        AudioManager.Instance?.PlaySFX(OnTakenAudio);
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        playerCoreSystem = coreSystem;
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
