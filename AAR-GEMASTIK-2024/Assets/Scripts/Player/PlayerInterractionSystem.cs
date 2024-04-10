using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterractionSystem : MonoBehaviour
{
    //The Similarity of Interractable and Detectable is that they both can be detected
    //The difference is that Interractable needs to be interracted using input by the player
    //IDetectable will invoke their method when detected

    private PlayerCoreSystem coreSystem;
    
    [SerializeField] private Transform interractiveHolderPositionForInterractableObject;
    [SerializeField] private Vector3 boxSizeForInterractableObject;

    [SerializeField] private float radiusForDetectableObjects;

    private Collider[] detectionResult = new Collider[0];
    private IInterractable ClosestInterractableObject;
    private Vector3 OffSetPosition
    {
        get
        {
            Vector3 offSetPosition = new Vector3(boxSizeForInterractableObject.x * 0.25f, 0, 0);
            Vector3 positionWithOffset = interractiveHolderPositionForInterractableObject.position + offSetPosition;
            return positionWithOffset;
        }
    }
    [SerializeField] private LayerMask targetLayerMaskForInterractableObject;
    [SerializeField] private LayerMask targetLayerMaskForDetectableObject;
    
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        Debug.Log("Interraction is ON");
    }
    private void Start()
    {
        PlayerInputSystem.InvokeInterractUsage += PlayerInputSystem_InvokeInterractUsage;
    }

    private void PlayerInputSystem_InvokeInterractUsage()
    {
        if(ClosestInterractableObject != null)
        {
            ClosestInterractableObject.Interracted(this);
        }
    }

    private void Update()
    {
        DetectionForInterractableObject();
        //DetectionForDetectableObject();
        FindClosestInterractableObjectToPlayer();
    }
    private void DetectionForInterractableObject()
    {
        detectionResult = Physics.OverlapBox(OffSetPosition, boxSizeForInterractableObject, Quaternion.identity, targetLayerMaskForInterractableObject);
        if(detectionResult.Length > 0)
        {
            Debug.Log("Detect Something");
        }
    }
    private void DetectionForDetectableObject()
    {
        Collider[] detectableObjects = Physics.OverlapSphere(transform.position, radiusForDetectableObjects, targetLayerMaskForDetectableObject);
        foreach(Collider collider in detectableObjects)
        {
            if(collider.TryGetComponent<IDetectable>(out IDetectable detectable))
            {
                Debug.Log("Detectable Object Found " + collider.gameObject.name);
            }
        }

    }
    private void FindClosestInterractableObjectToPlayer()
    {
        if (detectionResult.Length <= 0) return;
        float closestTotalDistance = float.MaxValue;
        foreach(Collider coll in detectionResult)
        {
            float currentObjectDistance = Vector3.Distance(interractiveHolderPositionForInterractableObject.position, coll.transform.position);
            if(currentObjectDistance < closestTotalDistance && coll.TryGetComponent<IInterractable>(out IInterractable interractableObject))
            {
                ClosestInterractableObject = interractableObject;
            }
        }
        Debug.Log(ClosestInterractableObject);
    }
    private void OnDrawGizmos()
    {
        if (interractiveHolderPositionForInterractableObject == null) return;
        Gizmos.color = detectionResult.Length != 0? Color.green: Color.red;
        Gizmos.DrawWireCube(OffSetPosition, boxSizeForInterractableObject);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusForDetectableObjects);
    }
}
