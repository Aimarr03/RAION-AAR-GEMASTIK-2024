using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreSystem : MonoBehaviour
{
    public PlayerMoveSystem moveSystem;
    public PlayerInputSystem inputSystem;

    private void Awake()
    {
        moveSystem = GetComponent<PlayerMoveSystem>();
        inputSystem = GetComponent<PlayerInputSystem>();
    }
}
