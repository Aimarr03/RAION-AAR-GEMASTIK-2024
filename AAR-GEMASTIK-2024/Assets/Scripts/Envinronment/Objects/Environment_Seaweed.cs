using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Seaweed : _EnvironmentBase
{
    private float originalLinearMovement, originalRotatingMovement;
    [Range(0,0.75f)][SerializeField] private float slowMultiplier;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {
            Debug.Log("Player enter the seaweed ");
            coreSystem.moveSystem.SetIsSlowed(true, slowMultiplier);
            /*coreSystem.moveSystem.GetMovement(out float linearValue, out float angularValue);
            originalLinearMovement = linearValue;
            originalRotatingMovement = angularValue;
            coreSystem.moveSystem.SetMovement(originalLinearMovement * slowMultiplier, originalRotatingMovement * slowMultiplier);*/
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {
            Debug.Log("Player exit the seaweed ");
            coreSystem.moveSystem.SetIsSlowed(false, 1f);
            /*coreSystem.moveSystem.SetMovement(originalLinearMovement, originalRotatingMovement);*/
        }
    }
    
}
