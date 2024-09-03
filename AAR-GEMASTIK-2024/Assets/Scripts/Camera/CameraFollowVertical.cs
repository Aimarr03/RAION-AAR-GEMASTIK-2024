using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowVertical : MonoBehaviour
{
    private PlayerCoreSystem playerCoreSystem;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake()
    {
        playerCoreSystem = FindAnyObjectByType<PlayerCoreSystem>();
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        if (cinemachineVirtualCamera.enabled)
        {
            float playerPositionY = playerCoreSystem.transform.position.y;
            Vector3 newPosition = transform.position;
            newPosition.y = playerPositionY;
            transform.position = newPosition;
        }
    }
}
