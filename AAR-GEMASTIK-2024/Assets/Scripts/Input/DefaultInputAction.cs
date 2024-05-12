//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Input/DefaultInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DefaultInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultInputAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b3487e32-bd45-4a41-9d53-13d2200e8caf"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c0d25dfb-1f8e-4137-bfef-1e39711ac634"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InvokeWeaponUsage"",
                    ""type"": ""Button"",
                    ""id"": ""2a0a042f-7b4d-49b2-89d4-dce7ff077278"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InvokeAbilityUsage"",
                    ""type"": ""Button"",
                    ""id"": ""5657b4aa-dfe4-4e12-8636-b1c78c20fd5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InvokeInterract"",
                    ""type"": ""Button"",
                    ""id"": ""e70be2cb-4a29-4746-95ae-2d38ee6e8b2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InvokeFlashlight"",
                    ""type"": ""Button"",
                    ""id"": ""187f66d1-d6d3-4e25-a275-0ad01e7b180f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InvokeSwitchItemFocus"",
                    ""type"": ""Button"",
                    ""id"": ""8890a046-9c1a-41c6-b6af-a2b56e2b5bd7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InvokeUseItem"",
                    ""type"": ""Button"",
                    ""id"": ""6c3e5977-35fd-4fab-b21a-08fd9d03c585"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""88d211bc-1055-4801-b6c2-85755272b47b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2c458a0b-5c8a-4478-868c-029db2e85a08"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DefaultControl"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ddbf3888-e0a6-4014-9efe-ec6a53a442d6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DefaultControl"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""13108232-e84c-4182-9173-2ee4c5f1eeb8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DefaultControl"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e81f2872-5576-4335-bdca-080e0e028e15"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DefaultControl"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f170d91f-4b1f-4501-95cd-114f8b6d8ef0"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeWeaponUsage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59c8d5e0-e45d-4b58-9eb2-a59ec3fe8146"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeAbilityUsage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1cf9643-4c55-423f-8d9b-e5d8cf4d10ee"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeInterract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db98813-6e9d-470f-9ace-4b93576c561c"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeFlashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3eb6e01-e4c4-49a8-8115-e98091bd3cd4"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeSwitchItemFocus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e24aa330-e6e1-46ee-84dd-f677c802d60d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InvokeUseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""DefaultControl"",
            ""bindingGroup"": ""DefaultControl"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_InvokeWeaponUsage = m_Player.FindAction("InvokeWeaponUsage", throwIfNotFound: true);
        m_Player_InvokeAbilityUsage = m_Player.FindAction("InvokeAbilityUsage", throwIfNotFound: true);
        m_Player_InvokeInterract = m_Player.FindAction("InvokeInterract", throwIfNotFound: true);
        m_Player_InvokeFlashlight = m_Player.FindAction("InvokeFlashlight", throwIfNotFound: true);
        m_Player_InvokeSwitchItemFocus = m_Player.FindAction("InvokeSwitchItemFocus", throwIfNotFound: true);
        m_Player_InvokeUseItem = m_Player.FindAction("InvokeUseItem", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_InvokeWeaponUsage;
    private readonly InputAction m_Player_InvokeAbilityUsage;
    private readonly InputAction m_Player_InvokeInterract;
    private readonly InputAction m_Player_InvokeFlashlight;
    private readonly InputAction m_Player_InvokeSwitchItemFocus;
    private readonly InputAction m_Player_InvokeUseItem;
    public struct PlayerActions
    {
        private @DefaultInputAction m_Wrapper;
        public PlayerActions(@DefaultInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @InvokeWeaponUsage => m_Wrapper.m_Player_InvokeWeaponUsage;
        public InputAction @InvokeAbilityUsage => m_Wrapper.m_Player_InvokeAbilityUsage;
        public InputAction @InvokeInterract => m_Wrapper.m_Player_InvokeInterract;
        public InputAction @InvokeFlashlight => m_Wrapper.m_Player_InvokeFlashlight;
        public InputAction @InvokeSwitchItemFocus => m_Wrapper.m_Player_InvokeSwitchItemFocus;
        public InputAction @InvokeUseItem => m_Wrapper.m_Player_InvokeUseItem;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @InvokeWeaponUsage.started += instance.OnInvokeWeaponUsage;
            @InvokeWeaponUsage.performed += instance.OnInvokeWeaponUsage;
            @InvokeWeaponUsage.canceled += instance.OnInvokeWeaponUsage;
            @InvokeAbilityUsage.started += instance.OnInvokeAbilityUsage;
            @InvokeAbilityUsage.performed += instance.OnInvokeAbilityUsage;
            @InvokeAbilityUsage.canceled += instance.OnInvokeAbilityUsage;
            @InvokeInterract.started += instance.OnInvokeInterract;
            @InvokeInterract.performed += instance.OnInvokeInterract;
            @InvokeInterract.canceled += instance.OnInvokeInterract;
            @InvokeFlashlight.started += instance.OnInvokeFlashlight;
            @InvokeFlashlight.performed += instance.OnInvokeFlashlight;
            @InvokeFlashlight.canceled += instance.OnInvokeFlashlight;
            @InvokeSwitchItemFocus.started += instance.OnInvokeSwitchItemFocus;
            @InvokeSwitchItemFocus.performed += instance.OnInvokeSwitchItemFocus;
            @InvokeSwitchItemFocus.canceled += instance.OnInvokeSwitchItemFocus;
            @InvokeUseItem.started += instance.OnInvokeUseItem;
            @InvokeUseItem.performed += instance.OnInvokeUseItem;
            @InvokeUseItem.canceled += instance.OnInvokeUseItem;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @InvokeWeaponUsage.started -= instance.OnInvokeWeaponUsage;
            @InvokeWeaponUsage.performed -= instance.OnInvokeWeaponUsage;
            @InvokeWeaponUsage.canceled -= instance.OnInvokeWeaponUsage;
            @InvokeAbilityUsage.started -= instance.OnInvokeAbilityUsage;
            @InvokeAbilityUsage.performed -= instance.OnInvokeAbilityUsage;
            @InvokeAbilityUsage.canceled -= instance.OnInvokeAbilityUsage;
            @InvokeInterract.started -= instance.OnInvokeInterract;
            @InvokeInterract.performed -= instance.OnInvokeInterract;
            @InvokeInterract.canceled -= instance.OnInvokeInterract;
            @InvokeFlashlight.started -= instance.OnInvokeFlashlight;
            @InvokeFlashlight.performed -= instance.OnInvokeFlashlight;
            @InvokeFlashlight.canceled -= instance.OnInvokeFlashlight;
            @InvokeSwitchItemFocus.started -= instance.OnInvokeSwitchItemFocus;
            @InvokeSwitchItemFocus.performed -= instance.OnInvokeSwitchItemFocus;
            @InvokeSwitchItemFocus.canceled -= instance.OnInvokeSwitchItemFocus;
            @InvokeUseItem.started -= instance.OnInvokeUseItem;
            @InvokeUseItem.performed -= instance.OnInvokeUseItem;
            @InvokeUseItem.canceled -= instance.OnInvokeUseItem;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_DefaultControlSchemeIndex = -1;
    public InputControlScheme DefaultControlScheme
    {
        get
        {
            if (m_DefaultControlSchemeIndex == -1) m_DefaultControlSchemeIndex = asset.FindControlSchemeIndex("DefaultControl");
            return asset.controlSchemes[m_DefaultControlSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInvokeWeaponUsage(InputAction.CallbackContext context);
        void OnInvokeAbilityUsage(InputAction.CallbackContext context);
        void OnInvokeInterract(InputAction.CallbackContext context);
        void OnInvokeFlashlight(InputAction.CallbackContext context);
        void OnInvokeSwitchItemFocus(InputAction.CallbackContext context);
        void OnInvokeUseItem(InputAction.CallbackContext context);
    }
}
