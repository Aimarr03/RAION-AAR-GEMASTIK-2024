using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrashBase : MonoBehaviour
{
    protected PlayerCoreSystem playerCoreSystem;
    [SerializeField] private float weight;
    protected abstract void OnCollisionEnter(Collision collision);

    public void OnTakenByPlayer()
    {
        if (playerCoreSystem == null) return;
        WeightSystem weightSystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity) as WeightSystem;
        if(!weightSystem.canAddWeight(weight)) return;
        weightSystem.OnIncreaseValue(weight);
        Debug.Log("Player receive trash");
        Destroy(gameObject);
    }
}
