using Cinemachine;
using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera ActiveCameraFollowPlayer;
    public CinemachineVirtualCamera BufferCamera;
    public CinemachineVirtualCamera cameraSeeShip;
    public CinemachineVirtualCamera cameraSeeShark;
    private List<CinemachineVirtualCamera> cameraList;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        cameraList = new List<CinemachineVirtualCamera>
        {
            ActiveCameraFollowPlayer,
            cameraSeeShip, cameraSeeShark,
        };
        ConversationManager.OnConversationEnded += OnEndedConversation;
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= OnEndedConversation;
    }
    private void OnEndedConversation()
    {
        if(!ActiveCameraFollowPlayer.enabled)
        {
            foreach(var camera in cameraList)
            {
                camera.enabled = false;
            }
            ActiveCameraFollowPlayer.enabled = true;
        }
    }
    public void OnSwapCameras(CameraControlComponent component, Vector3 direction)
    {
        Debug.Log("Attempt to Swap Cameras");
        Debug.Log("Direction " + direction);
        component.leftCamera.enabled = !(direction.x < 0);
        component.rightCamera.enabled = !(direction.x > 0);
        ActiveCameraFollowPlayer = component.leftCamera.enabled ? component.leftCamera : component.rightCamera;
        BufferCamera = ActiveCameraFollowPlayer;
    }
    public void OnSwapCamerasToSeeShip(bool yes)
    {
        ActiveCameraFollowPlayer.enabled = !yes;
        cameraSeeShip.enabled = yes;
    }
    public void OnSwapCamerasToSeeShark(bool yes)
    {
        ActiveCameraFollowPlayer.enabled = !yes;
        cameraSeeShark.enabled = yes;
    }
}