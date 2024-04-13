using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlBrake
{
    Right, Left
}
public class PlayerInputSystem : MonoBehaviour
{
    private DefaultInputAction playerInput;
    private PlayerCoreSystem coreSystem;

    public static event Action InvokeWeaponUsage;
    public static event Action InvokeAbilityUsage;
    public static event Action InvokeInterractUsage;
    public static event Action OnReleasedInvokeWeaponUsage;
    private void Awake()
    {
        playerInput = new DefaultInputAction();
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerInput.Player.Enable();
    }
    private void Start()
    {
        OnAddCallback();
        coreSystem.OnDead += CoreSystem_OnDead;
        coreSystem.OnDisabled += CoreSystem_OnDisabled;
    }

    private void CoreSystem_OnDisabled(bool obj)
    {
        if (obj) OnRemoveCallback();
        else OnAddCallback();
    }

    private void InvokeWeaponUsage_canceled(InputAction.CallbackContext obj)
    {
        Debug.Log("button is realeased");
        if (coreSystem.onDisabled) return;
        OnReleasedInvokeWeaponUsage?.Invoke();
    }

    private void InvokeInterract_performed(InputAction.CallbackContext obj)
    {
        if (coreSystem.onDisabled) return;
        InvokeInterractUsage?.Invoke();
    }
    private void OnAddCallback()
    {
        playerInput.Player.InvokeWeaponUsage.performed += InvokeWeaponUsage_performed;
        playerInput.Player.InvokeWeaponUsage.canceled += InvokeWeaponUsage_canceled;
        playerInput.Player.InvokeAbilityUsage.performed += InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed += InvokeInterract_performed;
    }
    private void OnRemoveCallback()
    {
        Debug.Log("Player cannot invoke anything");
        playerInput.Player.InvokeWeaponUsage.performed -= InvokeWeaponUsage_performed;
        playerInput.Player.InvokeAbilityUsage.performed -= InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed -= InvokeInterract_performed;
        playerInput.Player.InvokeWeaponUsage.canceled -= InvokeWeaponUsage_canceled;
    }
    private void CoreSystem_OnDead()
    {
        OnRemoveCallback();
    }
    private void InvokeAbilityUsage_performed(InputAction.CallbackContext obj)
    {
        if(coreSystem.onDisabled) return;
        InvokeAbilityUsage?.Invoke();
    }
    private void InvokeWeaponUsage_performed(InputAction.CallbackContext obj)
    {
        if (coreSystem.onDisabled) return;
        InvokeWeaponUsage?.Invoke();
    }

    public Vector2 GetMoveInput()
    {
        if(coreSystem.onDisabled) return Vector2.zero;
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
}
