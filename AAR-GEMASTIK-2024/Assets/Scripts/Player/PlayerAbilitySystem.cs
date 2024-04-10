using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitySystem : MonoBehaviour
{
    [SerializeField] private AbilitySO abilitySO;
    [SerializeField] private AbilityBase abilityBase;
    [SerializeField] private Transform abilityHolderPosition;

    private PlayerCoreSystem playerCoreSystem;

    private void Awake()
    {
        playerCoreSystem = GetComponent<PlayerCoreSystem>();
        if(abilitySO != null)
        {
            Transform abilityInstantiate = Instantiate(abilitySO.prefab, abilityHolderPosition);
            abilityBase = abilityInstantiate.GetComponent<AbilityBase>();
        }
    }
    private void Start()
    {
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        PlayerInputSystem.InvokeAbilityUsage += PlayerInputSystem_InvokeAbilityUsage;
    }

    private void PlayerCoreSystem_OnDead()
    {
        PlayerInputSystem.InvokeAbilityUsage -= PlayerInputSystem_InvokeAbilityUsage;
    }

    private void PlayerInputSystem_InvokeAbilityUsage()
    {
        if (abilityBase == null) return;
        abilityBase.Fire(playerCoreSystem);
    }
}
