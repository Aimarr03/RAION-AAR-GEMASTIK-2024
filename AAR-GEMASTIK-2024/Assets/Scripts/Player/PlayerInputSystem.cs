using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public static event Action InvokeSwitchItemFocus;
    public static event Action InvokeUseItem;
    public static event Action<bool> InvokeMoveSoundAction;
    public static event Action InvokePause;
    private bool OnMovePressed = false;
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
        DialogueEditor.ConversationManager.OnConversationStarted += OnConversationStarted;
        DialogueEditor.ConversationManager.OnConversationEnded += OnConversationFinished;
    }
    private void OnDisable()
    {
        OnRemoveCallback();
        coreSystem.OnDead -= CoreSystem_OnDead;
        coreSystem.OnDisabled -= CoreSystem_OnDisabled;
        DialogueEditor.ConversationManager.OnConversationStarted -= OnConversationStarted;
        DialogueEditor.ConversationManager.OnConversationEnded -= OnConversationFinished;
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
    

    
    private void OnAddCallback()
    {
        Debug.Log("Player can invoke now");
        playerInput.Player.Enable();
        playerInput.Player.InvokeWeaponUsage.performed += InvokeWeaponUsage_performed;
        playerInput.Player.InvokeWeaponUsage.canceled += InvokeWeaponUsage_canceled;
        playerInput.Player.InvokeAbilityUsage.performed += InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed += InvokeInterract_performed;
        playerInput.Player.InvokeSwitchItemFocus.performed += InvokeSwitchItemFocus_performed;
        playerInput.Player.InvokeUseItem.performed += InvokeUseItem_performed;
        playerInput.Player.Move.performed += Move_performed;
        playerInput.Player.Move.canceled += Move_canceled;
        playerInput.Player.Pause.performed += Pause_performed;
    }

    private void OnRemoveCallback()
    {
        Debug.Log("Player cannot invoke anything");
        playerInput.Player.Disable();
        playerInput.Player.InvokeWeaponUsage.performed -= InvokeWeaponUsage_performed;
        playerInput.Player.InvokeAbilityUsage.performed -= InvokeAbilityUsage_performed;
        playerInput.Player.InvokeInterract.performed -= InvokeInterract_performed;
        playerInput.Player.InvokeWeaponUsage.canceled -= InvokeWeaponUsage_canceled;
        playerInput.Player.InvokeSwitchItemFocus.performed -= InvokeSwitchItemFocus_performed;
        playerInput.Player.InvokeUseItem.performed -= InvokeUseItem_performed;
        playerInput.Player.Move.performed -= Move_performed;
        playerInput.Player.Move.canceled -= Move_canceled;
        playerInput.Player.Pause.performed -= Pause_performed;
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
    private void InvokeUseItem_performed(InputAction.CallbackContext obj)
    {
        InvokeUseItem?.Invoke();
    }

    private void InvokeSwitchItemFocus_performed(InputAction.CallbackContext obj)
    {
        InvokeSwitchItemFocus?.Invoke();
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
    private void Move_canceled(InputAction.CallbackContext obj)
    {
        Vector2 input = playerInput.Player.Move.ReadValue<Vector2>();
        InvokeMoveSoundAction?.Invoke(false);
    }
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Invoke Pause");
        InvokePause?.Invoke();
    }
    private void Move_performed(InputAction.CallbackContext obj)
    {
        Vector2 input = playerInput.Player.Move.ReadValue<Vector2>();
        bool isPressing = input.x != 0;
        InvokeMoveSoundAction?.Invoke(isPressing);
    }
    public Vector2 GetMoveInput()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
    private void OnConversationStarted()
    {
        OnRemoveCallback();
    }
    private void OnConversationFinished()
    {
        OnAddCallback();
    }
}
