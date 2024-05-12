using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitySystem : MonoBehaviour
{
    [SerializeField] private AbilitySO abilitySO;
    [SerializeField] private AbilityBase abilityBase;
    [SerializeField] private Transform abilityHolderPosition;

    private PlayerCoreSystem playerCoreSystem;
    public event Action<float> OnDoneInvokingAbility;

    private void Awake()
    {
        playerCoreSystem = GetComponent<PlayerCoreSystem>();
        if(GameManager.Instance != null)
        {
            abilitySO = GameManager.Instance.chosenAbilitySO;
            Transform abilityInstantiate = Instantiate(abilitySO.prefab, abilityHolderPosition);
            abilityBase = abilityInstantiate.GetComponent<AbilityBase>();
            abilityBase.SetPlayerCoreSystem(playerCoreSystem);
            abilityBase.SetUpData();
            return;
        }
        if(abilitySO != null && abilityBase == null)
        {
            Transform abilityInstantiate = Instantiate(abilitySO.prefab, abilityHolderPosition);
            abilityBase = abilityInstantiate.GetComponent<AbilityBase>();
            abilityBase.SetPlayerCoreSystem(playerCoreSystem);
            abilityBase.SetUpData();
        }
    }
    private void Start()
    {
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
        if (abilityBase.isInvokable)
        {
            PlayerInputSystem.InvokeAbilityUsage += PlayerInputSystem_InvokeAbilityUsage;
        }
    }

    private void PlayerCoreSystem_OnDead()
    {
        if (abilityBase.isInvokable)
        {
            PlayerInputSystem.InvokeAbilityUsage -= PlayerInputSystem_InvokeAbilityUsage;
        }
    }

    private void PlayerInputSystem_InvokeAbilityUsage()
    {
        if (abilityBase == null) return;
        abilityBase.Fire(playerCoreSystem);
    }
    public void SetUpAbilitySO(AbilitySO abilitySO)
    {
        this.abilitySO = abilitySO;
        Transform abilityInstantiate = Instantiate(abilitySO.prefab, abilityHolderPosition);
        abilityBase = abilityInstantiate.GetComponent<AbilityBase>();
        abilityBase.SetPlayerCoreSystem(playerCoreSystem);
        abilityBase.SetUpData();
    }
    public AbilitySO GetAbilitySO()
    {
        return abilitySO;
    }
    public void TriggerDoneInvokingAbility(float duration) => OnDoneInvokingAbility?.Invoke(duration);
}
