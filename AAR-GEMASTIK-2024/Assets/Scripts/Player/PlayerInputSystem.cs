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
    public static event Action AttemptRecoverFromDisableStatus;
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
        if (obj)
        {
            OnRemoveCallback();
            playerInput.Player.Move.performed += AttemptToRecoverDisableStatus;
        }
        else
        {
            OnAddCallback();
            playerInput.Player.Move.performed -= AttemptToRecoverDisableStatus;
        }
    }

    private void AttemptToRecoverDisableStatus(InputAction.CallbackContext obj)
    {
        AttemptRecoverFromDisableStatus?.Invoke();
    }

    private void InvokeWeaponUsage_canceled(InputAction.CallbackContext obj)
    {
        Debug.Log("button is realeased");
        OnReleasedInvokeWeaponUsage?.Invoke();
    }

    private void InvokeInterract_performed(InputAction.CallbackContext obj)
    {
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
        InvokeAbilityUsage?.Invoke();
    }
    private void InvokeWeaponUsage_performed(InputAction.CallbackContext obj)
    {
        InvokeWeaponUsage?.Invoke();
    }

    public Vector2 GetMoveInput()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
}
