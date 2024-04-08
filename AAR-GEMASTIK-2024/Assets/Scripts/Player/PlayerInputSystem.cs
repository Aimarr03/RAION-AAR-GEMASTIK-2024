using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    private DefaultInputAction playerInput;
    private PlayerCoreSystem coreSystem;

    private void Awake()
    {
        playerInput = new DefaultInputAction();
        coreSystem = new PlayerCoreSystem();
        playerInput.Player.Enable();
    }

    public Vector2 GetMoveInput()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
}
