using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDetectable : TrashBase, IDetectable
{
    [SerializeField] protected float movementSpeed;
    public void DetectedByPlayer(PlayerCoreSystem playerCoreSystem)
    {
        this.playerCoreSystem = playerCoreSystem;
        //MoveTowards();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
        {
            OnTakenByPlayer();
        }
    }
    private void Update()
    {
        MoveTowards();
    }
    private void MoveTowards()
    {
        if (playerCoreSystem == null) return;
        Vector3 direction = (playerCoreSystem.transform.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }


}
