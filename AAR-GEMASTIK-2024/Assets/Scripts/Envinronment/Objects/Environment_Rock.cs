using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Rock : _EnvironmentBase, IInterractable
{
    PlayerCoreSystem _playerCoreSystem;
    PlayerInterractionSystem _playerInterractionSystem;
    private bool isBeingHeld;
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = false;
        _playerInterractionSystem = null;
        playerInterractionSystem.SetIsHolding(false);
        _playerCoreSystem = null;
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isBeingHeld = true;
        _playerInterractionSystem = playerInterractionSystem;
        playerInterractionSystem.SetIsHolding(true);
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        _playerCoreSystem = coreSystem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBeingHeld || _playerInterractionSystem == null) return;
        transform.position = _playerInterractionSystem.holdingObjectTransform.position;
    }
}
