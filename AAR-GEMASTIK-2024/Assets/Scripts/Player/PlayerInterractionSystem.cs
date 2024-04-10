using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterractionSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    
    [SerializeField] private Transform interractiveHolderPosition;
    [SerializeField] private Vector3 boxSize;

    private Collider[] detectionResult = new Collider[0];
    private Vector3 OffSetPosition
    {
        get
        {
            Vector3 offSetPosition = new Vector3(boxSize.x * 0.25f, 0, 0);
            Vector3 positionWithOffset = interractiveHolderPosition.position + offSetPosition;
            return positionWithOffset;
        }
    }
    [SerializeField] private LayerMask targetLayerMask;
    
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        Debug.Log("Interraction is ON");
    }
    private void Update()
    {
        Detection();
    }
    private void Detection()
    {
        detectionResult = Physics.OverlapBox(OffSetPosition, boxSize, Quaternion.identity, targetLayerMask);
        if(detectionResult.Length > 0)
        {
            Debug.Log("Detect Something");
        }
    }
    private void OnDrawGizmos()
    {
        if (interractiveHolderPosition == null) return;
        Gizmos.color = detectionResult.Length != 0? Color.green: Color.red;
        Gizmos.DrawWireCube(OffSetPosition, boxSize);
    }
}
