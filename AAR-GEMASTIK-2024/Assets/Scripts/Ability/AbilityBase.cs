using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public AbilitySO abilitySO;
    public bool isInvokable;
    protected bool isCooldown;
    protected PlayerCoreSystem playerCoreSystem;
    [SerializeField] protected float intervalCooldown;

    public abstract void Fire(PlayerCoreSystem playerCoreSystem);
    public abstract IEnumerator OnCooldown();
    public virtual void SetPlayerCoreSystem(PlayerCoreSystem playerCoreSystem)
    {
        this.playerCoreSystem = playerCoreSystem;
    }
    public void SetUpData()
    {
        intervalCooldown = abilitySO.cooldownDuration;
        isInvokable = abilitySO.isInvokable;
    }
}
