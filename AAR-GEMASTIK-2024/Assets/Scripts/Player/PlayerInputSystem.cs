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
    public static event Action InvokeInterractUsage;
    public static event Action OnReleasedInvokeAbilityUsage;
    private void Awake()
    {
        playerInput = new DefaultInputAction();
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerInput.Player.Enable();
    }
    private void Start()
    {
        playerInput.Player.InvokeWeaponUsage.performed += InvokeWeaponUsage_performed;
        playerInput.Player.InvokeWeaponUsage.canceled += InvokeAbilityUsage_canceled;
        playerInput.Player.InvokeAbilityUsage.performed += InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed += InvokeInterract_performed;
        coreSystem.OnDead += CoreSystem_OnDead;
    }

    private void InvokeAbilityUsage_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("button is realeased");
        OnReleasedInvokeAbilityUsage?.Invoke();
    }

    private void InvokeInterract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InvokeInterractUsage?.Invoke();
    }

    private void CoreSystem_OnDead()
    {
        playerInput.Player.InvokeWeaponUsage.performed -= InvokeWeaponUsage_performed;
        playerInput.Player.InvokeAbilityUsage.performed -= InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed -= InvokeInterract_performed;
        playerInput.Player.InvokeWeaponUsage.canceled -= InvokeAbilityUsage_canceled;
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
