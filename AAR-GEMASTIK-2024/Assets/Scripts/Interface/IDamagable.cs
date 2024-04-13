using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);
    public void AddSuddenForce(Vector3 directiom, float forcePower);
    public void OnDisableMove(float moveDuration);
}
