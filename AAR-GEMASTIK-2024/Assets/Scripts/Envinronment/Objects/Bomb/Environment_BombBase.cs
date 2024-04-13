using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Environment_BombBase : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float ForceExplosion;
    [SerializeField] protected float radiusExplosion;
    [SerializeField] protected LayerMask damagableLayer;
    [SerializeField] protected float disabledDuration;
    [SerializeField] protected int maxAttemptToRecover;
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagableUnit))
        {
            OnExplode();
        }
    }
    protected virtual void OnExplode()
    {
        Collider[] unitWithinExplosionRadius = Physics.OverlapSphere(transform.position, radiusExplosion);
        foreach(Collider unit in unitWithinExplosionRadius)
        {
            if(unit.gameObject.TryGetComponent(out IDamagable damagableUnit))
            {
                Transform damagableUnitTransform = unit.transform;
                Vector3 direction = (damagableUnitTransform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, damagableUnitTransform.position);
                float normalizedDistance = 1 - Mathf.Clamp01(distance / radiusExplosion); 
                float forceMultiplier = Mathf.Lerp(1f, 0.1f, normalizedDistance); 
                float totalPowerForce = ForceExplosion * forceMultiplier;
                Debug.Log("Total Power Force " + totalPowerForce);
                damagableUnit.TakeDamage(damage);
                damagableUnit.AddSuddenForce(direction, totalPowerForce);
                damagableUnit.OnDisableMove(disabledDuration, maxAttemptToRecover);
            }
        }
    }
}
