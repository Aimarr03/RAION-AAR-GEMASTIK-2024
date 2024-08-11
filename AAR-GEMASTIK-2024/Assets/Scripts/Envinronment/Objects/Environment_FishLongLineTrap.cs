using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_FishLongLineTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {

            coreSystem.TakeDamage(damage);
        }
    }
    
}
