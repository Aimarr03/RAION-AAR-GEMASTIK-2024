using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public AbilitySO abilitySO;
    protected bool isCooldown;
    [SerializeField] protected float intervalCooldown;

    public abstract void Fire(PlayerCoreSystem playerCoreSystem);
    public abstract IEnumerator OnCooldown();

}
