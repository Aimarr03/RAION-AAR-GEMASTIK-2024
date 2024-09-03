using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraControllerTrigger : MonoBehaviour
{
    [Header("Camera Control Component")]
    public CameraControlComponent components = new CameraControlComponent();
    public Collider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D player)
    {
        Debug.Log("Trigger Enter");
        if (player.TryGetComponent(out PlayerCoreSystem PlayerCore))
        {
            Vector3 direction = (player.transform.position - boxCollider.bounds.center).normalized;
            Debug.Log(direction);
            if (components.swapCameras)
            {
                CameraManager.instance.OnSwapCameras(components, direction);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D player)
    {
        Debug.Log("Trigger Enter");
        if (player.TryGetComponent(out PlayerCoreSystem PlayerCore))
        {
            Vector3 direction = (boxCollider.bounds.center - player.transform.position).normalized;
            Debug.Log(direction);
            if (components.swapCameras)
            {
                CameraManager.instance.OnSwapCameras(components, direction);
            }
        }
    }
}
[System.Serializable]
public class CameraControlComponent
{
    public bool swapCameras = false;
    public bool panCamera = false;

    [HideInInspector] public CinemachineVirtualCamera leftCamera;
    [HideInInspector] public CinemachineVirtualCamera rightCamera;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance;
    [HideInInspector] public float panTime;
}

public enum PanDirection
{
    Up, Right, Down, Left
}
#if UNITY_EDITOR
[CustomEditor(typeof(CameraControllerTrigger))]
public class GUI_Editor_CameraControlComponent : Editor
{
    CameraControllerTrigger camera;
    private void OnEnable()
    {
        camera = (CameraControllerTrigger)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (camera.components.swapCameras)
        {
            camera.components.leftCamera = EditorGUILayout.ObjectField("Camera on Left", camera.components.leftCamera,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            camera.components.rightCamera = EditorGUILayout.ObjectField("Camera on Right", camera.components.rightCamera,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if (camera.components.panCamera)
        {
            camera.components.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", camera.components.panDirection);
            camera.components.panDistance = EditorGUILayout.FloatField("Pan Distance", camera.components.panDistance);
            camera.components.panTime = EditorGUILayout.FloatField("Pan Time", camera.components.panTime);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(camera);
        }
    }
}
#endif
