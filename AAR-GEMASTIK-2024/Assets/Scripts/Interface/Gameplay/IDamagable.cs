using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);
    public IEnumerator GetSlowed(float duration, float multilpier);
    public void AddSuddenForce(Vector3 directiom, float forcePower);
    public void OnDisableMove(float moveDuration, int maxAttemptToRecover);
}
