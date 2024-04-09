using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    private DefaultInputAction playerInput;
    private PlayerCoreSystem coreSystem;

    public static event Action InvokeWeaponUsage;
    public static event Action InvokeAbilityUsage;
    private void Awake()
    {
        playerInput = new DefaultInputAction();
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerInput.Player.Enable();
    }
    private void Start()
    {
        playerInput.Player.InvokeWeaponUsage.performed += InvokeWeaponUsage_performed;
        playerInput.Player.InvokeAbilityUsage.performed += InvokeAbilityUsage_performed;
        coreSystem.OnDead += CoreSystem_OnDead;
    }
    private void CoreSystem_OnDead()
    {
        playerInput.Player.InvokeWeaponUsage.performed -= InvokeWeaponUsage_performed;
        playerInput.Player.InvokeAbilityUsage.performed -= InvokeAbilityUsage_performed;
    }
    private void InvokeAbilityUsage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InvokeAbilityUsage?.Invoke();
    }
    private void InvokeWeaponUsage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InvokeWeaponUsage?.Invoke();
    }

    public Vector2 GetMoveInput()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
}
