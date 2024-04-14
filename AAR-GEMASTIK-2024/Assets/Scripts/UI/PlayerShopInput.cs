using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopInput : MonoBehaviour
{
    [SerializeField] private Image pointer;
    [SerializeField] private float movementPointer;

    private DefaultInputAction inputAction;

    private void Awake()
    {
        inputAction = new DefaultInputAction();
        inputAction.Player.Enable();
    }
    private void Start()
    {
        inputAction.Player.InvokeWeaponUsage.performed += InvokeWeaponUsage_performed;
    }
    private void Update()
    {
        Vector2 input = inputAction.Player.Move.ReadValue<Vector2>();
        pointer.transform.position += (Vector3)input * movementPointer * 100 * Time.deltaTime;
    }
    private void InvokeWeaponUsage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
    }
}
