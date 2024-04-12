using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_FishLongLineTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player enter the wiretrap");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {
            
            coreSystem.TakeDamage(damage);
        }
    }
    
}
